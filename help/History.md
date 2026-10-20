# BatCon History

## 1.0.2, 20 October 2026

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
