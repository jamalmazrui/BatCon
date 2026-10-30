// BatCon.cs -- batch conversion between any two formats the Homer kit converts.
//
// A HOMER LAUNCHPAD APP, made with newHomerApp from HomerDev's templates and built
// against the installed kit, never a copy of it. Every conversion is the kit's
// Conversion.convert, the one technique every Homer app uses.
//
// THE DIALOG IS THE KIT'S LAUNCH DIALOG (23 October 2026, help\LaunchDialog.md in
// HomerDev), the one every Homer launch app shows, in the order Tab moves through it:
//   Source paths            text box  &Source paths            paths, or /source <path>
//   Browse local source     button    &Browse local source     picks a folder of files
//   Target format           list      &Target format           /to <extension>, -t
//   Output directory        text box  &Output directory        /output-directory, -o
//   Choose output directory button    &Choose output directory
//   Force replacements      check     &Force replacements      /force-replacements, -f
//   Use configuration       check     &Use configuration       /use-configuration, -u
//   View output directory   check     &View output directory   /view-output-directory, -v
//   Default settings, Help  buttons                            /default-settings, -d
//   OK and Cancel, alone at the bottom right.
// Target format is BatCon's own control, in the band after the source. Logging is
// automatic: every run writes %LOCALAPPDATA%\BatCon\logs\BatCon-yyyyMMdd-HHmmss.log,
// and the Help box says so. Every run ends with the Results box, then BatCon closes.
//
// THE DEFAULTS ARE DELIBERATE, as in 2htm: Source files starts at your Documents
// folder; Output directory is derived from the source -- the folder itself, or the
// folder of a single file or pattern -- and follows the source while it still mirrors
// it, until you choose another. Blank means beside each source.
//
// THE TARGET LIST IS THE TWELVE FORMATS THE AUTHOR CHOSE (23 October 2026), csv, db,
// docx, epub, htm, inix, md, pdf, pptx, tsv, txt and xlsx, each named as the kit names
// it and sorted by that name, so it never changes under the cursor. A source whose type
// does not convert to the target is skipped, said once per type with the reason, and
// counted in the Results box -- which is what happens for db until the kit has a route
// that makes one.
//
// USE CONFIGURATION, as in 2htm and extCheck: when it is checked at OK, every field
// is saved in %LOCALAPPDATA%\BatCon\configs\BatCon.inix and loaded the next time;
// when it is not, the file is removed and BatCon starts from its defaults. Default
// settings restores the defaults and removes the file. This replaces the template's
// "saved the moment it is answered", which the launchpad pattern does not follow.
//
// FILE SAFETY: each result is written to a temporary file and moved into place only
// when its conversion succeeds; an existing result is kept unless Force replacements
// is checked; a source is never overwritten; two sources with one name get name_2.
//
// Copyright 2026 by Jamal Mazrui. MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Homer;

namespace BatCon
{
public static class Program
{
    private const string sAppName = "BatCon";
    private const string sGitHubOwner = "JamalMazrui";
    private const string sSettingsFile = "BatCon.inix";
    private const string sSection = "Settings";

    private static bool bForce = false;
    private static bool bUseConfig = false;
    private static bool bViewOutput = false;
    private static string sOutput = "";
    private static string sSource = "";
    private static string sTarget = "md";

    [DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int iProcessId);

    [STAThread]
    public static int Main(string[] aArguments)
    {
        Log.start(sAppName);
        Paths.start(sAppName);
        Elevate.configure(sGitHubOwner, sAppName, BuildVersion.Version);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        if (aArguments != null && aArguments.Length > 0)
        {
            // Print to the console it was started from, unless the output is already
            // going somewhere, as when a script or the kit's check reads it.
            if (!Console.IsOutputRedirected) AttachConsole(-1);
            return runFromCommandLine(aArguments);
        }
        loadConfiguration();
        return runFromDialog();
    }

    // ---- settings: Use configuration --------------------------------------

    private static string settingsPath() { return Paths.configFile(sSettingsFile); }

    private static string defaultSource()
    {
        try { return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); }
        catch (Exception) { return ""; }
    }

    private static bool restoreDefaults()
    {
        sSource = defaultSource();
        sOutput = Batch.deriveOutput(sSource);
        sTarget = "md";
        bForce = false; bViewOutput = false; bUseConfig = false;
        return true;
    }

    // loadConfiguration: the saved fields when Use configuration was checked last time,
    // otherwise the defaults.
    private static bool loadConfiguration()
    {
        restoreDefaults();
        string sPath = settingsPath();
        if (!File.Exists(sPath) || InixCodec.readValue(sPath, sSection, "UseConfig", "0") != "1") return true;
        sSource = InixCodec.readValue(sPath, sSection, "Source", sSource);
        sOutput = InixCodec.readValue(sPath, sSection, "Output", sOutput);
        sTarget = InixCodec.readValue(sPath, sSection, "Target", sTarget);
        bForce = InixCodec.readValue(sPath, sSection, "Force", "0") == "1";
        bViewOutput = InixCodec.readValue(sPath, sSection, "ViewOutput", "0") == "1";
        bUseConfig = true;
        Log.info("Configuration loaded from " + sPath);
        return true;
    }

    // saveConfiguration: at OK, every field when Use configuration is checked; otherwise
    // the file is removed, so the next start is from the defaults.
    private static bool saveConfiguration()
    {
        string sPath = settingsPath();
        try
        {
            if (!bUseConfig) { if (File.Exists(sPath)) { File.Delete(sPath); Log.info("Configuration removed: " + sPath); } return true; }
            InixCodec.writeValue(sPath, sSection, "UseConfig", "1");
            InixCodec.writeValue(sPath, sSection, "Source", sSource);
            InixCodec.writeValue(sPath, sSection, "Output", sOutput);
            InixCodec.writeValue(sPath, sSection, "Target", sTarget);
            InixCodec.writeValue(sPath, sSection, "Force", bForce ? "1" : "0");
            InixCodec.writeValue(sPath, sSection, "ViewOutput", bViewOutput ? "1" : "0");
            Log.info("Configuration saved in " + sPath);
        }
        catch (Exception oError) { Log.error("Configuration: " + oError.Message); }
        return true;
    }

    // ---- the dialog ---------------------------------------------------------

    // THE TARGET FORMATS OFFERED, by extension (23 October 2026). Their names come from
    // the kit, so a format reads as every Homer app names it.
    private static readonly string[] aOfferedTargets = { "csv", "db", "docx", "epub", "htm", "inix", "md", "pdf", "pptx", "tsv", "txt", "xlsx" };

    // offeredTargets: the extensions in the order shown, with their labels, sorted by label.
    private static string[] offeredTargets(out string[] aLabels)
    {
        string[] aKitLabels;
        string[] aKitTargets = Conversion.allTargets(out aKitLabels);
        List<string[]> lsPairs = new List<string[]>();
        foreach (string sType in aOfferedTargets)
        {
            int iKit = Array.IndexOf(aKitTargets, sType);
            string sLabel = iKit >= 0 ? aKitLabels[iKit] : "";
            if (sLabel == "")
            {
                // The kit's wording, "txt (plain text)", extension first, so the list sorted by label runs in
                // extension order and t reaches txt (HomerDev 1.93.7).
                sLabel = Conversion.targetLabel(sType, Conversion.typeName(sType));
            }
            lsPairs.Add(new string[] { sLabel, sType });
        }
        lsPairs.Sort(delegate(string[] a, string[] b) { return string.Compare(a[0], b[0], StringComparison.OrdinalIgnoreCase); });
        aLabels = new string[lsPairs.Count];
        string[] aTypes = new string[lsPairs.Count];
        for (int i = 0; i < lsPairs.Count; i++) { aLabels[i] = lsPairs[i][0]; aTypes[i] = lsPairs[i][1]; }
        return aTypes;
    }

    private static int runFromDialog()
    {
        string[] aLabels;
        string[] aTargets = offeredTargets(out aLabels);
        while (true)
        {
            string sButton;
            using (LaunchDialog oLaunch = new LaunchDialog(sAppName))
            {
                oLaunch.sourcePick = "folder";
                TextBox tbSource = oLaunch.addSourceBand(sSource,
                    "One or more files, folders or wildcard patterns such as *.pdf, separated by spaces, each in quotes if it has a space. A folder means every file in it.",
                    "Choose a folder of files to convert. It opens where Source paths points, else in Documents.");

                // BatCon's own choice: what to convert to.
                oLaunch.dialog.addBand();
                int iTarget = Array.IndexOf(aTargets, sTarget);
                ListBox lbTarget = oLaunch.dialog.addPickBox("&Target format:", aLabels, iTarget >= 0 ? aLabels[iTarget] : aLabels[0],
                    "The format to convert to. A source that does not convert to it is skipped, and the Results box says why.");
                oLaunch.dialog.endBand();

                TextBox tbOutput = oLaunch.addOutputBand(sOutput,
                    "Where each converted file is written. Blank means beside its source.",
                    "Choose the folder the converted files go to.");

                // THE OUTPUT FOLLOWS THE SOURCE while it still mirrors it, whether the source is
                // browsed or typed; a folder the user chose, or typed, stays put.
                string sMirror = Batch.deriveOutput(tbSource.Text);
                Action followSource = delegate()
                {
                    bool bMirrors = tbOutput.Text.Trim() == "" || string.Equals(tbOutput.Text.Trim(), sMirror, StringComparison.OrdinalIgnoreCase);
                    sMirror = Batch.deriveOutput(tbSource.Text);
                    if (bMirrors) tbOutput.Text = sMirror;
                };
                oLaunch.sourceChosen = followSource;
                tbSource.Leave += delegate(object oSender, EventArgs evArgs) { followSource(); };

                oLaunch.addOperationBands(bForce, "",
                    bUseConfig, "Load these settings next time, and save them when you press OK, in %LOCALAPPDATA%\\BatCon\\configs\\" + sSettingsFile + ".",
                    bViewOutput, "Open the output directory in File Explorer when the run is done.");

                sButton = oLaunch.run();

                // Harvest every field before the dialog is disposed, so a return keeps what was typed.
                sSource = (tbSource.Text ?? "").Trim();
                sOutput = (tbOutput.Text ?? "").Trim();
                int iPicked = lbTarget.SelectedIndex;
                if (iPicked >= 0 && iPicked < aTargets.Length) sTarget = aTargets[iPicked];
                bForce = oLaunch.forceBox.Checked;
                bViewOutput = oLaunch.viewOutputBox.Checked;
                bUseConfig = oLaunch.useConfigBox.Checked;
            }
            if (sButton == "Cancel") return 0;
            if (sButton == "Default settings")
            {
                restoreDefaults();
                try { if (File.Exists(settingsPath())) File.Delete(settingsPath()); } catch (Exception) { }
                Log.info("Default settings restored");
                continue;
            }
            if (!confirmOutput(sOutput)) continue;
            saveConfiguration();
            break;
        }
        List<string> lsProblems = new List<string>();
        List<string> lsFiles = Batch.discover(Batch.tokens(sSource), lsProblems);
        List<string> lsReport = new List<string>();
        string sOpen;
        int iResult = runJob(lsFiles, lsProblems, lsReport, true, out sOpen);
        LaunchDialog.finish(sAppName, string.Join("\r\n", lsReport.ToArray()), iResult != 0, bViewOutput ? sOpen : "");
        return iResult;
    }

    // confirmOutput: offer to make an output directory that does not exist yet, Yes by default.
    private static bool confirmOutput(string sFolder)
    {
        string sCandidate = (sFolder ?? "").Trim().Trim('"');
        if (sCandidate == "" || Directory.Exists(sCandidate)) return true;
        if (MessageBox.Show("Create " + sCandidate + "?", sAppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button1) != DialogResult.Yes) return false;
        try { Directory.CreateDirectory(sCandidate); return true; }
        catch (Exception oError)
        {
            MessageBox.Show("Could not create the folder:\r\n" + sCandidate + "\r\n\r\n" + oError.Message, sAppName,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
    }

    // ---- the command line ---------------------------------------------------

    private static int runFromCommandLine(string[] aArguments)
    {
        List<string> lsSources = new List<string>();
        bool bList = false;
        // USE CONFIGURATION FIRST, wherever it is written, so the switches after it can
        // still change any setting it loads.
        foreach (string sArgument in aArguments)
        {
            string sSwitch = Util.switchName(sArgument);
            if (sSwitch == "use-configuration" || sSwitch == "u") { loadConfiguration(); break; }
        }
        for (int i = 0; i < aArguments.Length; i++)
        {
            string sArgument = aArguments[i];
            string sSwitch = Util.switchName(sArgument);
            string sNext = (i + 1 < aArguments.Length) ? aArguments[i + 1] : "";
            if (sSwitch == "help" || sSwitch == "h" || sArgument == "/?") { Console.WriteLine(helpText()); return 0; }
            else if (sSwitch == "source" || sSwitch == "source-paths" || sSwitch == "s") { lsSources.Add(sNext); i++; }
            else if (sSwitch == "to" || sSwitch == "target-format" || sSwitch == "t") { sTarget = Conversion.typeOf(sNext); i++; }
            else if (sSwitch == "output-directory" || sSwitch == "output" || sSwitch == "o") { sOutput = sNext; i++; }
            else if (sSwitch == "force-replacements" || sSwitch == "force" || sSwitch == "f") bForce = true;
            else if (sSwitch == "view-output-directory" || sSwitch == "view-output" || sSwitch == "view" || sSwitch == "v") bViewOutput = true;
            else if (sSwitch == "use-configuration" || sSwitch == "u") { }
            else if (sSwitch == "list") bList = true;
            else if (sSwitch == "default-settings" || sSwitch == "defaults" || sSwitch == "d")
            {
                try { if (File.Exists(settingsPath())) File.Delete(settingsPath()); } catch (Exception) { }
                Console.WriteLine("Default settings restored.");
                return 0;
            }
            else if (sSwitch != "")
            {
                Console.WriteLine("Unknown switch " + sArgument + ". Run with /help to see the switches.");
                Log.error("Unknown switch: " + sArgument);
                return 1;
            }
            else lsSources.Add(sArgument);
        }
        if (bList)
        {
            string[] aLabels;
            offeredTargets(out aLabels);
            foreach (string sLabel in aLabels) Console.WriteLine(sLabel);
            return 0;
        }
        List<string> lsProblems = new List<string>();
        List<string> lsFiles = Batch.discover(lsSources, lsProblems);
        List<string> lsReport = new List<string>();
        string sOpen;
        int iResult = runJob(lsFiles, lsProblems, lsReport, false, out sOpen);
        foreach (string sLine in lsReport) Console.WriteLine(sLine);
        if (bViewOutput && sOpen != "")
        {
            try { if (Directory.Exists(sOpen)) Process.Start("explorer.exe", "\"" + sOpen + "\""); } catch (Exception oError) { Log.error("View output directory: " + oError.Message); }
        }
        return iResult;
    }

    // ---- the batch ----------------------------------------------------------

    // runJob: skip, by type, every source that does not convert to the target, saying why;
    // convert the rest; report each outcome with its count.
    // sOpen gives back the folder View output directory would open.
    //
    // bShow: from the dialog, the work runs under the kit's LaunchProgress window
    // (23 October 2026): "BatCon, <status>" in its title, a status box, Cancel
    // (Escape), and speech only while BatCon is the window in front, so a batch
    // never talks over work in another program. From the command line nothing is
    // shown or spoken, and the report goes to the console.
    private static int runJob(List<string> lsFiles, List<string> lsProblems, List<string> lsReport, bool bShow, out string sOpen)
    {
        sOpen = "";
        LaunchProgress oProgress = bShow ? new LaunchProgress(sAppName, "Finding the files", delegate(string sLine) { Log.info(sLine); }) : null;
        try { return runJobShown(lsFiles, lsProblems, lsReport, oProgress, out sOpen); }
        finally { if (oProgress != null) oProgress.Dispose(); }
    }

    private static int runJobShown(List<string> lsFiles, List<string> lsProblems, List<string> lsReport, LaunchProgress oProgress, out string sOpen)
    {
        sOpen = "";
        foreach (string sProblem in lsProblems) Log.error(sProblem);
        if (lsFiles.Count == 0 || sTarget == "")
        {
            lsReport.Add("Nothing to do: no source file was found" + (sTarget == "" ? ", and no target format was chosen." : "."));
            lsReport.AddRange(lsProblems);
            return 1;
        }
        // SKIPPING, BY TYPE: one message per source type that has no conversion to the target.
        Dictionary<string, List<string>> dSkipped = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        List<string> lsToConvert = new List<string>();
        foreach (string sFile in lsFiles)
        {
            string sType = Conversion.typeOf(sFile);
            if (sType != "" && Conversion.canConvert(sFile, "x." + sTarget)) { lsToConvert.Add(sFile); continue; }
            string sKey = sType == "" ? "(no extension)" : sType;
            if (!dSkipped.ContainsKey(sKey)) dSkipped[sKey] = new List<string>();
            dSkipped[sKey].Add(sFile);
        }
        List<string> lsSkipLines = new List<string>();
        int iSkippedNoRoute = 0;
        List<string> lsTypes = new List<string>(dSkipped.Keys);
        lsTypes.Sort(StringComparer.OrdinalIgnoreCase);
        foreach (string sType in lsTypes)
        {
            iSkippedNoRoute += dSkipped[sType].Count;
            string sLine = "Skipping " + Batch.count(dSkipped[sType].Count, "file") + " of type " + sType
                + ": the Homer kit has no conversion from " + sType + " to " + sTarget + ".";
            lsSkipLines.Add(sLine);
            Log.info(sLine);
            if (oProgress != null) oProgress.status(sLine);
        }

        string sOutputFull = sOutput == "" ? "" : Path.GetFullPath(Environment.ExpandEnvironmentVariables(sOutput.Trim('"')));
        Log.info("Batch: " + lsToConvert.Count + " files to " + sTarget + (sOutputFull == "" ? ", each beside its source" : " in " + sOutputFull)
            + (bForce ? ", replacing results" : ""));
        if (oProgress != null && lsToConvert.Count > 0) oProgress.status("Converting " + Batch.count(lsToConvert.Count, "file") + " to " + sTarget);
        HashSet<string> hsReserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> hsSources = new HashSet<string>(lsFiles, StringComparer.OrdinalIgnoreCase);
        int iConverted = 0, iFailed = 0, iKept = 0, iAt = 0;
        // INCOMPLETE IS NOT SUCCESS (30 October 2026, from an audit by another AI): the exit code
        // looked only at failed conversions, so a path that could not be found, or a run stopped
        // with Cancel, ended with 0. Problems found before converting, and Cancel, now end with 3,
        // as a failure does; a type skipped for want of a conversion is still not a failure.
        int iProblemsBefore = lsProblems.Count;
        bool bCancelled = false;
        List<string> lsLines = new List<string>();
        foreach (string sFile in lsToConvert)
        {
            // Cancel in the progress window stops before the next file.
            if (oProgress != null && oProgress.cancelled)
            {
                lsProblems.Add("Cancelled: " + Batch.count(lsToConvert.Count - iAt, "file") + " not converted.");
                bCancelled = true;
                break;
            }
            iAt++;
            if (oProgress != null) oProgress.step(Path.GetFileName(sFile), iAt - 1, lsToConvert.Count);
            string sFolder = sOutputFull == "" ? Path.GetDirectoryName(sFile) : sOutputFull;
            string sResultPath = Batch.destination(sFile, sFolder, sTarget, hsReserved, hsSources);
            string sOutcome = Batch.convertOne(sFile, sResultPath, bForce);
            if (sOutcome.StartsWith("Converted")) iConverted++;
            else if (sOutcome.StartsWith("Skipped")) iKept++;
            else iFailed++;
            lsLines.Add(sOutcome);
        }
        lsReport.Add(Batch.count(iConverted, "file") + " converted, " + iKept + " kept as they were, "
            + iSkippedNoRoute + " skipped for want of a conversion, " + iFailed + " failed.");
        if (iKept > 0) lsReport.Add(LaunchDialog.keptLine(iKept, "file"));
        lsReport.AddRange(lsSkipLines);
        lsReport.AddRange(lsProblems);
        lsReport.AddRange(lsLines);
        lsReport.Add("The log: " + Log.path);
        Log.info(lsReport[0]);
        sOpen = sOutputFull != "" ? sOutputFull : (lsFiles.Count > 0 ? Path.GetDirectoryName(lsFiles[0]) : "");
        return (iFailed > 0 || bCancelled || iProblemsBefore > 0) ? 3 : 0;
    }

    private static string helpText()
    {
        StringBuilder sbText = new StringBuilder();
        sbText.AppendLine("BatCon " + BuildVersion.Version + " -- convert many files at once between formats.");
        sbText.AppendLine("");
        sbText.AppendLine("  batcon                          the dialog");
        sbText.AppendLine("  batcon <paths> -t <format> [-o <folder>] [-f] [-u] [-v]");
        sbText.AppendLine("  batcon /list                    the formats BatCon converts to");
        sbText.AppendLine("  batcon -d                       settings back to their defaults");
        sbText.AppendLine("  batcon -h");
        sbText.AppendLine("");
        sbText.AppendLine("  <paths>                         files, folders or patterns such as *.pdf");
        sbText.AppendLine("  -s, --source-paths <path>       one more source, as often as wanted (/source too)");
        sbText.AppendLine("  -t, --target-format <format>    the format to convert to, such as md or docx");
        sbText.AppendLine("  -o, --output-directory <folder> where results go; without it, beside each source");
        sbText.AppendLine("  -f, --force-replacements        replace results that are already there");
        sbText.AppendLine("  -u, --use-configuration         start from the settings the dialog saved");
        sbText.AppendLine("  -v, --view-output-directory     open the output directory when done");
        sbText.AppendLine("  -d, --default-settings          forget the saved settings");
        sbText.AppendLine("  -h, --help                      this list");
        sbText.AppendLine("");
        sbText.AppendLine("A source that does not convert to the format is skipped and counted. Switches may be");
        sbText.AppendLine("written /switch, -switch or --switch, in any case. Exit code: 0 done, 1 nothing to do or");
        sbText.AppendLine("an unknown switch, 3 some files failed. Logging is automatic: each run writes");
        sbText.AppendLine("%LOCALAPPDATA%\\BatCon\\logs\\BatCon-yyyyMMdd-HHmmss.log.");
        return sbText.ToString();
    }
}
}
