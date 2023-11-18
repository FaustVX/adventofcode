using System.Reflection;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class)]
sealed partial class ProblemName([Property]string name) : Attribute
{ }

public interface ISolver
{
    object PartOne(string input);
    object PartTwo(string input);
}

interface IDisplay
{
    IEnumerable<(string name, Action<string> action)> GetDisplays();
}

#if !LIBRARY
[DebuggerStepThrough]
#endif
static class SolverExtensions
{

    public static IEnumerable<object> Solve(this ISolver solver, string input)
    {
        yield return solver.PartOne(input);
        var res = solver.PartTwo(input);
        if (res != null)
            yield return res;
    }

    public static string GetName(this ISolver solver)
    {
        return (
            solver
                .GetType()
                .GetCustomAttribute(typeof(ProblemName)) as ProblemName
        ).Name;
    }

    public static string DayName(this ISolver solver)
    => $"Day {solver.Day()}";

    public static int Year(this ISolver solver)
    => Year(solver.GetType());

    public static int Year(Type t)
    => int.Parse(t.FullName.Split('.')[1][1..]);

    public static int Day(this ISolver solver)
    => Day(solver.GetType());

    public static int Day(Type t)
    => int.Parse(t.FullName.Split('.')[2][3..]);

    public static string WorkingDir(int year)
    => Path.Combine(year.ToString());

    public static string WorkingDir(int year, int day)
    => Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));

    public static string WorkingDir(this ISolver solver)
    => WorkingDir(solver.Year(), solver.Day());

    public static string WorkingDir(Type solver)
    => WorkingDir(Year(solver), Day(solver));

    public static ISplashScreen SplashScreen(this ISolver solver)
    {
        var tsplashScreen = Assembly.GetEntryAssembly().GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && typeof(ISplashScreen).IsAssignableFrom(t))
             .Single(t => Year(t) == solver.Year());
        return (ISplashScreen)Activator.CreateInstance(tsplashScreen);
    }
}

#if !LIBRARY
[DebuggerStepThrough]
#endif
record SolverResult(string[] answers, string[] errors);

#if !LIBRARY
[DebuggerStepThrough]
#endif
static class Runner
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
        var psi = new ProcessStartInfo()
        {
            FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs", "Microsoft VS Code", "Code.exe"),
            ArgumentList = { "--reuse-window", "--", $"{SolverExtensions.Year(solver)}/Day{SolverExtensions.Day(solver):00}/benchmark.md" },
            UseShellExecute = false,
        };
        Process.Start(psi);
    }

    public static SolverResult RunSolver(ISolver solver)
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
            (var searchOption, Globals.IsTestInput) = dir.EndsWith("test")
                ? (SearchOption.AllDirectories, true)
                : (SearchOption.TopDirectoryOnly, false);
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
                    var stopwatch = TimeProvider.System.GetTimestamp();
                    foreach (var line in solver.Solve(input))
                    {
                        var ticks = TimeProvider.System.GetElapsedTime(stopwatch);
                        answers.Add(line.ToString());
                        var (statusColor, status, err) =
                            refout == null || refout.Length <= iline || string.IsNullOrWhiteSpace(refout[iline]) ? (ConsoleColor.Cyan, "?", null) :
                            refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                            (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

                        if (err is not null)
                            errors.Add(err);

                        Write(statusColor, $"{indent}  {status}");
                        Console.Write($" {line} ");

                        WriteLine(
                            ticks > TimeSpan.FromSeconds(5) ? ConsoleColor.Red :
                            ticks > TimeSpan.FromSeconds(1) ? ConsoleColor.Yellow :
                            ConsoleColor.DarkGreen,
                            $"({ticks.TotalMilliseconds:F3} ms)"
                        );
                        iline++;
                        stopwatch = TimeProvider.System.GetTimestamp();
                    }
                    solverResult = new SolverResult([.. answers], [.. errors]);
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
        Globals.CurrentRunMode = Mode.Display;
        var files = GetInputs((ISolver)display).ToArray();
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

        static IEnumerable<string> GetInputs(ISolver solver)
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
