using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day06;

#nullable enable
[ProblemName("Lanternfish")]
class Solution : Solver
{
    private class Lanternfish
    {
        private int _timer;

        public Lanternfish()
            : this(6, true)
        { }

        public Lanternfish(int timer)
            : this(timer, false)
        { }

        private Lanternfish(int timer, bool isBaby)
            => (_timer) = (timer + (isBaby ? 2 : 0));

        public Lanternfish? NextDay()
        {
            if (--_timer >= 0)
                return null;
            _timer = 6;
            return new();
        }
    }
    private static List<Lanternfish> Parse(string input)
        => input.Split(',').Select(int.Parse).Select(timer => new Lanternfish(timer)).ToList();

    public object PartOne(string input)
        => Solve(input, 80);

    public object PartTwo(string input)
        => Solve(input, 256);

    private static long Solve(string input, int days)
    {
        var fishes = Parse(input);
        for (int day = 0; day < days; day++)
            foreach (var lanternfish in fishes.ToArray())
                if (lanternfish.NextDay() is {} baby)
                    fishes.Add(baby);
#pragma warning disable CA1829 // Use Length/Count property instead of Enumerable.Count method
        return fishes.LongCount();
#pragma warning restore
    }
}
