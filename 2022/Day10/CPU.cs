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
            foreach (var cycle in instruction.Execute(this))
                for (int i = 0; i < cycle; i++)
                    yield return Cycle++;
        yield return Cycle;
    }
}
