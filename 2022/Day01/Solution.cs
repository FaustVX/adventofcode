namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
class Solution : Solver
{

    public object PartOne(string input)
        => input.Split2Lines().Select(static elf => elf.Split().Select(int.Parse).Aggregate(0ul, static (acc, v) => acc + (ulong)v)).Max();

    public object PartTwo(string input)
    {
        return 0;
    }
}
