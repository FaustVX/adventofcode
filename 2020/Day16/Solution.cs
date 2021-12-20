using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day16;

[ProblemName("Ticket Translation")]
class Solution : Solver
{
    private static (IEnumerable<(string name, (int min, int max) lower, (int min, int maw) upper)> rules, List<int> ticket, IEnumerable<List<int>> tickets) Parse(string input)
    {
        var splitted = input.Split2Lines();

        return (ParseRules(splitted[0]), ParseTicket(splitted[1].SplitLine()[1]), splitted[2].SplitLine().Skip(1).Select(ParseTicket));

        IEnumerable<(string name, (int min, int max) lower, (int min, int maw) upper)> ParseRules(string lines)
            => lines.SplitLine().Select(l => l.Extract<(string, int, int, int, int)>(@"([a-z\s]+):\s(\d+)-(\d+)\sor\s(\d+)-(\d+)")).Select(i => (i.Item1, (i.Item2, i.Item3), (i.Item4, i.Item5)));

        List<int> ParseTicket(string line)
            => line.Extract<List<int>>(@"(?:(\d+),?)+")!;
    }

    public object PartOne(string input)
    {
        var (rules, _, tickets) = Parse(input);
        return tickets.Sum(t => t.FirstOrDefault(n => !rules.Any(r => IsValid(r, n))));

        static bool IsValid((string, (int min, int max) lower, (int min, int max) upper) rule, int value)
            => (value >= rule.lower.min && value <= rule.lower.max) || (value >= rule.upper.min && value <= rule.upper.max);
    }

    public object PartTwo(string input)
        => null;
}
