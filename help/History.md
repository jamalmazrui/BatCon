# BatCon History

## 30 October 2026 -- more fixes from an audit by another AI

- **An incomplete run is not reported as a success.** A path that could not be found, or a run stopped with Cancel, ended with exit code 0. Both now end with 3, as a failed conversion does; a type skipped for want of a conversion still does not.

## 30 October 2026 -- fixes from an audit by another AI

- **A converted document's title is its own name.** A result made under BatCon's temporary name took ".batcon-" and a long number as its title. BatCon now tells the Homer kit the name the file is made for. The build needs HomerDev 1.94.1 or later.

## 25 October 2026, evening

- **Target formats read extension first,** "txt (plain text)", so the list runs in extension order and pressing t reaches txt (HomerDev 1.93.7).
- **The progress window keeps the focus,** running on a thread of its own, so you can stay on its Status box while the work goes on, and the time left is said without "about": "3 min left" (HomerDev 1.93.6).

## 25 October 2026, later

- **Built against HomerDev 1.93.5,** whose checks and tidy use the newest kit found, and whose tidy cleared the kit copy from the top of C:\. kitNeeded is 1.93.5.

## 24 October 2026

- **Its acceptance checks convert its own guide to Word, EPUB and PDF,** so a build that cannot do the job fails before it is released. Its spoken progress is also shown on a braille display (HomerDev 1.92.3).
- **Word, Excel and PowerPoint files it makes are judged by extCheck's rules,** from the kit (HomerDev 1.92.1).
- **The help names the source switch on a line of its own,** -s, --source-paths, which its acceptance check looks for; the release had stopped on it.
- **The build uses the newest Homer kit it finds,** logging each one: your HomerDev variable, C:\HomerDev and the folders above. The variable named an older installed kit, which stopped BatCon's build.
- **Markdown it makes is judged by every extCheck rule,** and every PDF is checked for tags, from the kit (HomerDev 1.92.0).
- **PDF and db are made too.** A PDF is printed by Microsoft Edge from a web page, so no Office is needed, and a db is an SQLite file of one table. Any source converts to any target the kit can reach, through Markdown when no direct route exists (HomerDev 1.90.0).
- **The build finds the kit again.** It climbed from its folder and took C:\ itself for the kit, so it stopped with "kit 0.0.0 is older than 1.89.0". It now tries C:\HomerDev first, accepts a kit only where HomerDev.cs is too, and honours the HomerDev environment variable.

## 1.0.3, 23 October 2026

- **Target format offers twelve formats:** csv, db, docx, epub, htm, inix, md, pdf, pptx, tsv, txt and xlsx, sorted by name. The kit cannot make a db yet, so a source is skipped for it, with the reason.
- **The dialog is the one every Homer launch app shows,** built by the Homer kit's LaunchDialog (kit 1.89.0). It is in rows called bands, read left to right: Source paths and Browse local source; Target format; Output directory and Choose output directory; Force replacements, Use configuration and View output directory; Default settings and Help; then OK and Cancel alone at the bottom right, as Microsoft's dialog guidance places them.
- **Insert+Tab names the Source paths box again, and the Output directory box is found again.** The kit put a band's label after the band, so the box had no name and Browse seemed to come first.
- **Control+Enter is OK and Escape is Cancel from any control, and F11 checks for a newer version from anywhere.** Help says so, and says where the logs are kept.
- **Logging is automatic.** The Log session checkbox and /log are gone; every run writes %LOCALAPPDATA%\BatCon\logs\BatCon-yyyyMMdd-HHmmss.log. The Guide button and Include subfolders are gone too: a folder means the files in it.
- **Every run ends with the Results box,** whose one OK button has the focus, so Enter, Space or Escape closes it; the output directory then opens if View output directory is checked, and BatCon closes.
- **Switches match the dialog:** a long switch is a control's label and a short one its Alt letter, written with /, - or --, in any case. New: -t, --target-format; -o, --output-directory; -u; -d, --default-settings. /recursive is gone. The older names still work.
- **The work has a progress window, the kit's LaunchProgress:** "BatCon, <status>" in its title, a Status box with the focus, the percentage done and time left ("2 of 5 done, 40%, about 3 min left"), and Cancel (Escape) to stop after the current file. Progress used to be spoken every two seconds even while you worked in another program; now it is spoken only while BatCon is the window in front, and coming back says where things stand.
- **Force replacements works as in every launch app:** unchecked, a source whose result is already there is skipped and kept, and the Results box says how many and how to replace them; checked, the work is done again and the old result kept until the new one is complete. Its tip is now the kit's standard one.
- **The build needs HomerDev 1.89.0 or later.**

## 1.0.2, 20 October 2026

- **PDFs are read with the Homer kit's own PDF reader,** which keeps a tagged PDF's structure and reads an untagged one by its layout. The build fetches what it needs, PdfPig, by itself. It needs HomerDev 1.88.0 or later.
- **Web pages in the Homer page form,** as every Homer app makes them: one heading for the title, the page's parts marked for a screen reader, and pictures carried inside the page. The build needs HomerDev 1.87.1 or later.
- **The spoken tutorials are full length,** three to five minutes each, as every Homer app's are: the dialog, installing, PDFs to Markdown, tables to workbooks, where results go, converting again, the command line, and reading the results.
- **Recordings become transcripts, and documents become speech.** An audio or video file converts to a transcript in any document format, by Whisper on your computer; a document converts to audio or video in Windows' own voice. Audio, video and pictures convert among their own formats too. All through the Homer kit, as every Homer app converts. The installer offers ffmpeg and Whisper for these.
- **The dialog has the shape of 2htm's and extCheck's,** the pattern of every Homer launchpad dialog. Add folder is gone: Browse source picks a folder, and files or patterns are typed. Use configuration, Log session and the Guide button are new; F11 checks for a newer BatCon, as Help (F1) does at its end.
- **The defaults are deliberate:** Source files starts at your Documents folder, and Output directory follows the source, typed or browsed, until you choose another folder.
- **Target format lists every format BatCon can make,** in alphabetical order by name, from the start. A source that does not convert to the chosen format is skipped, said once for each type with the reason, and counted in the Results box.
- **Settings are kept only when Use configuration is checked** as you press OK; Default settings forgets them.
- **The spoken tutorials ship.** The first release went out without them: the tutorial checker found eight problems in the walks and spoke nothing, and the build went on regardless. The walks are fixed, and a walk that cannot be spoken now stops the build.

## 1.0.0, 19 October 2026

- **The first release.** BatCon converts a batch of files between any two formats the Homer kit converts, from a dialog or a command line.
- **The formats offered are the ones every source converts to,** by the kit's readable names.
- **Output on a command line goes where it is sent.** BatCon writes to the console it was started from only when its output is not already redirected, so a script, or the kit's check, reads what /help and /list print.
- **The first BatCon's leftovers are retired by the build:** its copy of the kit, its third-party folder, its extra documents and scripts, and the PDF libraries it had put in exec, each moved into notes\retired\firstBatCon and logged.
- **Results are written safely:** to a temporary file first, put in place only when the conversion works, never over a source.
