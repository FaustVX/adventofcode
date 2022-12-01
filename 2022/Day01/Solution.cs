namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
class Solution : Solver
{

    public object PartOne(string input)
        => input.Split2Lines()
            .Select(static elf => elf.Split().Select(int.Parse).Sum())
            .Max();

    public object PartTwo(string input)
        => input.Split2Lines()
            .Select(static elf => elf.Split().Select(int.Parse).Sum())
            .OrderDescending()
            .Take(3)
            .Sum();
}
