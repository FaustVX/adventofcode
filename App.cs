using System.Reflection;
using AdventOfCode;
using Cocona;

CoconaLiteApp.Run<Commands>(args);

class Commands
{
    private static readonly Type[] _tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

    public void Update(DayParameters day)
    {

    }

    public void Run(DayParameters day)
    {

    }

    public void Upload(DayParameters day)
    {

    }

    public void Display(DayParameters day)
    {

    }

    public void Benchmark(DayParameters day)
    {

    }
}

record class DayParameters([Argument]string date) : ICommandParameterSet
{
    public static DateTime Today { get; } = DateTime.UtcNow.AddHours(-5);
    public static DateTime StartDateThisYear { get; } = new(Today.Year, 12, 1);
    public static DateTime LastValidDate { get; } = Today >= StartDateThisYear ? Today : new(Today.Year - 1, 12, 25);

    public int Year { get; } = ParseYear(date);
    public int Day { get; } = ParseDay(date);

    private static int ParseYear(string day)
    {
        if (day is "today")
            return Today.Year;
        return int.Parse(day.AsSpan(0, 4));
    }

    private static int ParseDay(string day)
    {
        if (day is "today")
            return Today.Day;
        if (day.Contains("day", StringComparison.InvariantCultureIgnoreCase))
            return int.Parse(day.AsSpan(8));
        return int.Parse(day.AsSpan(5));
    }
}

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
