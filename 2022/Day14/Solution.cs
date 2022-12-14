#nullable enable
namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var cave = Cave.Parse(input.AsMemory().SplitLine(), isPart2: false);
        var step = 0;
        while (cave.DropSand())
            step++;
        return step;
    }

    public object PartTwo(string input)
    {
        var cave = Cave.Parse(input.AsMemory().SplitLine(), isPart2: true);
        var step = 0;
        while (cave.DropSand())
            step++;
        return step;
    }
}
