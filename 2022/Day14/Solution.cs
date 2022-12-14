#nullable enable
namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    => Execute(input, isPart2: false);

    public object PartTwo(string input)
    => Execute(input, isPart2: true);

    private static object Execute(string input, bool isPart2)
    {
        var cave = Cave.Parse(input.AsMemory().SplitLine(), isPart2);
        var step = 0;
        while (cave.DropSand())
            step++;
        return step;
    }
}
