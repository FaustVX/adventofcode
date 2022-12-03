namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]
class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine()
            .Select(FindCommonItem)
            .Select(GetItemPriority)
            .Sum();

    public object PartTwo(string input)
    {
        return 0;
    }

    private static char FindCommonItem(string sack)
    {
        var length = sack.Length / 2;
        foreach (var item in sack.AsSpan(0, length))
            if (sack.AsSpan(length).Contains(item))
                return item;
        throw new();
    }

    private static int GetItemPriority(char item)
        => item is >= 'a' ? item - 'a' + 1 : item - 'A' + 27;
}
