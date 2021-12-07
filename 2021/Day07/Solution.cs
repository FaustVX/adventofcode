using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day07;

[ProblemName("The Treachery of Whales")]
class Solution : Solver
{
    private static int[] Parse(string input)
        => input.Split(',').Select(int.Parse).ToArray();

    public object PartOne(string input)
    {
        var crabs = Parse(input);
        return crabs.Min(pos => crabs.Aggregate(0L, (acc, crab) => acc + Math.Abs(crab - pos)));
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
