using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day06;

[ProblemName("Lanternfish")]
class Solution : Solver
{
    private static int[] Parse(string input)
        => input.Split(',')
            .ParseToArrayOfT(int.Parse);

    public object PartOne(string input)
        => Solve(input, 80);

    public object PartTwo(string input)
        => Solve(input, 256);

    private static ulong Solve(string input, int days)
    {
        var fishes = new ulong[9];

        foreach (var fish in Parse(input))
            fishes[fish]++;

        for (var day = 0; day < days; day++)
            fishes = Enumerable.Range(0, 9)
                .Select(i => i switch
                    {
                        6 => fishes[0] + fishes[7],
                        8 => fishes[0],
                        _ => fishes[i + 1],
                    })
                .ToArray();

        return fishes.Aggregate(0UL, static (acc, i) => acc + i);
    }
}
