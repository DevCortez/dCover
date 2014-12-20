dCover is a delphi coverage tool that is aimed at automated tests

Main features (implemented):

-Smart code coverage (Covers code paths instead of every available line)

-Process watcher (Automatically attaches to a process when its on the list of covered files, only halfway implemented)

-Partial coverage (Allows to select specific units/routines to be covered, halfway implemented)

-Simultaneous coverage (Can cover multiple modules at once even in the same process)


Planned features:

-Sub-module watcher (Automatically attaches to processes containing modules in coverage list)

-Code coverage statistics (Different kinds of report about coverage results)

-Export results in various formats

-Heuristic code validation (Read module memory and check if coverage points are valid)

-Source control integration (Allows reference to repositories instead of folders to find source codes)

-Execution history (Records the flow of execution to help creating reliable test cases)
