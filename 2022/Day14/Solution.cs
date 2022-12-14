#nullable enable
namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    => Execute(input, addFloor: false);

    public object PartTwo(string input)
    => Execute(input, addFloor: true);

    private static object Execute(string input, bool addFloor)
    {
        var cave = Cave.Parse(input.AsMemory().SplitLine(), addFloor);
        var step = 0;
        while (cave.DropSand())
            step++;
        return step;
    }
}
