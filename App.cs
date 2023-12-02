#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AdventOfCode;
using Cocona;

CoconaLiteApp.Run<Commands>(args);

#if !LIBRARY
[DebuggerStepThrough]
#endif
internal class Commands
{
    private static readonly IReadOnlyList<Type> _tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(static t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
    .OrderBy(static t => t.FullName)
    .ToImmutableList();

    [DoesNotReturn]
    private static void ThrowAoC(AocCommuncationException ex)
    => throw new CommandExitedException(ex.Message, 1);

    [Command]
    public static Task Update(DayParameters day, [Option("no-git")] bool no_git)
    {
        if (!day.IsValid)
            ThrowAoC(AocCommuncationException.WrongDate());
        return no_git ? Updater.Update(day.Year, day.Day) : Updater.UpdateWithGit(day.Year, day.Day);
    }

    [Command]
    public static void Run(DayParameters day)
    {
        if (!day.IsValid)
            ThrowAoC(AocCommuncationException.WrongDate());

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.RunSolver(GetSolver(tsolver));
    }

    [Command]
    public static Task Upload(DayParameters day, [Option("no-git")] bool no_git, [Option("no-benchmark")] bool no_benchmark)
    {
        if (!day.IsValid)
            ThrowAoC(AocCommuncationException.WrongDate());

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        return Updater.Upload(GetSolver(tsolver), !no_git, !no_benchmark);
    }

    [Command]
    public static void Display(DayParameters day)
    {
        if (!day.IsValid)
            ThrowAoC(AocCommuncationException.WrongDate());

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.DisplaySolver(GetDisplay(tsolver));
    }

    [Command]
    public static void Benchmark(DayParameters day)
    {
        if (!day.IsValid)
            ThrowAoC(AocCommuncationException.WrongDate());

        var tsolver = _tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == day.Year &&
            SolverExtensions.Day(tsolver) == day.Day);

        Runner.RunBenchmark(tsolver);
    }

    [Command]
    public static void Init([Option("git-repo", ['g'])] string git_repo, [Option("ssl-salt", ['s'])] string sslSalt, [Option("ssl-password", ['p'])] string? sslPassword, [Option(['u', 'n'])] string username, [Option('y')] int? year)
    {
        year ??= TimeProvider.System.GetLocalNow().Year;
        if (sslPassword is string password)
            new AdventOfCode.Model.Project(git_repo, sslSalt, password, year.Value) { UserName = username }.Init();
        else
            new AdventOfCode.Model.Project(git_repo, sslSalt, "", year.Value) { UserName = username }.Init();
    }

    private static ISolver? GetSolver(Type tsolver)
    => (ISolver?)Activator.CreateInstance(tsolver);

    private static IDisplay? GetDisplay(Type tdisplay)
    => (IDisplay?)Activator.CreateInstance(tdisplay);
}

internal record class DayParameters([Argument] string date = "today") : ICommandParameterSet
{
    public static DateTime Today { get; } = TimeProvider.System.GetUtcNow().AddHours(-5).DateTime;
    public static DateTime StartDateThisYear { get; } = new(Today.Year, 12, 1);
    public static DateTime LastValidDate { get; } = Today >= StartDateThisYear ? Today : new(Today.Year - 1, 12, 25);

    public int Year { get; } = ParseYear(date);
    public int Day { get; } = ParseDay(date);
    public bool IsValid
    => Year <= LastValidDate.Year && Day <= LastValidDate.Day;

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
