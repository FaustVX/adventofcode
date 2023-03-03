using System.Reflection;
using AdventOfCode;

var tsolvers = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

var action =
    Command(args, Params("update", @"([0-9]+)[/\\](?:Day)?([0-9]+)"), m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return Updater.UpdateWithGit(year, day).Wait;
    }) ??
    Command(args, Params("update", "today"), m => {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 }) {
            return Updater.UpdateWithGit(dt.Year, dt.Day).Wait;
        } else {
            throw AocCommuncationException.WrongDate();
        }
    }) ??
    Command(args, Params("upload", @"([0-9]+)[/\\](?:Day)?([0-9]+)"), m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return () => {
            var tsolver = tsolvers.First(tsolver =>
                SolverExtensions.Year(tsolver) == year &&
                SolverExtensions.Day(tsolver) == day);

            Updater.Upload(GetSolvers(tsolver)[0]).Wait();
        };
    }) ??
    Command(args, Params("upload", "today"), m => {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 }) {

            var tsolver = tsolvers.First(tsolver =>
                SolverExtensions.Year(tsolver) == dt.Year &&
                SolverExtensions.Day(tsolver) == dt.Day);

            return Updater.Upload(GetSolvers(tsolver)[0]).Wait;

        } else {
            throw AocCommuncationException.WrongDate();
        }
    }) ??
    Command(args, Params("display", @"([0-9]+)[/\\](?:Day)?([0-9]+)"), m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return () => {
            var tsolver = tsolvers.FirstOrDefault(tsolver =>
                tsolver.IsAssignableTo(typeof(IDisplay)) &&
                SolverExtensions.Year(tsolver) == year &&
                SolverExtensions.Day(tsolver) == day);

            Runner.DisplayAll(GetDisplays(tsolver));
        };
    }) ??
    Command(args, Params("display", "today"), m => {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 }) {

            var tsolver = tsolvers.First(tsolver =>
                tsolver.IsAssignableTo(typeof(IDisplay)) &&
                SolverExtensions.Year(tsolver) == dt.Year &&
                SolverExtensions.Day(tsolver) == dt.Day);

            return () => Runner.DisplayAll(GetDisplays(tsolver));

        } else {
            throw AocCommuncationException.WrongDate();
        }
    }) ??
    Command(args, Params("bench(mark)?", @"([0-9]+)[/\\](?:Day)?([0-9]+)"), m => {
        var year = int.Parse(m[1]);
        var day = int.Parse(m[2]);
        return () => {
            var tsolver = tsolvers.First(tsolver =>
                SolverExtensions.Year(tsolver) == year &&
                SolverExtensions.Day(tsolver) == day);
                Runner.RunBenchmark(tsolver);
        };
    }) ??
    Command(args, Params("bench(mark)?", "today"), m => {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 }) {

            var tsolver = tsolvers.First(tsolver =>
                SolverExtensions.Year(tsolver) == dt.Year &&
                SolverExtensions.Day(tsolver) == dt.Day);

            return () => Runner.RunBenchmark(tsolver);
        } else {
            throw AocCommuncationException.WrongDate();
        }
    }) ??
    Command(args, Params(@"([0-9]+)[/\\](?:Day)?([0-9]+)"), m => {
        var year = int.Parse(m[0]);
        var day = int.Parse(m[1]);
        var tsolversSelected = tsolvers.First(tsolver =>
            SolverExtensions.Year(tsolver) == year &&
            SolverExtensions.Day(tsolver) == day);
        return () => Runner.RunAll(GetSolvers(tsolversSelected));
    }) ??
    Command(args, Params("[0-9]+"), m => {
        var year = int.Parse(m[0]);
        var tsolversSelected = tsolvers.Where(tsolver =>
            SolverExtensions.Year(tsolver) == year);
        return () => Runner.RunAll(GetSolvers(tsolversSelected.ToArray()));
    }) ??
    Command(args, Params(@"([0-9]+)[/\\](?:Day)?all"), m => {
        var year = int.Parse(m[0]);
        var tsolversSelected = tsolvers.Where(tsolver =>
            SolverExtensions.Year(tsolver) == year);
        return () => Runner.RunAll(GetSolvers(tsolversSelected.ToArray()));
    }) ??
    Command(args, Params("all"), m => {
        return () => Runner.RunAll(GetSolvers(tsolvers));
    }) ??
    Command(args, Params("today"), m => {
        var dt = DateTime.UtcNow.AddHours(-5);
        if (dt is { Month: 12, Day: >= 1 and <= 25 }) {

            var tsolversSelected = tsolvers.First(tsolver =>
                SolverExtensions.Year(tsolver) == dt.Year &&
                SolverExtensions.Day(tsolver) == dt.Day);

            return () =>
                Runner.RunAll(GetSolvers(tsolversSelected));

        } else {
            throw AocCommuncationException.WrongDate();
        }
    }) ??
    Command(args, Params("calendars"), _ => {
        return () => {
            var tsolversSelected = (
                    from tsolver in tsolvers
                    group tsolver by SolverExtensions.Year(tsolver) into g
                    orderby SolverExtensions.Year(g.First()) descending
                    select g.First()
                ).ToArray();

            var solvers = GetSolvers(tsolversSelected);
            foreach (var solver in solvers) {
                solver.SplashScreen().Show();
            }
        };
    }) ??
    Command(args, Params("init", @".*\.git", ".*"), m => {
        return new AdventOfCode.Model.Project(m[1], m[2], "") { UserName = "FaustVX" }.Init;
    }) ??
    Command(args, Params("init", @".*\.git", ".*", ".*"), m => {
        return new AdventOfCode.Model.Project(m[1], m[2], m[3]) { UserName = "FaustVX" }.Init;
    }) ??
    new Action(() => {
        Console.WriteLine(Usage.Get());
    });

try {
    action();
} catch (AggregateException a){
    if (a is { InnerExceptions: [AocCommuncationException { Message: var msg }]}){
        Console.WriteLine(a.InnerException.Message);
    } else {
        throw;
    }
}

Solver[] GetSolvers(params Type[] tsolver) {
    return tsolver.Select(t => Activator.CreateInstance(t) as Solver).ToArray();
}

IDisplay[] GetDisplays(params Type[] tdisplay) {
    return tdisplay.Select(t => Activator.CreateInstance(t) as IDisplay).ToArray();
}

Action Command(string[] args, string[] regexes, Func<string[], Action> parse) {
    if (args.Length != regexes.Length) {
        return null;
    }
    var matches = Enumerable.Zip(args, regexes, (arg, regex) => new Regex("^" + regex + "$").Match(arg));
    if (!matches.All(match => match.Success)) {
        return null;
    }
    try {

        return parse(matches.SelectMany(m =>
                m.Groups.Count > 1 ? m.Groups.Cast<Group>().Skip(1).Select(g => g.Value)
                                   : new[] { m.Value }
            ).ToArray());
    } catch {
        return null;
    }
}

string[] Params(params string[] regex) {
    return regex;
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
