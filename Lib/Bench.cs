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
        _instance = new();
        _input = Runner.GetNormalizedInput(Path.Combine("..", "..", "..", "..", "..", "..", "..", _instance.WorkingDir(), "input.in")).AsMemory();
    }

    [Benchmark]
    public object PartOne()
    => _instance.PartOne(_input);

    [Benchmark]
    public object PartTwo()
    => _instance.PartTwo(_input);
}
