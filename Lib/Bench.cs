using BenchmarkDotNet.Attributes;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
[MemoryDiagnoser, ShortRunJob]
public class Bench<T>
where T : ISolver, new()
{
    private static readonly T _instance;
    private static readonly ReadOnlyMemory<char> _input;

    static Bench()
    {
        Globals.CurrentRunMode = Mode.Benchmark;
        Globals.InputFileName = "input.in";
        _instance = new();
        var file = Path.Combine("..", "..", "..", "..", "..", "..", "..", _instance.WorkingDir(), "input.in");
        _input = _instance.GetInput(file).AsMemory();
    }

    [Benchmark]
    public object PartOne()
    => _instance.PartOne(_input);

    [Benchmark]
    public object PartTwo()
    => _instance.PartTwo(_input);
}
