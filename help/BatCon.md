# BatCon

BatCon converts a batch of files between formats, using the conversions of the Homer Development Kit. It is a small program with one dialog: you say what to convert, to what, and where, and it does the rest.

## What it converts

BatCon converts whatever the Homer kit converts. Among the most useful:

- **Data tables:** CSV, inix, Markdown tables, TSV and Excel, each to the others.
- **Documents:** Word, web pages, Markdown, OpenDocument, rich text and plain text.
- **PDF:** to Markdown, a web page, Word or plain text, with recognition of scanned pages when Tesseract is installed.
- **Presentations:** Markdown to PowerPoint, and PowerPoint to Markdown or text.
- **Recordings to transcripts:** an audio or video file becomes a transcript -- plain text, Markdown, a web page, Word or PDF, among others -- written by Whisper on your own computer. With HomerScribe installed, a video also gets a description of what can be seen, in the same document.
- **Documents to speech:** a document becomes an MP3 or other audio file in Windows' own voice, or an MP4 or other video, the speech over a plain black picture.
- **Audio, video and pictures:** each to the other formats of its kind, such as WAV to MP3 or PNG to JPEG.

The Target format list offers twelve formats: CSV, an SQLite database (db), Word, EPUB, a web page, inix, Markdown, PDF, PowerPoint, TSV, plain text and Excel. A source that cannot become the one you pick is skipped, and BatCon says why. A database (db) is an SQLite file with one table named after it, its fields from the source's first table. A PDF is made through a web page that Microsoft Edge prints, so no Office is needed.

## The dialog

BatCon's dialog is the one every Homer launch app shows, with the same names and the same Alt letters in each. It is in rows called bands, and Tab moves through them in this order:

- **Source paths (Alt+S):** the files to convert. It starts at your Documents folder. Type one or more files, folders or patterns such as `*.docx`, putting quotes around any with a space. A folder means the files in it, not the folders inside it.
- **Browse local source (Alt+B):** picks a folder of files with a standard Browse for Folder dialog. It opens where Source paths points, or else in Documents.
- **Target format (Alt+T):** the format to convert to, in alphabetical order by name, such as "Markdown document (md)".
- **Output directory (Alt+O):** where the results go. It follows Source paths -- the folder itself, or the folder of a single file or pattern -- until you choose another. Blank means each result goes beside its source.
- **Choose output directory (Alt+C):** picks the output folder.
- **Force replacements (Alt+F):** Do the work again for a source whose result is already there, replacing the old result once the new one is complete. Unchecked, that source is skipped, its result kept, and the Results box says how many.
- **Use configuration (Alt+U):** when checked as you press OK, saves every field and starts from them next time. Unchecked, BatCon starts from its defaults.
- **View output directory (Alt+V):** opens the output folder after you close the Results box.
- **Default settings (Alt+D):** puts every field back to its default and forgets the saved configuration.
- **Help (Alt+H or F1):** lists each field with its tip, the dialog's keys and where the logs are kept, and checks for a newer BatCon.
- **OK and Cancel:** OK starts the batch, and offers to make an output folder that does not exist. Control+Enter presses OK and Escape presses Cancel, from anywhere in the dialog. F11 checks for a newer BatCon from anywhere, too.

## While it works

A file whose type does not convert to the target is skipped, and BatCon says so once for each type, with the reason, such as "Skipping 3 files of type wav: the Homer kit has no conversion from wav to xlsx." While it works, BatCon shows a small window titled with its name and what it is doing, such as "BatCon, Budget.pdf, 2 of 5 done, 40%". Once it can estimate honestly, it adds the time left, such as "about 3 min left", and a progress bar shows the same. Its Status box has the focus, so Insert+Tab reads the latest status; Cancel, or Escape, stops after the current file. Your screen reader speaks the progress only while BatCon is the window you are in, so you can work in another program undisturbed; coming back says where things stand. At the end, the BatCon Results box gives the counts -- converted, kept because a result was already there, skipped for want of a conversion, and failed -- then each skipping message and each file's outcome with its reason. The last line names the log, which has every detail. A long report shows its first 15 lines and names a file with the rest. Its only button is OK, so Enter, Space or Escape closes it, and then BatCon closes too.

Your files are safe. Each result is written to a temporary file first and put in place only when its conversion works, so a failure never leaves half a file. A source is never overwritten, and two sources with the same name get name_2, name_3 and so on.

## From a command line

    batcon "C:\Reports\*.pdf" -t md -o C:\Reports\Markdown
    batcon C:\Data -t xlsx -v
    batcon /list

Each long switch is a name from the dialog, and each letter is that control's Alt letter:

- **-t, --target-format format:** the format to convert to, by its extension.
- **-o, --output-directory folder:** where the results go.
- **-f, --force-replacements:** Do the work again for a source whose result is already there, replacing it; without it, that source is skipped and its result kept.
- **-u, --use-configuration:** starts from the settings the dialog saved.
- **-v, --view-output-directory:** opens the output folder at the end.
- **-d, --default-settings:** puts the settings back as they were first.
- **/list:** prints every format BatCon can convert to, and converts nothing.
- **-h, --help:** prints the usage.

A switch may be written with a slash, a dash or two dashes, in any case. The older names, such as /to, /output, /force and /view, still work. The exit code is 0 when all went well, 1 when there was nothing to do or a switch was unknown, and 3 when some files failed.

## Where things are

- **The program:** C:\Program Files\BatCon.
- **Your settings, when Use configuration is checked:** %LOCALAPPDATA%\BatCon\configs\BatCon.inix.
- **The logs:** %LOCALAPPDATA%\BatCon\logs, one for each run, named BatCon-yyyyMMdd-HHmmss.log for the date and time it started. Logging is automatic; there is nothing to switch on.
