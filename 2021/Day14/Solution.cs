using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day14;

[ProblemName("Extended Polymerization")]
class Solution : Solver
{
    private static (string template, Dictionary<(char, char), char> rules) Parse(string input)
    {
        var (template, (rules, _)) = input.Split2Lines();
        return (template, rules.SplitLine()
            .Select(static line => line.Split(" -> "))
            .Select(static line => ((line[0][0], line[0][1]), (line[1][0])))
            .ToDictionary(static line => line.Item1, static line => line.Item2));
    }

    public object PartOne(string input)
    {
        var line = Solve(input).ElementAt(10).GroupBy(static c => c);
        return line.Max(static g => g.Count()) - line.Min(static g => g.Count());
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    private static IEnumerable<string> Solve(string input)
    {
        var (template, rules) = Parse(input);
        var list = new LinkedList<char>(template);
        while (true)
        {
            var output = new LinkedList<char>();
            var c0 = list.First.Value;
            output.AddLast(c0);
            foreach (var c1 in list.Skip(1))
            {
                output.AddLast(rules[(c0, c1)]);
                output.AddLast(c1);
                c0 = c1;
            }
            yield return string.Concat(list);
            list = output;
        }
    }
}
