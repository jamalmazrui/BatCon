# BatCon History

## 1.0.0, 19 October 2026

- **The first release.** BatCon converts a batch of files between any two formats the Homer kit converts, from a dialog or a command line.
- **The formats offered are the ones every source converts to,** by the kit's readable names.
- **Output on a command line goes where it is sent.** BatCon writes to the console it was started from only when its output is not already redirected, so a script, or the kit's check, reads what /help and /list print.
- **The first BatCon's leftovers are retired by the build:** its copy of the kit, its third-party folder, its extra documents and scripts, and the PDF libraries it had put in exec, each moved into notes\retired\firstBatCon and logged.
- **Results are written safely:** to a temporary file first, put in place only when the conversion works, never over a source.
