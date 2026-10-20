# BatCon walkthroughs

These are written walkthroughs for this development build. Spoken tutorials and recorded screen-reader responses are not included.

## Convert a document

1. Run BatCon.cmd.
2. In Source, type the full path of a PDF, or press Browse source and choose it.
3. Tab to Target format. Use the arrow keys to choose md.
4. In Output, type a directory for the result, or use Choose output.
5. Press Control+Enter. The conversion window shows progress.
6. Read Results. The converted Markdown file is in the Output directory. The log gives the conversion route.

## Convert a table

1. Run BatCon.cmd.
2. In Source, enter an INIX file containing record sections.
3. In Target format, choose xlsx.
4. Choose an Output directory and press OK.
5. Open the workbook. The Data sheet has one column for each field. Leading zeros remain text, and values beginning with an equals sign remain literal values.

## Convert a folder

1. Use Add folder to append a directory.
2. Check Include subfolders if needed.
3. Press Refresh files. The status gives the source count and number of common target formats.
4. Choose a Target format and Output directory, then press OK.
5. Use Stop after current file if you need to end the batch. Read the unprocessed count in Results.

## Repeat safely

1. Run a completed batch again with Force replacements unchecked.
2. Results report existing outputs as skipped.
3. Check Force replacements only when you intend to replace those outputs.
4. Run again. A failed conversion preserves the existing output.

<!-- walkthrough: written by makeTutorials.py, do not edit between the markers -->

## 0 - Overview

What BatCon does, and what each walk covers.

**Before you start:** Nothing yet. This one is two minutes of what to expect.

### Step 1

BatCon converts many files at once, from one format to another: PDF to Markdown, Word to a web page, a table to Excel. It is one small dialog, made for the keyboard and the screen reader.

### Step 2

The walks: one, the dialog; two, installing and opening it; three to eight, a task each, from PDFs to the command line; nine, what to carry away. Take one and two first; the rest in any order.

### Step 3

Two voices take these walks. I am the host; the other voice plays the screen reader. It is a simulation, so your own reader's words may differ a little.

### Step 4: Insert+Tab

Three screen reader keys help whenever you are unsure, and Caps Lock serves in place of Insert where your reader is set up that way. Insert plus Tab says what has the focus.

Screen reader:

- Source files: edit

### Step 5: Insert+T

Insert plus T says the title of the window.

Screen reader:

- BatCon

### Step 6: Insert+Up Arrow

And Insert plus Up Arrow says again the line you are on.

Screen reader:

- C colon backslash Reports backslash star dot p d f

### Step 7

What this walk taught: what BatCon is for, and the order of the walks.

**Something to try:** Pick the walk that matches what you want to do today.

## 1 - User Interface

The one dialog, control by control, and the keys that reach each one.

**Before you start:** BatCon is installed and your reader is running.

### Step 1: Alt+Control+B

BatCon has one dialog, and everything it does starts there. Alt plus Control plus B, B for BatCon, opens it from anywhere in Windows.

Screen reader:

- BatCon dialog
- Source files: edit, Documents

### Step 2

Focus starts in Source files. It begins at your Documents folder, so a first run already has somewhere sensible to look. Here you name what to convert: one file, a whole folder, or a pattern such as star dot P D F for every PDF in a folder.

### Step 3: Tab

Tab moves forward through the dialog, one control at a time, in the order they are described in this walk. The next control is the button Browse source.

Screen reader:

- Browse source... button

### Step 4

Browse source opens the standard Browse for Folder dialog, for choosing a folder of files. The three dots after its name mean it opens another window rather than acting at once.

### Step 5: Tab

Tab again, and you reach Output directory, where the converted files go. It follows Source files: it names the folder the sources are in, until you choose a different one.

Screen reader:

- Output directory: edit, Documents

### Step 6: Tab

Left blank, each converted file is written beside its source. Beside the field is Choose output, for picking a folder with the same Browse for Folder dialog.

Screen reader:

- Choose output... button

### Step 7: Alt+T

Every control has an Alt key, the first letter of a word in its name, so you need not tab through the whole dialog. Alt plus T, Tango, goes straight to Target format.

Screen reader:

- Target format: list box
- Markdown document (md)

### Step 8

Target format lists every format BatCon can make, in alphabetical order by name, each with its extension in parentheses. Up and Down Arrow move through it, and a letter jumps to the first name starting with it.

### Step 9: Alt+I

After the list come the check boxes. Alt plus I, India, is Include subfolders, for looking inside the folders within a source folder too.

Screen reader:

- Include subfolders, check box, not checked

### Step 10: Alt+U

Space checks a box, and Space again clears it. The others are Force replacements, Alt plus F; View output, Alt plus V; Log session, Alt plus L; and Use configuration, Alt plus U.

Screen reader:

- Use configuration, check box, not checked

### Step 11

Use configuration matters most of these four. Checked when you press OK, it keeps every choice you made, so BatCon opens next time just as you left it. Unchecked, BatCon starts each time from its defaults.

### Step 12: Control+Enter

Below the check boxes is a row of buttons: OK, Guide, Default settings and Cancel. Enter presses OK from most controls, and Control plus Enter presses it from anywhere.

Screen reader:

- Converting 3 files to md

### Step 13

Guide opens the BatCon guide in your web browser. Default settings puts every control back as it was the first time you opened BatCon. Escape cancels, and closes the dialog without converting anything.

### Step 14: F1

Two more keys help at any moment. F1 opens Help, which lists every control with what it is for, and ends by checking the web for a newer BatCon. F11 checks for a newer version at once, without the list.

Screen reader:

- BatCon Help

### Step 15

What this walk taught: Alt plus Control plus B opens BatCon; Alt with a control's first letter goes to it; Space checks a box; and Enter or Control plus Enter presses OK.

**Something to try:** Open BatCon and visit each control with its Alt key, then press Escape.

## 2 - Install and Launch

Installing BatCon, what the installer offers, and opening the program.

**Before you start:** A web browser is open, and your reader is running.

### Step 1

BatCon installs from its page on GitHub, from a single file, BatCon underscore setup dot E X E. Downloading it and pressing Enter on it starts the installer.

### Step 2: Alt+Y

Windows first asks whether to let the installer make changes, since BatCon installs for every user of the computer. Alt plus Y, Yankee, answers yes.

Screen reader:

- Setup - BatCon dialog

### Step 3: Enter

The installer asks a few questions, and each has the usual answer ready, so Enter moves you on. The first is where to install; the usual place is the Program Files folder.

Screen reader:

- Select Destination Location

### Step 4: Enter

Keep pressing Enter through the questions. When the files are copied, the last page lists the extras BatCon can use, each as a check box, ticked when it is needed.

Screen reader:

- Completing the BatCon Setup Wizard

### Step 5

The extras are tools BatCon converts with. Pandoc and the PDF tools turn documents into other documents. ffmpeg handles sound and video. Whisper writes down what is said in a recording, on your own computer.

### Step 6: Enter

A box already ticked means the tool is missing or out of date. A box not ticked means it is already there and current. Enter installs whatever is ticked.

Screen reader:

- BatCon Setup

### Step 7: Enter

When the installer finishes, a Results box says what it did with each tool: installed, updated, or already current. Down Arrow reads it a line at a time, and Enter closes it.

The reader says nothing as the box closes; the installer has finished.

### Step 8

To install with no questions at all, start the installer with slash default after its name. It then takes every usual answer by itself, as if you had pressed Enter on each screen.

### Step 9: Alt+Control+B

Now open BatCon with its desktop key, Alt plus Control plus B. It works from anywhere in Windows, whatever program you are in.

Screen reader:

- BatCon dialog
- Source files: edit, Documents

### Step 10: Windows

BatCon is also on the Start menu. The Windows key, then the word BatCon, then Enter, opens it the same way.

Screen reader:

- Search box

### Step 11: Alt+U

Choices are kept between runs only when Use configuration is checked as you press OK; otherwise BatCon starts fresh each time, from its defaults.

Screen reader:

- Use configuration, check box, not checked

### Step 12

Updating later is the same: run the newest setup file, and it installs over the old one, keeping your settings. Inside BatCon, F11 checks for a newer version, says whether there is one, and offers to install it.

### Step 13

If a tool on the finish page could not be installed, the Results box says why, and BatCon still works for every conversion that does not need it. A conversion that does need it is reported as failed, with the tool named.

### Step 14: Windows+I

Uninstalling is from Windows Settings, under Apps, like any program; Windows plus I opens Settings. Your saved settings and logs stay in your own folder until you remove them.

Screen reader:

- Settings

### Step 15

What this walk taught: where to get BatCon, that the installer offers the tools BatCon needs, slash default for an install with no questions, and Alt plus Control plus B to open it.

**Something to try:** Install BatCon, then open it with its desktop key.

## 3 - Convert PDFs to Markdown

A folder of PDF files, each made a Markdown file you can read line by line.

**Before you start:** BatCon is open, and a folder holds a few PDF files.

### Step 1

Markdown is plain text with a little structure: headings start with a number sign, and list items with a dash. A screen reader reads it line by line, and its headings make a long document easy to move through.

### Step 2: Alt+S

Start in Source files. Alt plus S, Sierra, goes there from anywhere in the dialog.

Screen reader:

- Source files: edit, Documents

### Step 3

Type the folder and a pattern together, here C colon backslash Reports backslash star dot P D F. The star stands for any name, so this means every PDF in Reports.

C:\Reports\*.pdf

### Step 4: Tab

Tab away from the field. Output directory follows what you typed, and now names the Reports folder.

Screen reader:

- Browse source... button

### Step 5: Alt+O

Alt plus O, Oscar, to hear it.

Screen reader:

- Output directory: edit, C colon backslash Reports

### Step 6: Alt+T

Alt plus T, Tango, for Target format. Markdown document may already be chosen; if not, press M to jump to it.

Screen reader:

- Markdown document (md)

### Step 7

A tagged PDF, made with care for accessibility, keeps its headings and lists. An untagged one is read by its layout: large bold lines become headings, and columns are put in reading order.

### Step 8

A scanned PDF holds pictures of pages rather than text. BatCon reads those by recognizing the letters, which takes longer, and needs the Tesseract tool installed.

### Step 9: Enter

Press Enter to start. BatCon says how many files it is converting, then names each as it goes, no more often than every two seconds, so a long batch is never silent.

Screen reader:

- Converting 3 files to md
- 2 of 3: Budget dot p d f

### Step 10

When it is done, the Results box opens. Its first line is the summary.

Screen reader:

- BatCon results
- 3 files converted, 0 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 11: Down Arrow

Down Arrow reads each file's line, saying what it became.

Screen reader:

- Converted Budget dot p d f to Budget dot m d.

### Step 12: Escape

Escape closes the Results box. The Markdown files are now beside the PDFs, with the same names and the extension dot m d.

The reader says nothing as the window closes.

### Step 13: Alt+I

Check Include subfolders, Alt plus I, India, to take in the PDFs in every folder below Reports as well. Each result goes beside its own source, or into the output folder.

Screen reader:

- Include subfolders, check box, checked

### Step 14: Alt+T

A web page is the other good target for reading. Pick Web page in Target format, Alt plus T and the letter W, and each PDF becomes a page with the same headings, which a browser's heading keys move through.

Screen reader:

- Web page (htm)

### Step 15

Word document is a third choice, when someone wants to edit the text: after Web page, the letter W twice more passes Word 97 document and reaches it.

### Step 16

What this walk taught: a pattern in Source files, Markdown document in Target format, and Enter; and that scanned pages are read by recognizing the letters.

**Something to try:** Convert a folder of your own PDFs.

## 4 - Turn a Table into a Workbook

A table in CSV, inix or Markdown made an Excel workbook.

**Before you start:** BatCon is open, and a CSV file is at hand.

### Step 1

A CSV file is a table written as plain text, with commas between the values on each line. Excel opens one, but a true workbook keeps each value in its own cell and holds up better when shared.

### Step 2: Alt+S

Source files starts at your Documents folder. Alt plus S, Sierra, and type the one file you want, here C colon backslash Data backslash Members dot c s v.

Screen reader:

- Source files: edit, Documents

### Step 3

A single file is fine; Source files takes one file as readily as a folder.

C:\Data\Members.csv

### Step 4: Alt+O

Output directory follows: for one file, it names the folder that file is in, so the workbook will go beside it.

Screen reader:

- Output directory: edit, C colon backslash Data

### Step 5: Alt+T

Alt plus T, Tango, goes to Target format. The list is in alphabetical order, so the letter E lands first on EPUB book, and E again on Excel workbook.

Screen reader:

- Excel workbook (xlsx)

### Step 6

Every value goes into the workbook as text. A member number written 007 stays 007 rather than losing its zeros, and a value that begins with an equals sign stays words rather than becoming a formula.

### Step 7

Tables come in several forms, and BatCon converts among them all: comma separated, tab separated, a Markdown table, a Homer inix file, and an Excel workbook.

### Step 8: Enter

Press Enter to convert. A single file is quick, so the summary is all you hear.

Screen reader:

- BatCon results
- 1 file converted, 0 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 9: Down Arrow

Down Arrow reads the line for the file.

Screen reader:

- Converted Members dot c s v to Members dot x l s x.

### Step 10: Escape

Escape closes the Results box, and Members dot x l s x is beside the CSV file, ready for Excel.

The reader says nothing as the window closes.

### Step 11

The same works the other way. An Excel workbook as the source, with Comma separated table as the target, gives a CSV file of its first sheet, to read in any text editor.

### Step 12: Alt+S

A whole folder of tables converts the same way: put the folder in Source files, and each table becomes a workbook of its own. Files of other kinds in the folder are skipped, and the Results box says so.

Screen reader:

- Source files: edit, C colon backslash Data backslash Members dot c s v

### Step 13

A Markdown document with a table in it converts too: the table becomes the workbook's sheet. That is handy for a table typed by hand in a text editor.

### Step 14

The Results box counts what was converted, kept and skipped; walk 8 reads one in full.

### Step 15

An inix file is the Homer settings format: plain text in sections, each with names and values. Converting one to a workbook gives a sheet with a row for each section and a column for each name, which is an easy way to review many settings at once.

Screen reader:

- Excel workbook (xlsx)

### Step 16

Going back, a workbook converts to an inix file or a Markdown table, which a screen reader reads in order, row by row, with each value announced after its column's name.

### Step 17

What this walk taught: a single file can be the source, E twice reaches Excel workbook, and every value keeps its exact text.

**Something to try:** Make a workbook from a table of your own.

## 5 - Choose Where Results Go

Results in a folder of their own, the folder made if needed, and opened when the batch ends.

**Before you start:** BatCon is open with some source files named.

### Step 1

By default, each converted file goes beside its source. That keeps a file and its conversion together, but a batch of fifty conversions can crowd a folder.

### Step 2: Alt+O

Alt plus O, Oscar, is Output directory. It names the source's folder until you choose another.

Screen reader:

- Output directory: edit, C colon backslash Reports

### Step 3: Alt+C

To choose a folder by browsing, press Alt plus C, Charlie, for Choose output. The standard Browse for Folder dialog opens.

Screen reader:

- Choose the output directory

### Step 4: Enter

Arrow keys move through the folders, and Right Arrow opens one to show those inside it. Enter chooses the folder you are on.

Screen reader:

- Output directory: edit, C colon backslash Reports backslash Converted

### Step 5: Enter

You can also type a folder that does not exist yet, such as C colon backslash Reports backslash Markdown. When you press OK, BatCon offers to make it.

Screen reader:

- Create C colon backslash Reports backslash Markdown?
- Yes button

### Step 6: Enter

Yes is the usual answer, so Enter makes the folder and the batch goes on. No returns you to the dialog with nothing changed.

Screen reader:

- Converting 3 files to md

### Step 7

Once you choose an output folder, it stays put even if you change Source files; only a folder that still mirrors the source moves with it.

### Step 8

Two sources with the same name never overwrite each other's result. The second becomes its name followed by underscore 2, the third underscore 3, and so on.

### Step 9: Alt+V

To open the output folder when a batch ends, check View output: Alt plus V, Victor.

Screen reader:

- View output, check box, checked

### Step 10

With View output checked, File Explorer opens on the output folder after the Results box, with the new files there.

Screen reader:

- Markdown

### Step 11: Alt+L

Log session, Alt plus L, also puts the run's full log in the output folder, as BatCon dot log, handy when you want to see later exactly what happened.

Screen reader:

- Log session, check box, checked

### Step 12: Alt+U

Use configuration, Alt plus U, keeps the output folder and the check boxes for next time, so a folder you send every batch to need only be chosen once.

Screen reader:

- Use configuration, check box, checked

### Step 13

Default settings puts it all back: Source files at Documents, Output directory following it, and every box as it was the first time. It is one of the buttons below the check boxes.

### Step 14: Alt+O

Blank is also a choice. Clear Output directory, and each result goes beside its own source, even when the sources are in many folders.

Screen reader:

- Output directory: edit, blank

### Step 15: Alt+C

Choose output opens where Output directory already points, so a folder near the one you want is only a few arrow keys away.

Screen reader:

- Choose the output directory

### Step 16: Escape

Escape closes the Browse for Folder dialog without changing anything, and returns you to Output directory.

Screen reader:

- Output directory: edit, C colon backslash Reports

### Step 17

What this walk taught: Alt plus O and Alt plus C for the output, that BatCon makes a missing folder when you say yes, and Alt plus V and Alt plus L for after the batch.

**Something to try:** Send a batch to a new folder and let View output open it.

## 6 - Convert Again

Running a batch a second time: what is kept, and how to replace it safely.

**Before you start:** BatCon is open, and a batch has been converted once.

### Step 1

Sooner or later you convert the same files again: the sources changed, or you want the newest conversion. BatCon is careful with what is already there.

### Step 2: Enter

Run the same batch a second time, as it was. Enter starts it.

Screen reader:

- BatCon results

### Step 3

Every result already there is kept as it was, and the summary says so.

Screen reader:

- 0 files converted, 3 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 4: Down Arrow

Down Arrow reads why each was kept.

Screen reader:

- Skipped Budget dot m d: it is already there.

### Step 5: Escape

That is the safe choice: nothing you made earlier, or edited since, is replaced unless you ask. Escape closes the Results box.

The reader says nothing as the window closes.

### Step 6: Alt+F

To replace the results, open BatCon again, and check Force replacements: Alt plus F, Foxtrot.

Screen reader:

- Force replacements, check box, checked

### Step 7: Enter

Press Enter. This time every file is converted again.

Screen reader:

- Converting 3 files to md
- 3 files converted, 0 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 8

Even with Force replacements, an earlier result is replaced only once its new conversion has worked. Each conversion is written to a temporary file first, and put in place only when it succeeds, so a failure never loses what you had.

### Step 9

A source is never overwritten either. If a conversion's name would match a source's, BatCon gives the result a new name instead.

### Step 10: Alt+F

Force replacements stays checked only for this run, unless Use configuration is checked too, so you cannot replace results by surprise next time.

Screen reader:

- Force replacements, check box, not checked

### Step 11: Alt+O

A good habit when the sources change: convert into a fresh output folder rather than replacing. The old results stay as they were, and you can compare the two.

Screen reader:

- Output directory: edit, C colon backslash Reports

### Step 12

Force replacements is also a switch on the command line, slash force, for a script that rebuilds every result each night. Walk 7 shows the command line.

### Step 13

Whatever you choose, the Results box says what happened to each file, so a run that kept everything never looks like one that did nothing.

Screen reader:

- BatCon results

### Step 14

Kept results are not a failure, and the exit number for a script stays zero when everything was either converted or kept, since nothing went wrong.

Screen reader:

- 0 files converted, 3 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 15

If you want only the files that changed converted, put the changed ones alone in Source files, or convert into a new output folder and compare; BatCon does not judge which source is newer than its result.

### Step 16: Alt+F

Default settings clears Force replacements along with everything else, so it is always safe to start over from there.

Screen reader:

- Force replacements, check box, not checked

### Step 17

What this walk taught: results already there are kept by default, Alt plus F replaces them, and a replacement happens only once its new conversion has worked.

**Something to try:** Convert the same batch twice, once with Force replacements.

## 7 - Use the Command Line

BatCon without its dialog, from a command prompt or a script.

**Before you start:** A command prompt is open.

### Step 1

Everything the dialog does, the command line does too. That suits a script that runs every night, or a person who would rather type than tab.

### Step 2

Type BatCon, the files to convert, then slash to and the format's extension. Here, every PDF in Reports, to Markdown, into a folder of its own.

BatCon "C:\Reports\*.pdf" /to md /output C:\Reports\Markdown

### Step 3

Quotes go around a path with a space in it, and help with a pattern too. Slash output names the output folder; left out, each result goes beside its source.

### Step 4: Enter

Press Enter. Each file's result prints as it is made, and then the summary, and where the log is.

Screen reader:

- Converted Budget dot p d f to Budget dot m d.
- 1 file converted, 0 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 5

The same choices as the check boxes are switches: slash recursive includes subfolders, slash force replaces results already there, slash view opens the output folder, and slash log keeps the session log there.

### Step 6: Enter

To see every format BatCon can make, type BatCon slash list. It prints each format by name, with its extension, and converts nothing.

Screen reader:

- Comma separated table (csv)

BatCon /list

### Step 7: Enter

Slash help prints every switch with a line on what it does.

Screen reader:

- BatCon -- convert many files at once between formats.

BatCon /help

### Step 8

A switch may be typed with a slash, a dash or two dashes, in any case. Slash to, dash to and dash dash to all mean the same thing.

### Step 9

BatCon also ends with a number a script can test. Zero means it all went well, one means there was nothing to do or a switch was unknown, and three means some files failed.

### Step 10: Enter

Slash defaults puts the dialog's settings back to how they were the first time, for the next time you open it.

Screen reader:

- Default settings restored.

BatCon /defaults

### Step 11

Several sources can follow BatCon at once: a file, a folder and a pattern, separated by spaces. Slash source before any one of them means the same as giving it alone.

### Step 12

A batch run from a script writes its log just as the dialog does, in your own BatCon logs folder, one file for each run, named for its date and time.

### Step 13: Enter

Help for the dialog is F1, and help for the command line is slash help; between them, every choice BatCon offers is described where you are.

Screen reader:

- BatCon -- convert many files at once between formats.

BatCon /help

### Step 14: Enter

A script can name the program in full, so it runs whatever folder the command prompt is in: under Program Files, then BatCon, then exec.

Screen reader:

- Comma separated table (csv)

"C:\Program Files\BatCon\exec\BatCon.exe" /list

### Step 15

A command line never opens the dialog, and the dialog's saved settings never change what a command does: each command says everything it needs.

### Step 16

What this walk taught: slash to and slash output to convert, slash list to see every format, slash help for every switch, and the exit number for scripts.

**Something to try:** Convert a folder from the command line, then list the formats.

## 8 - Read the Results

The Results box, a skipped file and a failed one, and the log.

**Before you start:** BatCon has just finished a batch with a mix of files.

### Step 1

Every batch ends with the Results box, which tells you exactly what happened. It opens by itself when the batch is done.

Screen reader:

- BatCon results

### Step 2

The first line is the summary, four counts in one sentence: converted, kept as they were, skipped for want of a conversion, and failed.

Screen reader:

- 5 files converted, 1 kept as they were, 2 skipped for want of a conversion, 1 failed.

### Step 3: Down Arrow

Skipped for want of a conversion means some source could not become the format you chose. BatCon says so once for each type of file, with the reason. Down Arrow reads it.

Screen reader:

- Skipping 2 files of type wav: the Homer kit has no conversion from wav to xlsx.

### Step 4

That is not an error. A folder often holds files of several kinds, and BatCon converts the ones it can and tells you plainly about the rest.

### Step 5: Down Arrow

Next come the files themselves, one line each, in order. A converted file says what it became.

Screen reader:

- Converted Budget dot p d f to Budget dot m d.

### Step 6: Down Arrow

A failed file says why. Here, a PDF of pictures with no text in it, read when no letter recognition was available.

Screen reader:

- Failed Notes dot p d f: the conversion made an empty file

### Step 7

A failure never touches anything else. The other files still converted, and any earlier result of the failed one is still as it was.

### Step 8: Control+End

The last line names the session log, with every detail of the run. Control plus End goes straight to it.

Screen reader:

- The log: C colon backslash Users backslash you backslash AppData backslash Local backslash BatCon backslash logs backslash BatCon dash 20261020 dot log

### Step 9

Every run writes a new log, named for its date and time, so one batch never overwrites another's record.

### Step 10: Escape

Escape closes the Results box.

The reader says nothing as the window closes.

### Step 11: Control+Home

The Results box is a plain text window: Up and Down Arrow read it a line at a time, Control plus Home returns to the summary, and Control plus End goes to the log.

Screen reader:

- 5 files converted, 1 kept as they were, 2 skipped for want of a conversion, 1 failed.

### Step 12

A kept file is one whose result was already there; walk 6 shows how to replace it with Force replacements.

### Step 13: Control+End

If a batch surprises you, the log is the first place to look. It lists every file, every way BatCon tried, and how long each took, so even a failure has a clear story.

Screen reader:

- The log: C colon backslash Users backslash you backslash AppData backslash Local backslash BatCon backslash logs backslash BatCon dash 20261020 dot log

### Step 14: Alt+L

To keep the log with the converted files, check Log session before the batch, Alt plus L, Lima. BatCon dot log then lands in the output folder beside them.

Screen reader:

- Log session, check box, checked

### Step 15

A skipped type can often be reached another way. A sound file cannot become a workbook, but it can become a transcript as a Markdown document or a Word document, which may be what you wanted all along.

### Step 16

What this walk taught: the summary first, a reason for each skipped type and each failure, and the log named at the end.

**Something to try:** After a batch, read its results and open its log.

## 9 - Conclusion

What to carry away, and where to begin.

**Before you start:** BatCon is installed and your reader is running.

### Step 1: Alt+Control+B

Four things to carry away. One: Alt plus Control plus B opens BatCon.

Screen reader:

- BatCon dialog
- Source files: edit

### Step 2: Alt+T

Two: every format BatCon can make is listed, and a source that cannot become it is skipped, with the reason.

Screen reader:

- Target format: list box
- Markdown document (md)

### Step 3

Three: your files are safe. A result appears only when its conversion works, and a source is never overwritten.

Screen reader:

- 0 files converted, 3 kept as they were, 0 skipped for want of a conversion, 0 failed.

### Step 4: Control+End

Four: every run writes a log, named at the end of the results.

Screen reader:

- The log: C colon backslash Users backslash you backslash AppData backslash Local backslash BatCon backslash logs backslash BatCon dash 20261020 dot log

### Step 5

The guide and the hotkeys are in the help folder and on the project page. BatCon is one of the Homer Tools.

### Step 6

What this walk taught: the four things, and where help lives.

**Something to try:** Begin with a folder you convert often.

<!-- walkthrough ends -->

