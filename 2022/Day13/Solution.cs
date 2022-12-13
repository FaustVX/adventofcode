#nullable enable
namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    {
        var pairs = Parse(input.AsMemory().SplitLine());
        return pairs
            .Chunk(2)
            .Select(static (pair, i) => pair[0].IsOrdered(pair[1]) is true ? i + 1 : 0)
            .Sum();
    }

    private IReadOnlyList<List> Parse(ReadOnlyMemory<ReadOnlyMemory<char>> lines)
    {
        var list = new List<List>();
        foreach (var pair in lines.Span)
            if (!pair.IsEmpty)
                list.Add (List.Parse(pair.Span, out _));
        return list;
    }

    public object PartTwo(string input)
    {
        var divider1 = List.Parse("[[2]]", out _);
        var divider2 = List.Parse("[[6]]", out _);
        var pairs = Parse(input.AsMemory().SplitLine())
            .Append(divider1)
            .Append(divider2);
        var ordered = pairs
            .Order(new List.Comparer())
            .ToList();
        return (ordered.IndexOf(divider1) + 1) * (ordered.IndexOf(divider2) + 1);
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Part 1", Part1);
        yield return ("Part 2", Part2);
    }

    private void Part1(string input)
    {
        var pairs = Parse(input.AsMemory().SplitLine());
        var i = 1;
        foreach (var pair in pairs.Chunk(2))
        {
            Console.WriteLine($"\n== Pair {i++} ==");
            pair[0].IsOrdered(pair[1]);
        }
    }

    private void Part2(string input)
    {
        (var currentMode, Globals.CurrentRunMode) = (Globals.CurrentRunMode, (Mode)(-1));
        var divider1 = List.Parse("[[2]]", out _);
        var divider2 = List.Parse("[[6]]", out _);
        var pairs = Parse(input.AsMemory().SplitLine())
            .Append(divider1)
            .Append(divider2);
        var ordered = pairs
            .Order(new List.Comparer());
        foreach (var packet in ordered)
        {
            Console.WriteLine(packet);
        }
        Globals.CurrentRunMode = currentMode;
    }
}
