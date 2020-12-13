using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day13
{
    [ProblemName("Shuttle Search")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            var (timeStamp, buses) = input.Extract<(int, List<int>)>(@"(\d+)\W+(?:(?:(\d+)|x),?)+")!;
            return buses.Select(bus => (bus, mod: -(timeStamp % bus) + bus))
                .OrderBy(t => t.mod)
                .Select(t => t.bus * t.mod)
                .First();
        }

        int PartTwo(string input)
        {
            return 0;
        }
    }
}