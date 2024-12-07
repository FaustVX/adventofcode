using BenchmarkDotNet.Attributes;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
[MemoryDiagnoser, ShortRunJob]
public class ShortBench<T>
where T : ISolver, new()
{
    private static readonly T _instance;
    private static readonly ReadOnlyMemory<char> _input;

    static ShortBench()
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

#if !LIBRARY
[DebuggerStepThrough]
#endif
[MemoryDiagnoser]
public class LongBench<T>
where T : ISolver, new()
{
    private static readonly T _instance;
    private static readonly ReadOnlyMemory<char> _input;

    static LongBench()
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
