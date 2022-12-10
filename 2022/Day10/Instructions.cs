#nullable enable
using System.Collections;

namespace AdventOfCode.Y2022.Day10;

public abstract class Instruction
{
    public static Instruction Parse(ReadOnlyMemory<ReadOnlyMemory<char>> line)
        => line.Span switch
        {
            [{ Span: "noop" }] => new Noop(),
            [{ Span: "addx"}, .. var args] => AddX.ParseArgs(args),
            _ => throw new UnreachableException(),
        };
    public abstract IEnumerable Execute(CPU cpu);
    public override abstract string ToString();
}

public sealed class Noop : Instruction
{
    public override IEnumerable Execute(CPU cpu)
    {
        yield return null;
        yield break;
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

    public override IEnumerable Execute(CPU cpu)
    {
        yield return null;
        yield return null;
        cpu.X += Value;
        yield break;
    }

    public override string ToString()
    => "addx " + Value;
}
