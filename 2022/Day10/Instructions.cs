#nullable enable
using System.Collections;

namespace AdventOfCode.Y2022.Day10;

public abstract class Instruction
{
    public abstract IEnumerable Execute(CPU cpu);
    // public override abstract string ToString();
}

public sealed class Noop : Instruction
{
    public override IEnumerable Execute(CPU cpu)
    {
        yield return null;
        yield break;
    }
}

public sealed class AddX : Instruction
{
    public AddX(int value)
    => Value = value;

    public int Value { get; }

    public override IEnumerable Execute(CPU cpu)
    {
        yield return null;
        yield return null;
        cpu.X += Value;
        yield break;
    }
}
