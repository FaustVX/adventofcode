#nullable enable

namespace AdventOfCode.Y2022.Day10;

public class CPU
{
    public List<Instruction> Program { get; }
    public CPU(ReadOnlyMemory<ReadOnlyMemory<char>> lines)
    {
        Program = new();
        foreach (var line in lines.Span)
        {
            var instruction = line.SplitSpace().Span switch
            {
                [{ Span: "noop"}] => (Instruction)new Noop(),
                [{ Span: "addx"}, var a] when int.TryParse(a.Span, out var x) => new AddX(x),
                _ => throw new UnreachableException(),
            };
            Program.Add(instruction);
        }
    }
    public int X { get; set; } = 1;
    public int Cycle { get; private set; } = 1;

    public IEnumerable<int> Run()
    {
        foreach (var instruction in Program)
        {
            foreach (var _ in instruction.Execute(this))
            {
                yield return Cycle;
                Cycle++;
            }
        }
        yield return Cycle;
    }
}
