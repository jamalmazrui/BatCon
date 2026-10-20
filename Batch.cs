// Batch.cs -- BatCon's batch work: finding the source files, the formats they all
// convert to, a safe name for each result, and one conversion at a time.
//
// Everything about formats comes from the kit's Conversion class: which files it can
// convert (hasAnyRoute), what each converts to and its readable name
// (targetLabelsFor), and the conversion itself (convert, the one technique every Homer app uses). Nothing here reads
// conversions.inix, so BatCon offers exactly what the kit does.
//
// The path parsing, folder scan, naming and safe replacement are from ChatGPT's
// first BatCon, reviewed and kept (19 October 2026).

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Homer;

namespace BatCon
{
public static class Batch
{
    // count: the number with its noun matched to it, "1 file", "3 files".
    public static string count(int iCount, string sNoun)
    {
        return iCount + " " + sNoun + (iCount == 1 ? "" : "s");
    }

    // tokens: the paths in a Source files box, split on spaces outside quotes.
    public static List<string> tokens(string sText)
    {
        List<string> lsPaths = new List<string>();
        StringBuilder sbPath = new StringBuilder();
        bool bQuoted = false;
        foreach (char cChar in sText ?? "")
        {
            if (cChar == '"') { bQuoted = !bQuoted; continue; }
            if (char.IsWhiteSpace(cChar) && !bQuoted)
            {
                if (sbPath.Length > 0) { lsPaths.Add(sbPath.ToString()); sbPath.Length = 0; }
            }
            else sbPath.Append(cChar);
        }
        if (sbPath.Length > 0) lsPaths.Add(sbPath.ToString());
        return lsPaths;
    }

    // discover: every file the kit can convert, from files, folders and patterns.
    public static List<string> discover(IEnumerable<string> lsPaths, bool bRecursive, List<string> lsProblems)
    {
        HashSet<string> hsFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string sPath in lsPaths)
        {
            try
            {
                string sExpanded = Environment.ExpandEnvironmentVariables(sPath.Trim());
                if (sExpanded == "") continue;
                if (sExpanded.IndexOfAny(new char[] { '*', '?' }) >= 0)
                {
                    string sFolder = Path.GetDirectoryName(sExpanded);
                    sFolder = Path.GetFullPath(string.IsNullOrEmpty(sFolder) ? "." : sFolder);
                    if (!Directory.Exists(sFolder)) { lsProblems.Add("No folder " + sFolder); continue; }
                    scan(sFolder, Path.GetFileName(sExpanded), bRecursive, hsFiles, lsProblems);
                }
                else if (File.Exists(sExpanded))
                {
                    if (Conversion.hasAnyRoute(sExpanded)) hsFiles.Add(Path.GetFullPath(sExpanded));
                    else lsProblems.Add("BatCon cannot convert " + Path.GetFileName(sExpanded));
                }
                else if (Directory.Exists(sExpanded)) scan(Path.GetFullPath(sExpanded), "*", bRecursive, hsFiles, lsProblems);
                else lsProblems.Add("Not found: " + sExpanded);
            }
            catch (Exception oError) { lsProblems.Add(sPath + ": " + oError.Message); }
        }
        List<string> lsFiles = new List<string>(hsFiles);
        lsFiles.Sort(StringComparer.OrdinalIgnoreCase);
        return lsFiles;
    }

    // scan: the convertible files in one folder, and below it when asked, never
    // following a folder link, which can loop.
    private static bool scan(string sFolder, string sPattern, bool bRecursive, HashSet<string> hsFiles, List<string> lsProblems)
    {
        try
        {
            foreach (string sFile in Directory.GetFiles(sFolder, sPattern))
                if (Conversion.hasAnyRoute(sFile)) hsFiles.Add(Path.GetFullPath(sFile));
            if (bRecursive)
                foreach (string sChild in Directory.GetDirectories(sFolder))
                    if ((File.GetAttributes(sChild) & FileAttributes.ReparsePoint) == 0) scan(sChild, sPattern, true, hsFiles, lsProblems);
        }
        catch (Exception oError) { lsProblems.Add(sFolder + ": " + oError.Message); }
        return true;
    }

    // targets: the formats every source converts to, as extensions, with the kit's
    // readable label for each, in the kit's order for the first file.
    public static List<string> targets(List<string> lsFiles, out List<string> lsLabels)
    {
        lsLabels = new List<string>();
        List<string> lsTargets = new List<string>();
        if (lsFiles.Count == 0) return lsTargets;
        HashSet<string> hsCommon = null;
        Dictionary<string, string> dLabels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> hsTypesSeen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        string[] aFirstExtensions = null;
        foreach (string sFile in lsFiles)
        {
            // One file of each source type is enough: the targets depend on the type.
            if (!hsTypesSeen.Add(Conversion.typeOf(sFile))) continue;
            string[] aExtensions;
            string[] aLabels = Conversion.targetLabelsFor(sFile, out aExtensions);
            for (int i = 0; i < aExtensions.Length; i++) if (!dLabels.ContainsKey(aExtensions[i])) dLabels[aExtensions[i]] = aLabels[i];
            if (hsCommon == null) { hsCommon = new HashSet<string>(aExtensions, StringComparer.OrdinalIgnoreCase); aFirstExtensions = aExtensions; }
            else hsCommon.IntersectWith(aExtensions);
        }
        if (hsCommon == null) return lsTargets;
        foreach (string sExtension in aFirstExtensions)
        {
            if (!hsCommon.Contains(sExtension) || lsTargets.Contains(sExtension)) continue;
            lsTargets.Add(sExtension);
            lsLabels.Add(dLabels[sExtension]);
        }
        return lsTargets;
    }

    // destination: the result's path, never a source and never a name already given
    // to another result in this batch: name.md, then name_2.md, name_3.md.
    public static string destination(string sSource, string sFolder, string sType, HashSet<string> hsReserved, HashSet<string> hsSources)
    {
        string sStem = Path.GetFileNameWithoutExtension(sSource);
        string sResult = Path.GetFullPath(Path.Combine(sFolder, sStem + "." + sType));
        int iSuffix = 2;
        while (hsReserved.Contains(sResult) || hsSources.Contains(sResult))
            sResult = Path.GetFullPath(Path.Combine(sFolder, sStem + "_" + iSuffix++ + "." + sType));
        hsReserved.Add(sResult);
        return sResult;
    }

    // convertOne: one conversion, written to a temporary file in the same folder and
    // moved into place only when it succeeds, so a failure never leaves half a file
    // or loses an earlier result.
    public static string convertOne(string sSource, string sResult, bool bForce)
    {
        if (File.Exists(sResult) && !bForce) return "Skipped " + Path.GetFileName(sResult) + ": it is already there.";
        string sFolder = Path.GetDirectoryName(sResult);
        string sTemp = Path.Combine(sFolder, ".batcon-" + Guid.NewGuid().ToString("N") + Path.GetExtension(sResult));
        try
        {
            Directory.CreateDirectory(sFolder);
            string sError;
            if (!Conversion.convert(sSource, sTemp, out sError)) throw new IOException(sError == "" ? "the conversion failed" : sError);
            if (!File.Exists(sTemp) || new FileInfo(sTemp).Length == 0) throw new IOException("the conversion made an empty file");
            if (File.Exists(sResult)) File.Replace(sTemp, sResult, null);
            else File.Move(sTemp, sResult);
            Log.info("Converted " + sSource + " to " + sResult);
            return "Converted " + Path.GetFileName(sSource) + " to " + Path.GetFileName(sResult) + ".";
        }
        catch (Exception oError)
        {
            Log.error("Failed " + sSource + ": " + oError.Message);
            return "Failed " + Path.GetFileName(sSource) + ": " + oError.Message;
        }
        finally
        {
            try { if (File.Exists(sTemp)) File.Delete(sTemp); } catch (Exception oError) { Log.warn("Temporary file left: " + oError.Message); }
        }
    }
}
}
