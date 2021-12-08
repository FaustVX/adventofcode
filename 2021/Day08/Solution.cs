using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]
class Solution : Solver
{
    private class Entry
    {
        public char[][] Patterns { get; init; }
        public char[][] Values { get; init; }
        public static Entry Parse(string input)
        {
            var (first, (end, _)) = input.Split(" | ");
            return new()
            {
                Patterns = first.Split(' ').ParseToArrayOfT(Enumerable.ToArray),
                Values = end.Split(' ').ParseToArrayOfT(Enumerable.ToArray),
            };
        }
    }

    private static IEnumerable<Entry> Parse(string input)
        => input.ParseToIEnumOfT(Entry.Parse);

    public object PartOne(string input)
        => Parse(input).Sum(static entry => entry.Values.Count(static value => value.Length is 2 or 4 or 3 or 7));

    public object PartTwo(string input)
    {
        return 0;
    }
}
