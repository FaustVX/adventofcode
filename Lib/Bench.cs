using BenchmarkDotNet.Attributes;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
[MemoryDiagnoser]
public class Bench<T>
where T : ISolver, new()
{
    private readonly T _instance;
    private readonly string _input;

    public Bench()
    {
        Globals.CurrentRunMode = Mode.Benchmark;
        _instance = new();
        _input = Runner.GetNormalizedInput(Path.Combine("..", "..", "..", "..", "..", "..", "..", _instance.WorkingDir(), "input.in"));
    }

    [Benchmark]
    public object PartOne()
    => _instance.PartOne(_input);

    [Benchmark]
    public object PartTwo()
    => _instance.PartTwo(_input);
}
