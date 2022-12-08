using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day18;

[ProblemName("Operation Order")]
public class Solution : Solver
{
    public object PartOne(string input)
        => Solve(input, Calculate1);

    public object PartTwo(string input)
        => Solve(input, Calculate2);

    static long Solve(string input, Func<string, long> calculate)
        => input.SplitLine().Select(line =>
        {
            var formula = line;
            while (formula.Contains('('))
            {
                var open = formula.LastIndexOf('(');
                var close = formula.IndexOf(')', open);
                formula = formula[..open] + calculate(formula[(open + 1)..close]) + formula[(close + 1)..];
            }
            return calculate(formula);
        }).Sum();

    static long Calculate1(string formula)
        => int.TryParse(formula, out var i) ? i : formula.Extract<(string, char, int)>(@"\s*(.*)\s(\+|\*)\s(\d+)") switch
        {
            (var other, '+', var n) => Calculate1(other) + n,
            (var other, '*', var n) => Calculate1(other) * n,
        };

    static long Calculate2(string formula)
    {
        if (int.TryParse(formula, out var i))
            return i;
        if (formula.Contains('*'))
            return formula.Extract<(string, string)>(@"\s*(.*?)\s\*\s(.*)") is var (a, b) ? Calculate2(a) * Calculate2(b) : 0;
        return formula.Extract<(string, string)>(@"\s*(.*?)\s\+\s(.*)") is var (c, d) ? Calculate2(c) + Calculate2(d) : 0;
    }
}
