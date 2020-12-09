using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day08
{
    [ProblemName("Handheld Halting")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            var proc = GenerateProcessor(input);
            proc.Run();
            return proc.Accumulator;
        }

        int PartTwo(string input)
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

        Processor GenerateProcessor(string input)
            => new Processor(input.SplitLine().Select(OpCode.Parse).ToArray());
    }
}