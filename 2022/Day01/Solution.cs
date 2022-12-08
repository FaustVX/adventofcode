namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
public class Solution : Solver
{
    public object PartOne(string input)
        => GetFirsts(1, input);

    public object PartTwo(string input)
        => GetFirsts(3, input);

    private static int GetFirsts(int count, string input)
    {
        Span<int> firsts = stackalloc int[count];
        Parse(firsts, input);
        return CalculateSum(firsts);

        static void Parse(Span<int> firsts, string input)
        {
            var current = 0;
            var count = firsts.Length;
            foreach (var line in (input.TrimEnd() + "\n\n").SplitLine())
                if (line == "")
                {
                    for (int i = 0; i < count; i++)
                        if (current > firsts[i])
                            (current, firsts[i]) = (firsts[i], current);
                    current = 0;
                }
                else
                    current += int.Parse(line);
        }

        static int CalculateSum(Span<int> firsts)
        {
            var sum = 0;
            foreach (var first in firsts)
                sum += first;
            return sum;
        }
    }
}
