using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day18
{
    [ProblemName("Operation Order")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        ulong PartOne(string input)
        {
            var sum = 0uL;
            foreach (var line in input.SplitLine())
            {
                var formula = line;
                while (formula.Contains('('))
                {
                    var open = formula.LastIndexOf('(');
                    var close = formula.IndexOf(')', open);
                    formula = formula[..open] + Calculate(formula[(open + 1)..close]) + formula[(close + 1)..];
                }
                sum += (ulong)Calculate(formula);
            }
            return sum;
            static long Calculate(string formula)
                => int.TryParse(formula, out var i) ? i : formula.Extract<(string, char, int)>(@"\s*(.*)\s(\+|\*)\s(\d+)") switch
                    {
                        (var other, '+', var n) => Calculate(other) + n,
                        (var other, '*', var n) => Calculate(other) * n,
                    };
        }

        ulong PartTwo(string input)
        {
            var sum = 0uL;
            foreach (var line in input.SplitLine())
            {
                var formula = line;
                while (formula.Contains('('))
                {
                    var open = formula.LastIndexOf('(');
                    var close = formula.IndexOf(')', open);
                    formula = formula[..open] + Calculate(formula[(open + 1)..close]) + formula[(close + 1)..];
                }
                sum += (ulong)Calculate(formula);
            }
            return sum;
            static long Calculate(string formula)
            {
                if (int.TryParse(formula, out var i))
                    return i;
                if (formula.Contains('*'))
                    return formula.Extract<(string, string)>(@"\s*(.*?)\s\*\s(.*)") is var (a, b) ? Calculate(a) * Calculate(b) : 0;
                return formula.Extract<(string, string)>(@"\s*(.*?)\s\+\s(.*)") is var (c, d) ? Calculate(c) + Calculate(d) : 0;
            }
        }
    }
}