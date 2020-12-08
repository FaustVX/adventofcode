using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day08
{
    class Solution : Solver
    {
        public string Name => "Handheld Halting";

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

        int PartTwo(string input) => 0;

        Processor GenerateProcessor(string input)
            => new Processor(input.SplitLine().Select(OpCode.Parse).ToArray());
    }
}