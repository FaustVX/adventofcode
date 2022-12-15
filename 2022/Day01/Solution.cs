#nullable enable
namespace AdventOfCode.Y2022.Day01;

[ProblemName("Calorie Counting")]
public class Solution : Solver
{
    public object PartOne(string input) {
        var elfs = input.Split("\n\n");
        int maxCalories = 0;

        foreach (var elf in elfs) {
            var calories = elf.Split("\n").Select(x => int.Parse(x)).Sum();
            maxCalories = Math.Max(calories, maxCalories);
        }

        return maxCalories;
    }

    public object PartTwo(string input) {
        var elfs = input.Split("\n\n");

        var elfsCalories = elfs.Select(elf => elf.Split("\n").Select(x => int.Parse(x)).Sum()).ToList();
        elfsCalories.Sort();
        return elfsCalories.TakeLast(3).Sum();
    }
}
