using System.Reflection;
using AdventOfCode;
using Cocona;

CoconaLiteApp.Run<Program>(args);

static class Usage {
    public static string Get()
        => """
            Usage: dotnet run [arguments]
            1) To run the solutions and admire your advent calendar:

            [year]/[day|all]                                   Solve the specified problems
            today                                              Shortcut to the above
            [year]                                             Solve the whole year
            all                                                Solve everything

            calendars                                          Show the calendars

            init [this .git repo] [sslSalt] ([sslPassword])    Initialize the current folder

            2) To start working on new problems:
            login to https://adventofcode.com, then copy your session cookie, and export
            it in your console like this

             export SESSION=73a37e9a72a...

            then run the app with

             update [year]/[day]   Prepares a folder for the given day, updates the input,
                                   the readme and creates a solution template.
             update today          Shortcut to the above.

            3) To upload your answer:
            set up your SESSION variable as above.

             upload [year]/[day]   Upload the answer for the selected year and day.
             upload today          Shortcut to the above.

            """;
}
