using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day06
{
    class Solution : Solver
    {
        public string Name => "Custom Customs";

        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
            => Regex.Split(input, "\r?\n\r?\n").Select(group => group.SplitLine().SelectMany(l => l).ToHashSet().Count).Sum();

        int PartTwo(string input) => 0;
    }
}