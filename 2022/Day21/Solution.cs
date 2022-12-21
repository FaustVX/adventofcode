#nullable enable
namespace AdventOfCode.Y2022.Day21;

[ProblemName("Monkey Math")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    => IMonkey<long>.ParseMonkeys(input.AsMemory().SplitLine())["root"].Value;

    public object PartTwo(string input)
    {
        return 0;
    }
}
