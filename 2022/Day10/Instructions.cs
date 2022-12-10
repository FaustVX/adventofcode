#nullable enable

namespace AdventOfCode.Y2022.Day10;

public abstract class Instruction
{
    public static Instruction Parse(ReadOnlyMemory<ReadOnlyMemory<char>> line)
        => line.Span switch
        {
            [{ Span: "noop" }] => Noop.Instance,
            [{ Span: "addx"}, .. var args] => AddX.ParseArgs(args),
            _ => throw new UnreachableException(),
        };
    public abstract IEnumerable<int> Execute(CPU cpu);
    public override abstract string ToString();
}

public sealed class Noop : Instruction
{
    public static Noop Instance { get; } = new();

    private Noop()
    { }

    public override IEnumerable<int> Execute(CPU cpu)
    {
        yield return 1;
    }

    public override string ToString()
    => "noop";
}

public sealed class AddX : Instruction
{
    public required int Value { get; init; }

    internal static AddX ParseArgs(ReadOnlySpan<ReadOnlyMemory<char>> args)
    => args switch
    {
        [var a] when int.TryParse(a.Span, out var x) => new() { Value = x },
        _ => throw new UnreachableException(),
    };

    public override IEnumerable<int> Execute(CPU cpu)
    {
        yield return 2;
        cpu.X += Value;
        yield break;
    }

    public override string ToString()
    => "addx " + Value;
}
