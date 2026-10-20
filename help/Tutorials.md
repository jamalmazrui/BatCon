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

### Step 4

Three screen reader keys help whenever you are unsure. Insert plus Tab says what has the focus.

### Step 5

Insert plus T says the title of the window.

### Step 6

And Insert plus Up Arrow says again the line you are on.

### Step 7

What this walk taught: what BatCon is for, and the order of the walks.

**Something to try:** Pick the walk that matches what you want to do today.

## 1 - User Interface

The one dialog, control by control, and the keys that reach each one.

**Before you start:** BatCon is installed and your reader is running.

### Step 1: Alt+Control+B

BatCon is one dialog. Alt plus Control plus B, B for BatCon, opens it from anywhere in Windows.

Screen reader:

- BatCon dialog
- Source files: edit

### Step 2

Focus starts in Source files, where you name what to convert: files, folders, or patterns such as star dot P D F.

### Step 3: Alt+T

Every control has an Alt key: the first letter of its name. Alt plus T, Tango, goes to Target format.

Screen reader:

- Target format: list box
- Markdown (md)

### Step 4

The list shows only the formats every source converts to, each by name, with its extension.

### Step 5: Alt+O

Alt plus O, Oscar, is Output directory. Left blank, each result goes beside its source.

Screen reader:

- Output directory: edit, blank

### Step 6: Alt+V

Three check boxes follow: Include subfolders, Alt plus I; Force replacements, Alt plus F; View output, Alt plus V.

Screen reader:

- View output, check box, checked

### Step 7

Enter, or Control plus Enter from anywhere, presses OK and starts the batch. Escape cancels.

### Step 8

What this walk taught: Alt plus Control plus B to open BatCon, and Alt with each control's first letter.

**Something to try:** Open BatCon and visit each control with its Alt key.

## 2 - Install and Launch

Installing BatCon, and opening it the two ways.

**Before you start:** A web browser, and your reader running.

### Step 1

BatCon installs from its page on GitHub: the file named BatCon underscore setup dot E X E.

### Step 2

The installer asks a few questions, each with Enter for the usual answer, and ends with a list of what it did.

### Step 3: Alt+Control+B

Open BatCon with Alt plus Control plus B, or from the Start menu.

Screen reader:

- BatCon dialog
- Source files: edit

### Step 4

Every choice you make is saved as soon as you make it, so BatCon opens next time as you left it.

### Step 5

What this walk taught: where to get BatCon, and Alt plus Control plus B.

**Something to try:** Install BatCon, then open it with its desktop key.

## 3 - Convert PDFs to Markdown

A folder of PDF files, each made a Markdown file you can read line by line.

**Before you start:** BatCon is open, and a folder holds a few PDF files.

### Step 1: Alt+S

In Source files, type the folder and a pattern, here C colon backslash Reports backslash star dot P D F.

Screen reader:

- Source files: edit

### Step 2: Tab

Tab away, and BatCon finds the files and lists what they convert to.

Screen reader:

- Browse source... button

### Step 3: Alt+T

Alt plus T, Tango, for Target format, and Markdown is already first.

Screen reader:

- Target format: list box
- Markdown (md)

### Step 4: Enter

Enter starts the batch. BatCon says how many files, then names each as it goes.

Screen reader:

- Converting 3 files to md
- 2 of 3: Budget.pdf

### Step 5

At the end, a window lists each file. Scanned pages are read with Tesseract, when it is installed.

Screen reader:

- 3 files converted, 0 skipped, 0 failed.

### Step 6

What this walk taught: a pattern in Source files, Markdown in Target format, and Enter.

**Something to try:** Convert a folder of your own PDFs.

## 4 - Turn a Table into a Workbook

A table in CSV, inix or Markdown made an Excel workbook.

**Before you start:** BatCon is open, and a CSV file is at hand.

### Step 1: Alt+B

Press Alt plus B, Bravo, to browse for the file.

Screen reader:

- Browse source dialog
- File name: edit combo

### Step 2: Enter

Choose the file and press Enter. BatCon puts it in Source files and refreshes the list.

Screen reader:

- Source files: edit, quote C colon backslash Data backslash Members dot csv quote

### Step 3: Alt+T

Alt plus T, then the letter E for Excel, moves to it.

Screen reader:

- Excel workbook (xlsx)

### Step 4

Every value goes into the workbook as text, so leading zeros stay, and nothing turns into a formula.

### Step 5: Enter

Enter converts it.

Screen reader:

- 1 file converted, 0 skipped, 0 failed.

### Step 6

What this walk taught: Alt plus B to browse, and a table made a workbook.

**Something to try:** Make a workbook from a table of your own.

## 5 - Choose Where Results Go

Results in a folder of their own, opened when the batch ends.

**Before you start:** BatCon is open with some source files named.

### Step 1: Alt+O

Alt plus O, Oscar, is Output directory. Blank means beside each source.

Screen reader:

- Output directory: edit, blank

### Step 2: Alt+C

Alt plus C, Charlie, chooses a folder with the standard Browse for Folder dialog.

Screen reader:

- Choose the output folder dialog

### Step 3

Or type a folder that does not exist yet; BatCon makes it.

### Step 4: Alt+V

Alt plus V, Victor, checks View output, which opens the folder at the end.

Screen reader:

- View output, check box, checked

### Step 5

Two sources with one name never clash: the second becomes name underscore 2.

### Step 6

What this walk taught: Alt plus O and Alt plus C for the output, and Alt plus V to view it.

**Something to try:** Send a batch to a new folder and let View output open it.

## 6 - Convert Again

Running a batch a second time: what is skipped, and how to replace it.

**Before you start:** BatCon is open, and a batch has been converted once.

### Step 1: Enter

Run the same batch again, and BatCon keeps the results already there.

Screen reader:

- 0 files converted, 3 skipped, 0 failed.

### Step 2: Alt+F

To replace them, press Alt plus F, Foxtrot, for Force replacements.

Screen reader:

- Force replacements, check box, checked

### Step 3: Enter

Even then, an earlier result is replaced only once its new conversion works, so a failure never loses it.

Screen reader:

- 3 files converted, 0 skipped, 0 failed.

### Step 4

What this walk taught: results are kept by default, and Alt plus F replaces them safely.

**Something to try:** Convert the same batch twice, once with Force replacements.

## 7 - Use the Command Line

BatCon without its dialog, from a command prompt or a script.

**Before you start:** A command prompt is open.

### Step 1

Type BatCon, the files, slash to and a format. Here, every PDF in Reports to Markdown.

BatCon "C:\Reports\*.pdf" /to md /output C:\Reports\Markdown

### Step 2

Each file's result prints as it is made, then the summary and where the log is.

Screen reader:

- Converted Budget.pdf to Budget.md.

### Step 3

Slash list shows the formats without converting anything, and slash help shows every switch.

### Step 4

A switch may be typed with a slash, a dash or two dashes, in any case.

### Step 5

What this walk taught: slash to, slash output, slash list and slash help.

**Something to try:** Convert a folder from the command line.

## 8 - Read the Results

The results window, the log, and what a failure says.

**Before you start:** BatCon has just finished a batch.

### Step 1

When a batch ends, a window lists every file: converted, skipped or failed.

Screen reader:

- BatCon results

### Step 2: Down Arrow

The first line is the summary. Down Arrow reads each file in turn.

Screen reader:

- Failed Notes.pdf: the conversion made an empty file

### Step 3: Control+End

A failure says why. The last line names the log, which has every detail, one file for each run.

Screen reader:

- The log: C:\Users\you\AppData\Local\BatCon\logs\BatCon-20261019-120000.log

### Step 4: Escape

Escape closes the window.

### Step 5

What this walk taught: the summary first, a reason for each failure, and the log at the end.

**Something to try:** After a batch, read its results and open its log.

## 9 - Conclusion

What to carry away, and where to begin.

**Before you start:** BatCon is installed and your reader is running.

### Step 1

Four things to carry away. One: Alt plus Control plus B opens BatCon.

### Step 2

Two: the formats listed are the ones every source converts to.

### Step 3

Three: your files are safe. A result appears only when its conversion works, and a source is never overwritten.

### Step 4

Four: every run writes a log, named at the end of the results.

### Step 5

The guide and the hotkeys are in the help folder and on the project page. BatCon is one of the Homer Tools.

### Step 6

What this walk taught: the four things, and where help lives.

**Something to try:** Begin with a folder you convert often.

<!-- walkthrough ends -->

