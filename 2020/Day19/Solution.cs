using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day19
{
    abstract record Expr();
    sealed record Value(string Text) : Expr;
    sealed record Reference(int Ref) : Expr;
    sealed record Concat(Expr Left, Expr Right) : Expr;
    sealed record Or(Expr Left, Expr Right) : Expr;

    [ProblemName("Monster Messages")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            var rule0 = ParseExpressions(input.Split2Lines()[0].SplitLine());
            return input.Split2Lines()[1].SplitLine().Count(i => TryParse(i, rule0, out var l) && l == i.Length);
        }

        int PartTwo(string input)
        {
            return 0;
        }

        bool TryParse(ReadOnlySpan<char> input, Expr expr, out int length)
        {
            length = default;
            switch (expr)
            {
                case Value (var v) when input.StartsWith(v):
                    length = v.Length;
                    return true;
                case Concat (var l, var r):
                    return (TryParse(input, l, out var len1) && TryParse(input[len1..], r, out var len2)) && (length = len1 + len2) is not 0;
                case Or (var l, var r):
                    return TryParse(input, l, out length) || TryParse(input, r, out length);
            }
            return false;
        }

        Expr ParseExpressions(string[] lines)
        {
            var rules = lines.Select(line => line.Extract<(int, string)>(@"(\d+):\s(.*)")).OrderBy(l => l.Item1).Select(l => l.Item2).Select(rule => Parse(rule)).ToList();
            return PostProcess(rules[0], rules);

            static Expr Parse(ReadOnlySpan<char> rule)
            {
                if (int.TryParse(rule, out var i))
                    return new Reference(i);
                if (rule.Contains('"'))
                    return new Value(rule[1..^1].ToString());
                if (rule.IndexOf('|') is var @or and > 0)
                    return new Or(Parse(rule[..(or-1)]), Parse(rule[(or+2)..]));
                if (rule.IndexOf(' ') is var concat and > 0)
                    return new Concat(Parse(rule[..concat]), Parse(rule[(concat+1)..]));
                throw new();
            }

            static Expr PostProcess(Expr rule, List<Expr> rules)
                => rule switch
                {
                    Reference(var r) => PostProcess(rules[r], rules),
                    Concat(var l, var r) => new Concat(PostProcess(l, rules), PostProcess(r, rules)),
                    Or(var l, var r) => new Or(PostProcess(l, rules), PostProcess(r, rules)),
                    Expr v => v,
                };
        }
    }
}