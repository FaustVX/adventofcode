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
        var sum = 0;
        var lines = input.SplitLine();
        for (var i = 0; i < lines.Length; i += 3)
            foreach (var item in lines[i])
                if (lines[i+1].Contains(item) && lines[i+2].Contains(item))
                {
                    sum += GetItemPriority(item);
                    break;
                }
        return sum;
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
