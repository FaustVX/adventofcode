#nullable enable
namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var pairs = ParsePairs(input.AsMemory().Split2Lines());
        return pairs
            .Select(static (pair, i) => pair.left.IsOrdered(pair.right) is true ? i + 1 : 0)
            .Sum();
    }

    private List<(List left, List right)> ParsePairs(Memory<ReadOnlyMemory<char>> pairs)
    {
        var packets = new List<(List left, List right)>(capacity: pairs.Length);
        foreach (var pair in pairs.Span)
        {
            var p = pair.SplitLine().Span;
            var left = List.Parse(p[0].Span, out _);
            var right = List.Parse(p[1].Span, out _);
            packets.Add((left, right));
        }
        return packets;
    }

    public object PartTwo(string input)
    {
        input += "\n\n[[2]]\n[[6]]";
        var pairs = ParsePairs(input.AsMemory().Split2Lines());
        var ordered = pairs
            .SelectMany(static pair => new[]{pair.left, pair.right})
            .Order(new List.Comparer())
            .ToList();
        return (ordered.IndexOf(pairs[^1].left) + 1) * (ordered.IndexOf(pairs[^1].right) + 1);
    }
}
