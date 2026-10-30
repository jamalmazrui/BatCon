// Batch.cs -- BatCon's batch work: finding the source files, the formats they all
// convert to, a safe name for each result, and one conversion at a time.
//
// Everything about formats comes from the kit's Conversion class: every target it can make
// (allTargets), whether a file converts to the chosen one (hasRoute), and the conversion
// itself (convert, the one technique every Homer app uses). Nothing here reads
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
    // A folder gives the files in it, not in the folders within it: Include subfolders was
    // never asked for, and went on 23 October 2026.
    public static List<string> discover(IEnumerable<string> lsPaths, List<string> lsProblems)
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
                    scan(sFolder, Path.GetFileName(sExpanded), hsFiles, lsProblems);
                }
                else if (File.Exists(sExpanded)) hsFiles.Add(Path.GetFullPath(sExpanded));
                else if (Directory.Exists(sExpanded)) scan(Path.GetFullPath(sExpanded), "*", hsFiles, lsProblems);
                else lsProblems.Add("Not found: " + sExpanded);
            }
            catch (Exception oError) { lsProblems.Add(sPath + ": " + oError.Message); }
        }
        List<string> lsFiles = new List<string>(hsFiles);
        lsFiles.Sort(StringComparer.OrdinalIgnoreCase);
        return lsFiles;
    }

    // scan: the files in one folder. Every visible file is taken, whether or not it converts to the
    // target, so a file of another type is reported as skipped rather than passed over in
    // silence; hidden and system files, such as desktop.ini, are not the user's documents.
    private static bool scan(string sFolder, string sPattern, HashSet<string> hsFiles, List<string> lsProblems)
    {
        try
        {
            foreach (string sFile in Directory.GetFiles(sFolder, sPattern))
            {
                FileAttributes iAttr = File.GetAttributes(sFile);
                if ((iAttr & (FileAttributes.Hidden | FileAttributes.System)) != 0) continue;
                hsFiles.Add(Path.GetFullPath(sFile));
            }
        }
        catch (Exception oError) { lsProblems.Add(sFolder + ": " + oError.Message); }
        return true;
    }

    // deriveOutput: the Output directory a source implies, 2htm's rule: one folder gives
    // itself, one pattern or one file gives its folder, anything else gives nothing,
    // which means each result goes beside its source.
    public static string deriveOutput(string sSourceField)
    {
        List<string> lsPaths = tokens(sSourceField);
        if (lsPaths.Count != 1) return "";
        string sOne = Environment.ExpandEnvironmentVariables(lsPaths[0]);
        try
        {
            if (Directory.Exists(sOne)) return Path.GetFullPath(sOne);
            if (sOne.IndexOfAny(new char[] { '*', '?' }) >= 0)
            {
                string sDir = Path.GetDirectoryName(sOne);
                return !string.IsNullOrEmpty(sDir) && Directory.Exists(sDir) ? Path.GetFullPath(sDir) : "";
            }
            if (File.Exists(sOne)) return Path.GetDirectoryName(Path.GetFullPath(sOne)) ?? "";
        }
        catch (Exception) { }
        return "";
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
            // THE TITLE COMES FROM THE RESULT'S NAME (30 October 2026, from an audit by another
            // AI): a document made under this temporary name took ".batcon-" and a number as
            // its title. The kit is told the name it is made for (HomerDev 1.94.1).
            Conversion.intendFor(sTemp, sResult);
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
            Conversion.forgetIntended(sTemp);
            try { if (File.Exists(sTemp)) File.Delete(sTemp); } catch (Exception oError) { Log.warn("Temporary file left: " + oError.Message); }
        }
    }
}
}
