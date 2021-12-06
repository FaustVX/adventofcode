using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day01;

[ProblemName("Sonar Sweep")]
class Solution : Solver
{
    public object PartOne(string input)
        => Solve(input, i => i);

    public object PartTwo(string input)
        => Solve(input, static source => source.Zip(source[1..], source[2..]).Select(t => t.First + t.Second + t.Third));

    private static int Solve(string input, Func<int[], IEnumerable<int>> selector)
    {
        var inputs = input.SplitLine().Select(int.Parse).ToArray();
        var (increasedCount, last) = (-1, int.MinValue);
        foreach (var item in selector(inputs))
            if (item > last)
                increasedCount++;
            else
                last = item;
        return increasedCount;
    }
}
