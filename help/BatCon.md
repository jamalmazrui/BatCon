# BatCon

BatCon converts a batch of files between formats, using the conversions of the Homer Development Kit. It is a small program with one dialog: you say what to convert, to what, and where, and it does the rest.

## What it converts

BatCon converts whatever the Homer kit converts. Among the most useful:

- **Data tables:** CSV, inix, Markdown tables, TSV and Excel, each to the others.
- **Documents:** Word, web pages, Markdown, OpenDocument, rich text and plain text.
- **PDF:** to Markdown, a web page, Word or plain text, with recognition of scanned pages when Tesseract is installed.
- **Presentations:** Markdown to PowerPoint, and PowerPoint to Markdown or text.
- **Recordings to transcripts:** an audio or video file becomes a transcript -- plain text, Markdown, a web page, Word or PDF, among others -- written by Whisper on your own computer.
- **Documents to speech:** a document becomes an MP3 or other audio file in Windows' own voice, or an MP4 or other video, the speech over a plain black picture.
- **Audio, video and pictures:** each to the other formats of its kind, such as WAV to MP3 or PNG to JPEG.

The Target format list shows every format BatCon can make. A source that cannot become the one you pick is skipped, and BatCon says why.

## The dialog

BatCon's dialog has the same shape as 2htm's and extCheck's. The controls, in the order Tab moves through them:

- **Source files (Alt+S):** the files to convert. It starts at your Documents folder. Type one or more files, folders or patterns such as `*.docx`, putting quotes around any with a space.
- **Browse source (Alt+B):** picks a folder of files with a standard Browse for Folder dialog.
- **Output directory (Alt+O):** where the results go. It follows Source files -- the folder itself, or the folder of a single file or pattern -- until you choose another. Blank means each result goes beside its source.
- **Choose output (Alt+C):** picks the output folder.
- **Target format (Alt+T):** the format to convert to, from every format BatCon can make, in alphabetical order by name, such as "Markdown document (md)".
- **Include subfolders (Alt+I):** looks inside the folders within each source folder too.
- **Force replacements (Alt+F):** replaces a result already there, once its new conversion succeeds, instead of keeping it.
- **View output (Alt+V):** opens the output folder when the batch finishes.
- **Log session (Alt+L):** also writes BatCon.log, the run's log, in the output folder.
- **Use configuration (Alt+U):** when checked as you press OK, saves every field and starts from them next time. Unchecked, BatCon starts from its defaults.
- **OK, Guide, Default settings, Cancel and Help:** OK, or Enter, starts the batch, and offers to make an output folder that does not exist. Guide opens this guide. Default settings puts every field back to its default and forgets the saved configuration. Help, or F1, lists each field and checks for a newer BatCon; F11 checks at once.

## While it works

A file whose type does not convert to the target is skipped, and BatCon says so once for each type, with the reason, such as "Skipping 3 files of type wav: the Homer kit has no conversion from wav to xlsx." BatCon then says how many files it is converting, and names each as it goes, no more often than every two seconds. At the end, a window gives the counts -- converted, kept because a result was already there, skipped for want of a conversion, and failed -- then each skipping message and each file's outcome with its reason. The last line names the log, which has every detail.

Your files are safe. Each result is written to a temporary file first and put in place only when its conversion works, so a failure never leaves half a file. A source is never overwritten, and two sources with the same name get name_2, name_3 and so on.

## From a command line

    BatCon "C:\Reports\*.pdf" /to md /output C:\Reports\Markdown
    BatCon C:\Data /recursive /to xlsx
    BatCon /list

- **/to format:** the format to convert to, by its extension.
- **/output folder:** where the results go.
- **/recursive:** includes subfolders.
- **/force:** replaces results already there.
- **/view:** opens the output folder at the end.
- **/list:** prints every format BatCon can convert to, and converts nothing.
- **/log:** also writes BatCon.log in the output folder.
- **/defaults:** puts the settings back as they were first.
- **/help:** prints the usage.

A switch may also be written with a dash or two, such as -to or --to. The exit code is 0 when all went well, 1 when there was nothing to do or a switch was unknown, and 3 when some files failed.

## Where things are

- **The program:** C:\Program Files\BatCon.
- **Your settings, when Use configuration is checked:** %LOCALAPPDATA%\BatCon\configs\BatCon.inix.
- **The logs:** %LOCALAPPDATA%\BatCon\logs, one for each run.
