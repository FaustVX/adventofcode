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
            var splitted = input.SplitLine();
            var timeStamp = int.Parse(splitted[0]);
            var lists = splitted[1].Extract<List<string>>(@"(?:(\d+|x),?)+")!
                .Select(s => (ok: int.TryParse(s, out var bus), bus))
                .Where(t => t.ok)
                .Select(t => t.bus)
                .Select(bus => (bus, mod: -(timeStamp % bus) + bus))
                .OrderBy(t => t.mod)
                .Select(t => t.bus * t.mod)
                .First();
            return lists;
        }

        int PartTwo(string input)
        {
            return 0;
        }
    }
}