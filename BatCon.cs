// BatCon.cs -- batch conversion between any two formats the Homer kit converts.
//
// A HOMER LAUNCHPAD APP: one dialog, one job, one result, made with newHomerApp from
// HomerDev's templates and built against the installed kit, never a copy of it.
// Every conversion is the kit's Conversion class, so BatCon converts exactly as
// FileDir, EdSharp and DbDo do, through the routes in exec\conversions.inix.
//
// WHAT IT DOES. You name source files -- files, folders or wildcard patterns -- and
// BatCon lists the formats every one of them converts to, with the kit's readable
// labels ("Markdown (md)"). You pick one and an output folder (or none, to write
// beside each source), and it converts them all, one at a time, saying how far it
// has got, then shows what was converted, skipped or failed.
//
// THE DIALOG (Lbc, the order of adding is the focus order):
//   Source files       text box   &Source files      /source <path>, or a bare path
//   Browse source      button     &Browse source...
//   Add folder         button     &Add folder...
//   Target format      pick box   &Target format     /to <extension>
//   Output directory   text box   &Output directory  /output <folder>
//   Choose output      button     &Choose output...
//   Include subfolders check      &Include subfolders /recursive
//   Force replacements check      &Force replacements /force
//   View output        check      &View output       /view
//   Default settings, OK, Cancel                      /defaults
// /list prints the formats the sources convert to; /help, /? the usage. Switches
// are the suite's one convention: /switch, -switch or --switch, in any case.
//
// FILE SAFETY (from ChatGPT's BatCon, reviewed): each result is written to a
// temporary file and moved into place only when the conversion succeeds; an
// existing result is kept unless Force replacements is checked; a source is never
// overwritten; two sources with one name get name_2, name_3 and so on.
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
    // The longest gap between spoken progress reports, in milliseconds: each file is
    // reported, but never faster than this, so a fast batch is not a torrent of speech.
    private const int iSpeakGap = 2000;

    private static bool bForce = false;
    private static bool bRecursive = false;
    private static bool bViewOutput = true;
    private static string sOutput = "";
    private static string sSource = "";
    private static string sTarget = "";

    [DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int iProcessId);

    [STAThread]
    public static int Main(string[] aArguments)
    {
        // The kit's services first: the session log, the app's folders, and Elevate,
        // which lets the Help box offer an update from GitHub.
        Log.start(sAppName);
        Paths.start(sAppName);
        Elevate.configure(sGitHubOwner, sAppName, BuildVersion.Version);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        loadSettings();
        if (aArguments != null && aArguments.Length > 0)
        {
            // A window program prints to the console it was started from, so the
            // desktop shortcut never flashes a console and a command line still reads.
            AttachConsole(-1);
            return runFromCommandLine(aArguments);
        }
        return runFromDialog();
    }

    private static string settingsPath()
    {
        return Paths.configFile(sSettingsFile);
    }

    private static bool loadSettings()
    {
        string sPath = settingsPath();
        sSource = InixCodec.readValue(sPath, sSection, "Source", "");
        sOutput = InixCodec.readValue(sPath, sSection, "Output", "");
        sTarget = InixCodec.readValue(sPath, sSection, "Target", "md");
        bRecursive = InixCodec.readValue(sPath, sSection, "Recursive", "0") == "1";
        bForce = InixCodec.readValue(sPath, sSection, "Force", "0") == "1";
        bViewOutput = InixCodec.readValue(sPath, sSection, "ViewOutput", "1") == "1";
        return true;
    }

    // Saved the moment it is answered, as every Homer setting is.
    private static bool saveSetting(string sKey, string sValue)
    {
        return InixCodec.writeValue(settingsPath(), sSection, sKey, sValue);
    }

    private static int runFromDialog()
    {
        List<string> lsFiles = new List<string>();
        List<string> lsTargets = new List<string>();
        using (LbcDialog dlg = new LbcDialog(sAppName, null))
        {
            dlg.addBand();
            TextBox tbSource = dlg.addInputBox("&Source files:", sSource,
                "Files, folders or patterns such as *.pdf, each in quotes if it has a space. Type them, or press Browse source.");
            Button btnBrowse = dlg.addButton("&Browse source...", "Pick one or more files with a standard Open dialog.");
            Button btnFolder = dlg.addButton("&Add folder...", "Add a folder, whose files are converted.");
            dlg.endBand();
            ListBox lbTarget = dlg.addPickBox("&Target format:", new List<string>(), "",
                "The formats every source file converts to. They change as the sources change.");
            dlg.addBand();
            TextBox tbOutput = dlg.addInputBox("&Output directory:", sOutput,
                "Where the results go. Leave it blank to write each beside its source.");
            Button btnChoose = dlg.addButton("&Choose output...", "Pick the output folder.");
            dlg.endBand();
            CheckBox cbRecursive = dlg.addCheckBox("&Include subfolders", bRecursive, "Look inside the folders within each source folder too.");
            CheckBox cbForce = dlg.addCheckBox("&Force replacements", bForce, "Replace a result already there, once its new conversion succeeds.");
            CheckBox cbView = dlg.addCheckBox("&View output", bViewOutput, "Open the output folder when the batch finishes.");

            Action refresh = delegate()
            {
                List<string> lsProblems = new List<string>();
                lsFiles = Batch.discover(Batch.tokens(tbSource.Text), cbRecursive.Checked, lsProblems);
                string sWanted = lbTarget.SelectedIndex >= 0 && lbTarget.SelectedIndex < lsTargets.Count ? lsTargets[lbTarget.SelectedIndex] : sTarget;
                List<string> lsLabels;
                lsTargets = Batch.targets(lsFiles, out lsLabels);
                dlg.setListItems(lbTarget, lsLabels);
                int iWanted = lsTargets.IndexOf(sWanted);
                if (lsTargets.Count > 0) lbTarget.SelectedIndex = iWanted >= 0 ? iWanted : 0;
                dlg.setStatusText(Batch.count(lsFiles.Count, "source file") + "; " + Batch.count(lsTargets.Count, "format")
                    + (lsProblems.Count > 0 ? ". " + lsProblems[0] : ""));
            };
            btnBrowse.Click += delegate(object oSender, EventArgs evArgs)
            {
                using (OpenFileDialog dlgOpen = new OpenFileDialog())
                {
                    dlgOpen.Multiselect = true;
                    dlgOpen.Title = "Browse source";
                    if (dlgOpen.ShowDialog(dlg.form) != DialogResult.OK) return;
                    List<string> lsQuoted = new List<string>();
                    foreach (string sFile in dlgOpen.FileNames) lsQuoted.Add("\"" + sFile + "\"");
                    tbSource.Text = string.Join(" ", lsQuoted.ToArray());
                    saveSetting("Source", tbSource.Text);
                    refresh();
                    tbSource.Focus();
                }
            };
            btnFolder.Click += delegate(object oSender, EventArgs evArgs)
            {
                using (FolderBrowserDialog dlgFolder = new FolderBrowserDialog())
                {
                    dlgFolder.Description = "Add a source folder";
                    if (dlgFolder.ShowDialog(dlg.form) != DialogResult.OK) return;
                    tbSource.Text = (tbSource.Text + " \"" + dlgFolder.SelectedPath + "\"").Trim();
                    saveSetting("Source", tbSource.Text);
                    refresh();
                    tbSource.Focus();
                }
            };
            btnChoose.Click += delegate(object oSender, EventArgs evArgs)
            {
                using (FolderBrowserDialog dlgFolder = new FolderBrowserDialog())
                {
                    dlgFolder.Description = "Choose the output folder";
                    if (Directory.Exists(tbOutput.Text)) dlgFolder.SelectedPath = tbOutput.Text;
                    if (dlgFolder.ShowDialog(dlg.form) != DialogResult.OK) return;
                    tbOutput.Text = dlgFolder.SelectedPath;
                    saveSetting("Output", tbOutput.Text);
                    tbOutput.Focus();
                }
            };
            tbSource.Leave += delegate(object oSender, EventArgs evArgs) { saveSetting("Source", tbSource.Text); refresh(); };
            tbOutput.Leave += delegate(object oSender, EventArgs evArgs) { saveSetting("Output", tbOutput.Text); };
            lbTarget.SelectedIndexChanged += delegate(object oSender, EventArgs evArgs)
            {
                if (lbTarget.SelectedIndex >= 0 && lbTarget.SelectedIndex < lsTargets.Count) saveSetting("Target", lsTargets[lbTarget.SelectedIndex]);
            };
            cbRecursive.CheckedChanged += delegate(object oSender, EventArgs evArgs) { saveSetting("Recursive", cbRecursive.Checked ? "1" : "0"); refresh(); };
            cbForce.CheckedChanged += delegate(object oSender, EventArgs evArgs) { saveSetting("Force", cbForce.Checked ? "1" : "0"); };
            cbView.CheckedChanged += delegate(object oSender, EventArgs evArgs) { saveSetting("ViewOutput", cbView.Checked ? "1" : "0"); };
            refresh();

            string sButton = dlg.runWithButtons(new string[] { "Default settings", "OK", "Cancel" });
            if (string.Equals(sButton, "Default settings", StringComparison.OrdinalIgnoreCase))
            {
                restoreDefaults();
                return runFromDialog();
            }
            if (!string.Equals(sButton, "OK", StringComparison.OrdinalIgnoreCase)) return 0;
            refresh();
            sSource = tbSource.Text.Trim();
            sOutput = tbOutput.Text.Trim();
            sTarget = lbTarget.SelectedIndex >= 0 && lbTarget.SelectedIndex < lsTargets.Count ? lsTargets[lbTarget.SelectedIndex] : "";
            bRecursive = cbRecursive.Checked;
            bForce = cbForce.Checked;
            bViewOutput = cbView.Checked;
        }
        List<string> lsReport = new List<string>();
        int iResult = runJob(lsFiles, lsReport, true);
        showResults(lsReport);
        return iResult;
    }

    private static bool restoreDefaults()
    {
        sSource = ""; sOutput = ""; sTarget = "md"; bRecursive = false; bForce = false; bViewOutput = true;
        saveSetting("Source", ""); saveSetting("Output", ""); saveSetting("Target", "md");
        saveSetting("Recursive", "0"); saveSetting("Force", "0"); saveSetting("ViewOutput", "1");
        Say.say("Settings restored to their defaults");
        return true;
    }

    private static int runFromCommandLine(string[] aArguments)
    {
        List<string> lsSources = new List<string>();
        bool bList = false;
        bool bViewGiven = false;
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
            else if (sSwitch == "view") bViewGiven = true;
            else if (sSwitch == "list") bList = true;
            else if (sSwitch == "defaults") { restoreDefaults(); return 0; }
            else if (sSwitch != "")
            {
                Console.WriteLine("Unknown switch " + sArgument + ". Run with /help to see the switches.");
                Log.error("Unknown switch: " + sArgument);
                return 1;
            }
            else lsSources.Add(sArgument);
        }
        List<string> lsProblems = new List<string>();
        List<string> lsFiles = Batch.discover(lsSources, bRecursive, lsProblems);
        foreach (string sProblem in lsProblems) { Console.WriteLine(sProblem); Log.error(sProblem); }
        if (lsFiles.Count == 0) { Console.WriteLine("No source file BatCon can convert was found."); return 1; }
        List<string> lsLabels;
        List<string> lsTargets = Batch.targets(lsFiles, out lsLabels);
        if (bList) { foreach (string sLabel in lsLabels) Console.WriteLine(sLabel); return 0; }
        if (!lsTargets.Contains(sTarget))
        {
            Console.WriteLine("Every source must convert to the format given with /to. These do: " + string.Join(", ", lsTargets.ToArray()) + ".");
            return 1;
        }
        bViewOutput = bViewGiven;
        List<string> lsReport = new List<string>();
        int iResult = runJob(lsFiles, lsReport, false);
        foreach (string sLine in lsReport) Console.WriteLine(sLine);
        return iResult;
    }

    // runJob: convert every file, saying how far the batch has got, and report each one.
    private static int runJob(List<string> lsFiles, List<string> lsReport, bool bSpeak)
    {
        if (lsFiles.Count == 0 || sTarget == "")
        {
            lsReport.Add("Nothing to do: choose source files and a format they convert to.");
            return 1;
        }
        string sOutputFull = sOutput == "" ? "" : Path.GetFullPath(Environment.ExpandEnvironmentVariables(sOutput));
        Log.info("Batch: " + lsFiles.Count + " files to " + sTarget + (sOutputFull == "" ? ", each beside its source" : " in " + sOutputFull)
            + (bForce ? ", replacing results" : ""));
        if (bSpeak) Say.say("Converting " + Batch.count(lsFiles.Count, "file") + " to " + sTarget);
        HashSet<string> hsReserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        HashSet<string> hsSources = new HashSet<string>(lsFiles, StringComparer.OrdinalIgnoreCase);
        int iConverted = 0, iFailed = 0, iSkipped = 0, iAt = 0;
        Stopwatch oSince = Stopwatch.StartNew();
        List<string> lsLines = new List<string>();
        foreach (string sFile in lsFiles)
        {
            iAt++;
            if (bSpeak && oSince.ElapsedMilliseconds >= iSpeakGap)
            {
                Say.say(iAt + " of " + lsFiles.Count + ": " + Path.GetFileName(sFile));
                oSince.Restart();
            }
            string sFolder = sOutputFull == "" ? Path.GetDirectoryName(sFile) : sOutputFull;
            string sResultPath = Batch.destination(sFile, sFolder, sTarget, hsReserved, hsSources);
            string sOutcome = Batch.convertOne(sFile, sResultPath, bForce);
            if (sOutcome.StartsWith("Converted")) iConverted++;
            else if (sOutcome.StartsWith("Skipped")) iSkipped++;
            else iFailed++;
            lsLines.Add(sOutcome);
        }
        lsReport.Add(Batch.count(iConverted, "file") + " converted, " + iSkipped + " skipped, " + iFailed + " failed.");
        lsReport.AddRange(lsLines);
        lsReport.Add("The log: " + Log.path);
        Log.info(lsReport[0]);
        if (bViewOutput)
        {
            string sOpen = sOutputFull != "" ? sOutputFull : Path.GetDirectoryName(lsFiles[0]);
            try { if (Directory.Exists(sOpen)) Process.Start(sOpen); } catch (Exception oError) { Log.error("View output: " + oError.Message); }
        }
        return iFailed > 0 ? 3 : 0;
    }

    private static bool showResults(List<string> lsReport)
    {
        HelpDialog.show(null, "BatCon results", string.Join("\r\n", lsReport.ToArray()));
        return true;
    }

    private static string helpText()
    {
        StringBuilder sbText = new StringBuilder();
        sbText.AppendLine("BatCon " + BuildVersion.Version + " -- convert many files at once between formats.");
        sbText.AppendLine("");
        sbText.AppendLine("  BatCon                                   the dialog");
        sbText.AppendLine("  BatCon <paths> /to <format> [/output <folder>] [/recursive] [/force] [/view]");
        sbText.AppendLine("  BatCon <paths> /list                     the formats the sources convert to");
        sbText.AppendLine("  BatCon /defaults                         settings back to their defaults");
        sbText.AppendLine("  BatCon /help");
        sbText.AppendLine("");
        sbText.AppendLine("A path is a file, a folder or a pattern such as *.pdf; /source <path> names one too.");
        sbText.AppendLine("Switches may be written /switch, -switch or --switch. Exit code: 0 done, 1 nothing to do or an");
        sbText.AppendLine("unknown switch, 3 some files failed. Each run writes a log under %LOCALAPPDATA%\\BatCon\\logs.");
        return sbText.ToString();
    }
}
}
