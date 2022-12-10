#nullable enable

namespace AdventOfCode.Y2022.Day10;

public class CPU
{
    public List<Instruction> Program { get; }
    public CPU(ReadOnlyMemory<ReadOnlyMemory<char>> lines)
    {
        Program = new();
        foreach (var line in lines.Span)
            Program.Add(Instruction.Parse(line.SplitSpace()));
    }
    public int X { get; set; } = 1;
    public int Cycle { get; private set; } = 1;

    public IEnumerable<int> Run()
    {
        foreach (var instruction in Program)
            foreach (var _ in instruction.Execute(this))
            {
                yield return Cycle;
                Cycle++;
            }
        yield return Cycle;
    }
}
