namespace AdventOfCode.Y2021.Day14;

#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
[ProblemName("Extended Polymerization")]
public class Solution : Solver
{
    private static (string template, IReadOnlyDictionary<(char, char), char> rules) Parse(string input)
    {
        var (template, (rules, _)) = input.Split2Lines();
        return (template, rules.SplitLine()
            .Select(static line => line.Split(" -> "))
            .Select(static line => ((line[0][0], line[0][1]), (line[1][0])))
            .ToDictionary(static line => line.Item1, static line => line.Item2));
    }

    public object PartOne(string input)
        => Solve(input, 10);

    public object PartTwo(string input)
        => Solve(input, 40);

    private static ulong Solve(string input, int step)
    {
        var ((c0, template), rules) = Parse(input);
        var count = new DefaultableDictionary<char, ulong>();
        var backup = new Dictionary<(char, char, int step), IReadOnlyDictionary<char, ulong>>();
        count[c0]++;
        for (int i = 0; i < template.Length; i++)
        {
            var c1 = template[i];
            count = Merge(count, Compute(c0, c1, rules, step, backup));
            c0 = c1;
            count[c1]++;
        }
        return count.Values.Max() - count.Values.Min();

        static IReadOnlyDictionary<char, ulong> Compute(char left, char right, IReadOnlyDictionary<(char, char), char> rules, int step, Dictionary<(char, char, int step), IReadOnlyDictionary<char, ulong>> backup)
        {
            if (step <= 0)
                return new DefaultableDictionary<char, ulong>();
            if (backup.TryGetValue((left, right, step), out var dict))
                return dict;
            var middle = rules[(left, right)];
            var count = Merge(Compute(left, middle, rules, step - 1, backup), Compute(middle, right, rules, step - 1, backup));
            count[middle]++;
            return backup[(left, right, step)] = count;
        }

        static DefaultableDictionary<char, ulong> Merge(IReadOnlyDictionary<char, ulong> left, IReadOnlyDictionary<char, ulong> right)
        {
            var count = new DefaultableDictionary<char, ulong>(left);
            foreach (var (key, value) in right)
                count[key] += value;
            return count;
        }
    }
}
