# BatCon

BatCon converts a batch of files between formats, using the conversions of the Homer Development Kit. It is a small program with one dialog: you say what to convert, to what, and where, and it does the rest.

## What it converts

BatCon converts whatever the Homer kit converts. Among the most useful:

- **Data tables:** CSV, inix, Markdown tables, TSV and Excel, each to the others.
- **Documents:** Word, web pages, Markdown, OpenDocument, rich text and plain text.
- **PDF:** to Markdown, a web page, Word or plain text, with recognition of scanned pages when Tesseract is installed.
- **Presentations:** Markdown to PowerPoint, and PowerPoint to Markdown or text.

The Target format list shows only what every source file converts to, so you can never pick a format one of them cannot reach.

## The dialog

The controls, in the order Tab moves through them:

- **Source files (Alt+S):** the files to convert. Type one or more files, folders or patterns such as `*.docx`, putting quotes around any with a space.
- **Browse source (Alt+B):** picks files with a standard Open dialog.
- **Add folder (Alt+A):** adds a folder; its files are converted.
- **Target format (Alt+T):** the format to convert to, by name, such as "Markdown (md)".
- **Output directory (Alt+O):** where the results go. Left blank, each result goes beside its source.
- **Choose output (Alt+C):** picks the output folder.
- **Include subfolders (Alt+I):** looks inside folders within each source folder too.
- **Force replacements (Alt+F):** replaces a result that is already there, once its new conversion succeeds.
- **View output (Alt+V):** opens the output folder when the batch finishes.
- **Default settings, OK and Cancel:** Default settings puts every choice back as it was first; OK, or Enter, starts the batch.

Every answer is saved as soon as you give it, so BatCon opens next time as you left it.

## While it works

BatCon says how many files it is converting, then names each file as it goes, no more often than every two seconds. At the end, a window lists every file: converted, skipped because its result was already there, or failed, with the reason. The last line names the log, which has every detail.

Your files are safe. Each result is written to a temporary file first and put in place only when its conversion works, so a failure never leaves half a file. A source is never overwritten, and two sources with the same name get name_2, name_3 and so on.

## From a command line

    BatCon "C:\Reports\*.pdf" /to md /output C:\Reports\Markdown
    BatCon C:\Data /recursive /to xlsx
    BatCon C:\Reports /list

- **/to format:** the format to convert to, by its extension.
- **/output folder:** where the results go.
- **/recursive:** includes subfolders.
- **/force:** replaces results already there.
- **/view:** opens the output folder at the end.
- **/list:** prints the formats the sources convert to, and converts nothing.
- **/defaults:** puts the settings back as they were first.
- **/help:** prints the usage.

A switch may also be written with a dash or two, such as -to or --to. The exit code is 0 when all went well, 1 when there was nothing to do or a switch was unknown, and 3 when some files failed.

## Where things are

- **The program:** C:\Program Files\BatCon.
- **Your settings:** %LOCALAPPDATA%\BatCon\configs\BatCon.inix.
- **The logs:** %LOCALAPPDATA%\BatCon\logs, one for each run.
