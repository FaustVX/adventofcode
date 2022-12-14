#nullable enable
namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    => Execute(input, addFloor: false);

    public object PartTwo(string input)
    => Execute(input, addFloor: true);

    private static object Execute(string input, bool addFloor)
    {
        var cave = Cave.Parse(input.AsMemory().SplitLine(), addFloor);
        return cave.DropSand().Count();
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("part 1", static input => Display(input, addFloor: false));
        yield return ("part 2", static input => Display(input, addFloor: true));
    }

    private static void Display(string input, bool addFloor)
    {
        Console.CursorVisible = false;
        var cave = Cave.Parse(input.AsMemory().SplitLine(), addFloor);
        for (int y = 0; y < cave.Size.y; y++)
        {
            for (int x = 0; x < cave.Size.x; x++)
            {
                Console.Write(cave[x + cave.OffsetX, y] ? '#' : ' ');
            }
            Console.WriteLine();
        }
        foreach (var sand in cave.DropSand())
        {
            Console.ReadKey(intercept: true);
            Console.SetCursorPosition(sand.x - cave.OffsetX, sand.y);
            Console.Write('o');
        }
        Console.CursorVisible = true;
    }
}
