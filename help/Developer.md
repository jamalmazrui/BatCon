# BatCon for Developers

BatCon is a Homer launchpad app: one dialog, one job, one result. It was made with newHomerApp from the HomerDev templates and builds against the installed kit, never a copy of it.

## The sources

- **BatCon.cs:** the program: the dialog, the command line, the settings and the batch loop. It starts the kit's log, Paths and Elevate first in Main.
- **Batch.cs:** the batch work: finding the source files, the formats they all convert to, a safe name for each result, and one conversion at a time.

## What the kit does

Every format question goes to the kit's Conversion class: hasAnyRoute says whether a file converts at all, targetLabelsFor lists what it converts to with readable names, and convert does the work, the one conversion technique every Homer app uses, through the routes in exec\conversions.inix. BatCon never reads that file itself, so it offers exactly what the kit does, and gains each new route the day the kit does.

The build compiles these kit modules: Conversion, Elevate, Inix, KeyName, Lbc, Log, Paths, Say, Table, Util and Web. It copies conversions.inix into exec, and the installer ships it.

## History of the design

The first BatCon was written by ChatGPT from the kit's documents alone. It worked, but it carried a copy of the kit, read conversions.inix with its own code, and lacked tutorials and Elevate. This version keeps its sound batch logic and file safety, and builds the rest the Homer way; what that first version taught the kit is in HomerDev's History for 1.80.0 and 1.80.1.

## Next

PDF to Markdown goes through the kit's usual Python route here. Compiling the kit's PdfReader, as HomerScribe does, would add its tagged-PDF and layout reading; that needs PdfPig and its binding redirects, the next step for BatCon.

## Building

Unzip BatCon.zip into C:\BatCon and run build_release.bat there; the build finds the kit wherever it is and stops if it is older than the kitNeeded in build.cmd.
