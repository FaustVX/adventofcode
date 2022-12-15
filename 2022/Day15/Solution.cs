#nullable enable
namespace AdventOfCode.Y2022.Day15;

[ProblemName("Beacon Exclusion Zone")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var cave = Cave.Parse(input.AsMemory());
        return Enumerable.Repeat(cave, cave.Size.x).Where(cave.Size.y <= 100 ? GetForTest : GetForInput).Count();

        static bool GetForTest(Cave c, int x)
        {
            switch (c[x + c.Offset.x, 10])
            {
                case Location.NotBeacon:
                    Console.Write('#');
                    return true;
                case Location.Empty:
                    Console.Write('.');
                    break;
                case Location.Sensor:
                    Console.Write('S');
                    break;
                case Location.Beacon:
                    Console.Write('B');
                    break;
            }
            return false;
        }

        static bool GetForInput(Cave c, int x)
        => c[x + c.Offset.x, 2_000_000] is Location.NotBeacon;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
