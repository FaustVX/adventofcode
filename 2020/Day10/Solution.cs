namespace AdventOfCode.Y2020.Day10;

[ProblemName("Adapter Array")]
public class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine().Select(int.Parse).OrderBy(i => i)
            .Aggregate((one: 0, three: 0, last: 0), (a, c) => (c - a.last) switch
            {
                1 => (a.one + 1, a.three, c),
                3 => (a.one, a.three + 1, c),
                _ => (a.one, a.three, c),
            }, a => a.one * (a.three + 1));

    public object PartTwo(string input)
    {
        var dict = new Dictionary<int, long>();
        var inputs = input.SplitLine().Select(int.Parse).OrderBy(i => i);
        return Count(0, inputs.Append(inputs.Max() + 3).ToArray());

        long Count(int value, Memory<int> nexts)
        {
            if (dict.TryGetValue(value, out var sum))
                return sum;

            for (var i = 0; i < nexts.Length; i++)
            {
                var next = nexts.Span[i];
                if (next - value > 3)
                    break;
                if (nexts.Length >= 2)
                    sum += Count(next, nexts[(i + 1)..]);
                else
                    sum++;
            }
            return dict[value] = sum;
        }
    }
}
