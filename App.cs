#nullable enable

using System.Reflection;
using AdventOfCode;
using Cocona;

CoconaLiteApp.Run<Commands>(args);

class Commands
{
    private static readonly IReadOnlyList<Type> _tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToImmutableList();

    [Command]
    public Task Update(DayParameters day, [Option("no-git")]bool no_git)
    {
        if (!day.IsValid)
            throw AocCommuncationException.WrongDate();
        return no_git ? Updater.Update(day.Year, day.Day) : Updater.UpdateWithGit(day.Year, day.Year);
    }

    public void Run(DayParameters day)
    {
        if (!day.IsValid)
            throw AocCommuncationException.WrongDate();

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.RunSolver(GetSolver(tsolver));
    }

    public Task Upload(DayParameters day, [Option("no-git")]bool no_git, [Option("no-benchmark")]bool no_benchmark)
    {
        if (!day.IsValid)
            throw AocCommuncationException.WrongDate();

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        return Updater.Upload(GetSolver(tsolver), !no_git, !no_benchmark);
    }

    public void Display(DayParameters day)
    {
        if (!day.IsValid)
            throw AocCommuncationException.WrongDate();

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.DisplaySolver(GetDisplay(tsolver));
    }

    public void Benchmark(DayParameters day)
    {
        if (!day.IsValid)
            throw AocCommuncationException.WrongDate();

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.RunBenchmark(tsolver);
    }

    private static Solver? GetSolver(Type tsolver)
    => (Solver?)Activator.CreateInstance(tsolver);

    private static IDisplay? GetDisplay(Type tdisplay)
    => (IDisplay?)Activator.CreateInstance(tdisplay);
}

record class DayParameters([Argument]string date) : ICommandParameterSet
{
    public static DateTime Today { get; } = DateTime.UtcNow.AddHours(-5);
    public static DateTime StartDateThisYear { get; } = new(Today.Year, 12, 1);
    public static DateTime LastValidDate { get; } = Today >= StartDateThisYear ? Today : new(Today.Year - 1, 12, 25);

    public int Year { get; } = ParseYear(date);
    public int Day { get; } = ParseDay(date);
    public bool IsValid
    => Year >= LastValidDate.Year && Day >= LastValidDate.Day;

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
