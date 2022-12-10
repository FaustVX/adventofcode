namespace AdventOfCode.Y2020.Day08;

#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
[ProblemName("Handheld Halting")]
public class Solution : Solver
{
    public object PartOne(string input)
    {
        var proc = GenerateProcessor(input);
        proc.Run();
        return proc.Accumulator;
    }

    public object PartTwo(string input)
    {
        return GenerateProcessor(input).Program
            .Select((op, i) => (op, i))
            .Where(t => t.op is OpCode.Jmp or OpCode.Nop)
            .Select(t => (op: t.op is OpCode.Nop { Value: var val } ? (OpCode)new OpCode.Jmp(val) : new OpCode.Nop(0), t.i))
            .Select(t =>
            {
                var proc = GenerateProcessor(input);
                proc.Program[t.i] = t.op;
                return proc;
            })
            .First(proc => proc.Run()).Accumulator;
    }

    static Processor GenerateProcessor(string input)
        => new(input.SplitLine().Select(OpCode.Parse).ToArray());
}
