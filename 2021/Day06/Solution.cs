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
            .Select(int.Parse)
            .ToArray();

    public object PartOne(string input)
        => Solve(input, 80);

    public object PartTwo(string input)
        => Solve(input, 256);

    private static ulong Solve(string input, int days)
    {
        // fishes: TKey => Timer; TValue => Number of fishes
        var fishes = Parse(input)
            .GroupBy(static i => i)
            .ToDictionary(static group => group.Key, static group => group.LongCount());

        Sanitize(fishes, 8, 0);

        for (var day = 0; day < days; day++)
        {
            fishes = Enumerable.Range(0, 9).ToDictionary(static i => i, i => i switch
            {
                6 => fishes[0] + fishes[7],
                8 => fishes[0],
                _ => fishes[i + 1],
            });
        }

        return fishes.Values.Aggregate(0UL, static (acc, i) => acc + (ulong)i);

        static void Sanitize<TValue>(Dictionary<int, TValue> dictionary, int maxValue, TValue defaultValue)
        {
            for (int i = 0; i <= maxValue; i++)
                if (!dictionary.ContainsKey(i))
                    dictionary[i] = defaultValue;
        }
    }
}
