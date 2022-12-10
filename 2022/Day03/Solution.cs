namespace AdventOfCode.Y2022.Day03;

#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
[ProblemName("Rucksack Reorganization")]
public class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine()
            .Select(FindCommonItem)
            .Select(GetItemPriority)
            .Sum();

    public object PartTwo(string input)
        => input.SplitLine()
            .Chunk(3)
            .Select(GetBadgeItem)
            .Select(GetItemPriority)
            .Sum();

    private static char GetBadgeItem(string[] items)
    {
        foreach (var item in items[0])
            if (items[1].Contains(item) && items[2].Contains(item))
                return item;
        throw new("No badge found");
    }

    private static char FindCommonItem(string sack)
    {
        var length = sack.Length / 2;
        foreach (var item in sack.AsSpan(0, length))
            if (sack.AsSpan(length).Contains(item))
                return item;
        throw new("No item in common between the 2 compartments");
    }

    private static int GetItemPriority(char item)
        => item is >= 'a' ? item - 'a' + 1 : item - 'A' + 27;
}
