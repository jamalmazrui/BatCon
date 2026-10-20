// BatCon.cs -- batch conversion between any two formats the Homer kit converts.
//
// A HOMER LAUNCHPAD APP, made with newHomerApp from HomerDev's templates and built
// against the installed kit, never a copy of it. Every conversion is the kit's
// Conversion.convert, the one technique every Homer app uses.
//
// THE DIALOG FOLLOWS 2htm AND extCheck, the pattern of every Homer launchpad dialog
// (20 October 2026, the author's audit), in the order Tab moves through it:
//   Source files        text box   &Source files       a path, or /source <path>
//   Browse source       button     &Browse source...   picks a folder of files
//   Output directory    text box   &Output directory   /output <folder>
//   Choose output       button     &Choose output...
//   -- separator --
//   Target format       list       &Target format      /to <extension>
//   Include subfolders  check      &Include subfolders /recursive
//   Force replacements  check      &Force replacements /force
//   View output         check      &View output        /view
//   Log session         check      &Log session        /log
//   Use configuration   check      &Use configuration
//   OK, Guide, Default settings, Cancel; Lbc adds Help (F1), whose box lists each
//   field's tip and ends with the version check. F11 checks for a newer BatCon.
// Target format and Include subfolders are BatCon's own options, after the
// separator where 2htm puts its own.
//
// THE DEFAULTS ARE DELIBERATE, as in 2htm: Source files starts at your Documents
// folder; Output directory is derived from the source -- the folder itself, or the
// folder of a single file or pattern -- and follows the source while it still mirrors
// it, until you choose another. Blank means beside each source.
//
// THE TARGET LIST IS THE KIT'S WHOLE LIST, alphabetical by name, from the outset
// (Conversion.allTargets), so it never changes under the cursor. A source whose type
// does not convert to the target is skipped, said once per type with the reason, and
// counted in the Results box.
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
    private const string sSessionLogName = "BatCon.log";
    // Spoken progress at most this often, in milliseconds, so a fast batch is not a torrent of speech.
    private const int iSpeakGap = 2000;

    private static bool bForce = false;
    private static bool bLogSession = false;
    private static bool bRecursive = false;
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
        bRecursive = false; bForce = false; bViewOutput = false; bLogSession = false; bUseConfig = false;
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
        bRecursive = InixCodec.readValue(sPath, sSection, "Recursive", "0") == "1";
        bForce = InixCodec.readValue(sPath, sSection, "Force", "0") == "1";
        bViewOutput = InixCodec.readValue(sPath, sSection, "ViewOutput", "0") == "1";
        bLogSession = InixCodec.readValue(sPath, sSection, "LogSession", "0") == "1";
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
            InixCodec.writeValue(sPath, sSection, "Recursive", bRecursive ? "1" : "0");
            InixCodec.writeValue(sPath, sSection, "Force", bForce ? "1" : "0");
            InixCodec.writeValue(sPath, sSection, "ViewOutput", bViewOutput ? "1" : "0");
            InixCodec.writeValue(sPath, sSection, "LogSession", bLogSession ? "1" : "0");
            Log.info("Configuration saved in " + sPath);
        }
        catch (Exception oError) { Log.error("Configuration: " + oError.Message); }
        return true;
    }

    // ---- the dialog ---------------------------------------------------------

    private static int runFromDialog()
    {
        string[] aLabels;
        string[] aTargets = Conversion.allTargets(out aLabels);
        while (true)
        {
            string sButton;
            using (LbcDialog dlg = new LbcDialog(sAppName, null))
            {
                dlg.addBand();
                TextBox tbSource = dlg.addInputBox("&Source files:", sSource,
                    "One or more files, folders or wildcard patterns such as *.pdf, separated by spaces, each in quotes if it has a space. A folder means every file in it.");
                Button btnBrowse = dlg.addButton("&Browse source...", "Choose a folder of files to convert.");
                dlg.addBand();
                TextBox tbOutput = dlg.addInputBox("&Output directory:", sOutput,
                    "Where each converted file is written. Blank means beside its source.");
                Button btnChoose = dlg.addButton("&Choose output...", "Choose the folder the converted files go to.");
                dlg.endBand();

                // THE OUTPUT FOLLOWS THE SOURCE while it still mirrors it, whether the source is
                // browsed or typed; a folder the user chose, or typed, stays put.
                string sMirror = Batch.deriveOutput(tbSource.Text);
                Action followSource = delegate()
                {
                    bool bMirrors = tbOutput.Text.Trim() == "" || string.Equals(tbOutput.Text.Trim(), sMirror, StringComparison.OrdinalIgnoreCase);
                    sMirror = Batch.deriveOutput(tbSource.Text);
                    if (bMirrors) tbOutput.Text = sMirror;
                };
                btnBrowse.Click += delegate(object oSender, EventArgs evArgs)
                {
                    string sPicked = pickFolder(dlg.form, "Choose a folder of files to convert", tbSource.Text);
                    if (sPicked == null) return;
                    tbSource.Text = sPicked.Contains(" ") ? "\"" + sPicked + "\"" : sPicked;
                    followSource();
                    tbSource.Focus();
                };
                tbSource.Leave += delegate(object oSender, EventArgs evArgs) { followSource(); };
                btnChoose.Click += delegate(object oSender, EventArgs evArgs)
                {
                    string sPicked = pickFolder(dlg.form, "Choose the output directory", tbOutput.Text);
                    if (sPicked == null) return;
                    tbOutput.Text = sPicked;
                    tbOutput.Focus();
                };

                dlg.addSeparator();
                int iTarget = Array.IndexOf(aTargets, sTarget);
                ListBox lbTarget = dlg.addPickBox("&Target format:", aLabels, iTarget >= 0 ? aLabels[iTarget] : (aLabels.Length > 0 ? aLabels[0] : ""),
                    "The format to convert to: every format the Homer kit makes, by name. A source that does not convert to it is skipped and counted.");
                CheckBox cbRecursive = dlg.addCheckBox("&Include subfolders", bRecursive, "Look inside the folders within each source folder too.");
                CheckBox cbForce = dlg.addCheckBox("&Force replacements", bForce, "Replace a result already there, once its new conversion succeeds, instead of keeping it.");
                CheckBox cbView = dlg.addCheckBox("&View output", bViewOutput, "Open the output folder in File Explorer when the run is done.");
                CheckBox cbLog = dlg.addCheckBox("&Log session", bLogSession,
                    "Also write " + sSessionLogName + " in the output folder. A session log is always kept in %LOCALAPPDATA%\\BatCon\\logs.");
                CheckBox cbUseConfig = dlg.addCheckBox("&Use configuration", bUseConfig,
                    "Load these settings next time, and save them when you press OK, in %LOCALAPPDATA%\\BatCon\\configs\\" + sSettingsFile + ".");

                // F11: is there a newer BatCon on the web? (Elevate sounds like eleven.)
                dlg.commandKey = delegate(Keys k)
                {
                    if (k != Keys.F11) return false;
                    Log.info("F11: checking the web for a newer version");
                    if (Elevate.offer(dlg.form))
                    {
                        Log.info("F11: the setup program was started; closing the dialog");
                        dlg.form.DialogResult = DialogResult.Cancel;
                        dlg.form.Close();
                    }
                    return true;
                };

                sButton = dlg.runWithButtons(new string[] { "OK", "Guide", "Default settings", "Cancel" });

                // Harvest every field before the dialog is disposed, so a return keeps what was typed.
                sSource = (tbSource.Text ?? "").Trim();
                sOutput = (tbOutput.Text ?? "").Trim();
                int iPicked = lbTarget.SelectedIndex;
                if (iPicked >= 0 && iPicked < aTargets.Length) sTarget = aTargets[iPicked];
                bRecursive = cbRecursive.Checked;
                bForce = cbForce.Checked;
                bViewOutput = cbView.Checked;
                bLogSession = cbLog.Checked;
                bUseConfig = cbUseConfig.Checked;
            }
            if (string.IsNullOrEmpty(sButton) || string.Equals(sButton, "Cancel", StringComparison.OrdinalIgnoreCase)) return 0;
            if (string.Equals(sButton, "Guide", StringComparison.OrdinalIgnoreCase)) { openGuide(); continue; }
            if (string.Equals(sButton, "Default settings", StringComparison.OrdinalIgnoreCase))
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
        List<string> lsFiles = Batch.discover(Batch.tokens(sSource), bRecursive, lsProblems);
        List<string> lsReport = new List<string>();
        int iResult = runJob(lsFiles, lsProblems, lsReport, true);
        HelpDialog.show(null, "BatCon results", string.Join("\r\n", lsReport.ToArray()));
        return iResult;
    }

    // pickFolder: a Browse For Folder dialog starting where the field points, else Documents.
    private static string pickFolder(IWin32Window owner, string sTitle, string sFieldText)
    {
        using (FolderBrowserDialog dlgFolder = new FolderBrowserDialog())
        {
            dlgFolder.Description = sTitle;
            dlgFolder.ShowNewFolderButton = true;
            string sStart = Batch.deriveOutput(sFieldText);
            dlgFolder.SelectedPath = sStart != "" ? sStart : defaultSource();
            return dlgFolder.ShowDialog(owner) == DialogResult.OK ? dlgFolder.SelectedPath : null;
        }
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

    // openGuide: help\BatCon.htm beside exec, in the installed tree and the project alike.
    private static bool openGuide()
    {
        foreach (string sName in new string[] { "help\\BatCon.htm", "ReadMe.htm", "help\\BatCon.md" })
        {
            string sPath = Path.Combine(Paths.installedFolder, sName);
            if (!File.Exists(sPath)) continue;
            try { Process.Start(sPath); return true; } catch (Exception oError) { Log.error("Guide: " + oError.Message); }
        }
        Say.say("The guide was not found");
        return false;
    }

    // ---- the command line ---------------------------------------------------

    private static int runFromCommandLine(string[] aArguments)
    {
        List<string> lsSources = new List<string>();
        bool bList = false;
        for (int i = 0; i < aArguments.Length; i++)
        {
            string sArgument = aArguments[i];
            string sSwitch = Util.switchName(sArgument);
            string sNext = (i + 1 < aArguments.Length) ? aArguments[i + 1] : "";
            if (sSwitch == "help" || sSwitch == "h" || sArgument == "/?") { Console.WriteLine(helpText()); return 0; }
            else if (sSwitch == "source") { lsSources.Add(sNext); i++; }
            else if (sSwitch == "to") { sTarget = Conversion.typeOf(sNext); i++; }
            else if (sSwitch == "output") { sOutput = sNext; i++; }
            else if (sSwitch == "recursive") bRecursive = true;
            else if (sSwitch == "force") bForce = true;
            else if (sSwitch == "view") bViewOutput = true;
            else if (sSwitch == "log") bLogSession = true;
            else if (sSwitch == "list") bList = true;
            else if (sSwitch == "defaults")
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
            Conversion.allTargets(out aLabels);
            foreach (string sLabel in aLabels) Console.WriteLine(sLabel);
            return 0;
        }
        List<string> lsProblems = new List<string>();
        List<string> lsFiles = Batch.discover(lsSources, bRecursive, lsProblems);
        List<string> lsReport = new List<string>();
        int iResult = runJob(lsFiles, lsProblems, lsReport, false);
        foreach (string sLine in lsReport) Console.WriteLine(sLine);
        return iResult;
    }

    // ---- the batch ----------------------------------------------------------

    // runJob: skip, by type, every source that does not convert to the target, saying why;
    // convert the rest; report each outcome with its count.
    private static int runJob(List<string> lsFiles, List<string> lsProblems, List<string> lsReport, bool bSpeak)
    {
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
            if (bSpeak) Say.say(sLine);
        }

        string sOutputFull = sOutput == "" ? "" : Path.GetFullPath(Environment.ExpandEnvironmentVariables(sOutput.Trim('"')));
        Log.info("Batch: " + lsToConvert.Count + " files to " + sTarget + (sOutputFull == "" ? ", each beside its source" : " in " + sOutputFull)
            + (bForce ? ", replacing results" : ""));
        if (bSpeak && lsToConvert.Count > 0) Say.say("Converting " + Batch.count(lsToConvert.Count, "file") + " to " + sTarget);
        HashSet<string> hsReserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> hsSources = new HashSet<string>(lsFiles, StringComparer.OrdinalIgnoreCase);
        int iConverted = 0, iFailed = 0, iKept = 0, iAt = 0;
        Stopwatch oSince = Stopwatch.StartNew();
        List<string> lsLines = new List<string>();
        foreach (string sFile in lsToConvert)
        {
            iAt++;
            if (bSpeak && oSince.ElapsedMilliseconds >= iSpeakGap)
            {
                Say.say(iAt + " of " + lsToConvert.Count + ": " + Path.GetFileName(sFile));
                oSince.Restart();
            }
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
        lsReport.AddRange(lsSkipLines);
        lsReport.AddRange(lsProblems);
        lsReport.AddRange(lsLines);
        lsReport.Add("The log: " + Log.path);
        Log.info(lsReport[0]);
        string sOpen = sOutputFull != "" ? sOutputFull : (lsFiles.Count > 0 ? Path.GetDirectoryName(lsFiles[0]) : "");
        if (bLogSession && sOpen != "")
        {
            try { File.Copy(Log.path, Path.Combine(sOpen, sSessionLogName), true); lsReport.Add("The session log is also in " + Path.Combine(sOpen, sSessionLogName)); }
            catch (Exception oError) { Log.error("Log session: " + oError.Message); }
        }
        if (bViewOutput && sOpen != "")
        {
            try { if (Directory.Exists(sOpen)) Process.Start(sOpen); } catch (Exception oError) { Log.error("View output: " + oError.Message); }
        }
        return iFailed > 0 ? 3 : 0;
    }

    private static string helpText()
    {
        StringBuilder sbText = new StringBuilder();
        sbText.AppendLine("BatCon " + BuildVersion.Version + " -- convert many files at once between formats.");
        sbText.AppendLine("");
        sbText.AppendLine("  BatCon                                   the dialog");
        sbText.AppendLine("  BatCon <paths> /to <format> [/output <folder>] [/recursive] [/force] [/view] [/log]");
        sbText.AppendLine("  BatCon /list                             every format BatCon can convert to");
        sbText.AppendLine("  BatCon /defaults                         settings back to their defaults");
        sbText.AppendLine("  BatCon /help");
        sbText.AppendLine("");
        sbText.AppendLine("A path is a file, a folder or a pattern such as *.pdf; /source <path> names one too.");
        sbText.AppendLine("A source that does not convert to the format is skipped and counted. Switches may be");
        sbText.AppendLine("written /switch, -switch or --switch. Exit code: 0 done, 1 nothing to do or an unknown");
        sbText.AppendLine("switch, 3 some files failed. Each run writes a log under %LOCALAPPDATA%\\BatCon\\logs.");
        return sbText.ToString();
    }
}
}
