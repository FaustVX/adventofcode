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
    {
        return Solve(input, Slide);

        static IEnumerable<int> Slide(int[] source)
        {
            for (var i = 0; i < source.Length - 2; i++)
                yield return source[i] + source[i + 1] + source[i + 2];
        }
    }

    private static int Solve(string input, Func<int[], IEnumerable<int>> selector)
    {
        var inputs = input.SplitLine().Select(int.Parse).ToArray();
        var (increasedCount, last) = (-1, int.MinValue);
        foreach (var item in selector(inputs))
        {
            if (item > last)
                increasedCount++;
            last = item;
        }
        return increasedCount;
    }
}
