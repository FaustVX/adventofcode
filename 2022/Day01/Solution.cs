namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
class Solution : Solver
{

    public object PartOne(string input)
        => GetFirsts(1, input);

    public object PartTwo(string input)
        => GetFirsts(3, input);

    private static int GetFirsts(int count, string input)
    {
        Span<int> firsts = stackalloc int[count];
        var current = 0;
        foreach (var line in (input.TrimEnd() + "\n\n").SplitLine())
        {
            if (line == "")
            {
                for (int i = 0; i < count; i++)
                    if (current > firsts[i])
                        (current, firsts[i]) = (firsts[i], current);
                current = 0;
                continue;
            }
            current += int.Parse(line);
        }
        var sum = 0;
        foreach (var first in firsts)
            sum += first;
        return sum;
    }
}
