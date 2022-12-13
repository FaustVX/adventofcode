using System.Reflection;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class)]
class ProblemName : Attribute
{
    public readonly string Name;
    public ProblemName(string name)
    {
        Name = name;
    }
}

public interface Solver
{
    object PartOne(string input);
    object PartTwo(string input) => null;
}

interface IDisplay
{
    public abstract IEnumerable<(string name, Action<string> action)> GetDisplays();
}

static class SolverExtensions
{

    public static IEnumerable<object> Solve(this Solver solver, string input)
    {
        yield return solver.PartOne(input);
        var res = solver.PartTwo(input);
        if (res != null) {
            yield return res;
        }
    }

    public static string GetName(this Solver solver)
    {
        return (
            solver
                .GetType()
                .GetCustomAttribute(typeof(ProblemName)) as ProblemName
        ).Name;
    }

    public static string DayName(this Solver solver)
    => $"Day {solver.Day()}";

    public static int Year(this Solver solver)
    => Year(solver.GetType());

    public static int Year(Type t)
    => int.Parse(t.FullName.Split('.')[1][1..]);

    public static int Day(this Solver solver)
    => Day(solver.GetType());

    public static int Day(Type t)
    => int.Parse(t.FullName.Split('.')[2][3..]);

    public static string WorkingDir(int year)
    => Path.Combine(year.ToString());

    public static string WorkingDir(int year, int day)
    => Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));

    public static string WorkingDir(this Solver solver)
    => WorkingDir(solver.Year(), solver.Day());

    public static string WorkingDir(Type solver)
    => WorkingDir(Year(solver), Day(solver));

    public static SplashScreen SplashScreen(this Solver solver)
    {
        var tsplashScreen = Assembly.GetEntryAssembly().GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && typeof(SplashScreen).IsAssignableFrom(t))
             .Single(t => Year(t) == solver.Year());
        return (SplashScreen)Activator.CreateInstance(tsplashScreen);
    }
}

record SolverResult(string[] answers, string[] errors);

class Runner
{

    public static string GetNormalizedInput(string file)
    {
        var input = File.ReadAllText(file);
        if (input.EndsWith("\r\n"))
            return input[0..^2];
        if (input.EndsWith("\n"))
            return input[0..^1];
        return input;
    }

    public static void RunBenchmark(Type solver)
    {
        BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(Bench<>).MakeGenericType(solver));
        File.Copy("BenchmarkDotNet.Artifacts/results/Bench_Solution_-report-github.md", Path.Combine(SolverExtensions.WorkingDir(solver), "benchmark.md"), overwrite: true);
    }

    public static SolverResult RunSolver(Solver solver)
    {

        var workingDir = solver.WorkingDir();
        var indent = "    ";
        Write(ConsoleColor.White, $"{indent}{solver.DayName()}: {solver.GetName()}");
        WriteLine();
        var solverResult = default(SolverResult);
        foreach (var dir in new[] { Path.Combine(workingDir, "test"), workingDir })
        {
            if (!Directory.Exists(dir))
                continue;
            var searchOption = dir.EndsWith("test")
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
            foreach (var file in Directory.EnumerateFiles(dir, "*.in", searchOption).OrderBy(file => file))
                try
                {
                    Console.WriteLine("  " + file + ":");
                    var refoutFile = file.Replace(".in", ".refout");
                    var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                    var input = GetNormalizedInput(file);
                    var iline = 0;
                    var answers = new List<string>();
                    var errors = new List<string>();
                    var stopwatch = Stopwatch.StartNew();
                    foreach (var line in solver.Solve(input))
                    {
                        var ticks = stopwatch.ElapsedTicks;
                        answers.Add(line.ToString());
                        var (statusColor, status, err) =
                            refout == null || refout.Length <= iline ? (ConsoleColor.Cyan, "?", null) :
                            refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                            (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

                        if (err is not null)
                            errors.Add(err);

                        Write(statusColor, $"{indent}  {status}");
                        Console.Write($" {line} ");
                        var diff = ticks * 1000.0 / Stopwatch.Frequency;

                        WriteLine(
                            diff > 1000 ? ConsoleColor.Red :
                            diff > 500 ? ConsoleColor.Yellow :
                            ConsoleColor.DarkGreen,
                            $"({diff:F3} ms)"
                        );
                        iline++;
                        stopwatch.Restart();
                    }
                    solverResult = new SolverResult(answers.ToArray(), errors.ToArray());
                }
                catch (Exception ex)
                {
                    WriteLine(ConsoleColor.DarkRed, ex.ToString());
                }
        }
        return solverResult ?? throw new Exception();
    }

    public static void DisplaySolver(IDisplay display)
    {
        var files = GetInputs((Solver)display).ToArray();
        var displays = display.GetDisplays().ToArray();
        var fileSelected = 0;
        var displaySelected = 0;

        while (true)
        {
            Console.Clear();
            for (int f = 0; f < files.Length; f++)
            {
                string file = files[f];
                WriteSelected(file, fileSelected, f, false);
            }
            Console.WriteLine();
            for (int d = 0; d < displays.Length; d++)
            {
                var (name, action) = displays[d];
                WriteSelected(name, displaySelected, d, true);
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.UpArrow:
                    Decrement(ref displaySelected);
                    break;
                case ConsoleKey.DownArrow:
                    Increment(ref displaySelected, displays.Length);
                    break;
                case ConsoleKey.LeftArrow:
                    Decrement(ref fileSelected);
                    break;
                case ConsoleKey.RightArrow:
                    Increment(ref fileSelected, files.Length);
                    break;
                case ConsoleKey.Escape:
                    return;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    Console.Clear();
                    displays[displaySelected].action(GetNormalizedInput(files[fileSelected]));
                    Console.ReadLine();
                    break;
            }
        }

        static void WriteSelected(string text, int selected, int i, bool newLine)
        {
            Console.Write((selected == i ? ">" : " ") + text + (selected == i ? "<" : " "));
            if (newLine)
                Console.WriteLine();
            else
                Console.Write(" \t");
        }

        static void Increment(ref int selected, int length)
        {
            if (selected < length - 1)
                selected++;
        }

        static void Decrement(ref int selected)
        {
            if (selected > 0)
                selected--;
        }

        static IEnumerable<string> GetInputs(Solver solver)
        {
            var workingDir = solver?.WorkingDir();
            if (workingDir is null)
                yield break;
            foreach (var dir in new[] { Path.Combine(workingDir, "test"), workingDir })
            {
                if (!Directory.Exists(dir))
                    continue;
                var searchOption = dir.EndsWith("test")
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly;
                foreach (var file in Directory.EnumerateFiles(dir, "*.in", searchOption).OrderBy(file => file))
                    yield return file;
            }
        }
    }

    public static void RunAll(params Solver[] solvers)
    {
        var errors = new List<string>();

        var lastYear = -1;
        foreach (var solver in solvers) {
            if (lastYear != solver.Year()) {
                solver.SplashScreen().Show();
                lastYear = solver.Year();
            }

            var result = RunSolver(solver);
            WriteLine();
            errors.AddRange(result.errors);
        }

        WriteLine();

        if (errors.Any()) {
            WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
        }
    }

    public static void DisplayAll(params IDisplay[] displays)
    {
        Globals.CurrentRunMode = Mode.Display;
        foreach (var solver in displays)
        {
            DisplaySolver(solver);
            WriteLine();
        }

        WriteLine();
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "")
    => Write(color, text + "\n");

    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}
