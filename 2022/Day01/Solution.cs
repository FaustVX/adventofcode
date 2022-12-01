namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var max = 0;
        var current = 0;
        foreach (var line in input.SplitLine())
        {
            if (line == "")
            {
                if (current > max)
                    max = current;
                current = 0;
                continue;
            }
            current += int.Parse(line);
        }
        return max;
    }

    public object PartTwo(string input)
    {
        var (max1, max2, max3) = (0, 0, 0);
        var current = 0;
        foreach (var line in input.SplitLine())
        {
            if (line == "")
            {
                if (current > max1)
                    (current, max1) = (max1, current);
                if (current > max2)
                    (current, max2) = (max2, current);
                if (current > max3)
                    max3 = current;
                current = 0;
                continue;
            }
            current += int.Parse(line);
        }
        return max1 + max2 + max3;
    }
}
