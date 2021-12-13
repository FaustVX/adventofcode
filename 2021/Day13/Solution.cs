using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day13;

[ProblemName("Transparent Origami")]
class Solution : Solver
{
    private static (HashSet<(int x, int y)> paper, Queue<(char dir, int value)> folds) Parse(string input)
    {
        var (dotsS, (foldsS, _)) = input.Split2Lines();
        var paper = dotsS.SplitLine()
            .Select(static line => line.Split(','))
            .Select(static pos => (int.Parse(pos[0]), int.Parse(pos[1])))
            .ToHashSet();

        var folds = foldsS.SplitLine()
            .Select(static line => line.Split('='))
            .Select(static fold => (fold[0][^1], int.Parse(fold[1])))
            .ToQueue();

        return (paper, folds);
    }

    public object PartOne(string input)
        => Solve(input).First().Length;

    public object PartTwo(string input)
    {
        var code = Solve(input).Last();
        Console.Clear();
        var maxY = 0;
        foreach (var (x, y) in code)
        {
            Console.SetCursorPosition(x, y);
            Console.Write('#');
            if (y > maxY)
                maxY = y;
        }
        Console.SetCursorPosition(0, maxY + 1);
        Console.Write("What is the code ? ");
        return Console.ReadLine();
    }

    private static IEnumerable<(int x, int y)[]> Solve(string input)
    {
        var (dots, folds) = Parse(input);
        var dotsArray = dots.ToArray();
        foreach (var (dir, value) in folds)
        {
            switch (dir)
            {
                case 'x':
                    foreach (var dot in dotsArray)
                        if (dot.x > value)
                        {
                            var offset = dot.x - value;
                            dots.Remove(dot);
                            dots.Add(dot with { x = value - offset });
                        }
                    break;
                case 'y':
                    foreach (var dot in dotsArray)
                        if (dot.y > value)
                        {
                            var offset = dot.y - value;
                            dots.Remove(dot);
                            dots.Add(dot with { y = value - offset });
                        }
                    break;
            }
            yield return dotsArray = dots.ToArray();;
        }
    }
}
