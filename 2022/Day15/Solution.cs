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
        var cave = Cave.Parse(input.AsMemory());

        var (minX, maxX, minY, maxY) = (int.MaxValue, 0, int.MaxValue, 0);
        foreach (var (x, y) in cave.Sensors.Keys)
        {
            if (x < minX)
                minX = x;
            else if (x > maxX)
                maxX = x;
            if (y < minY)
                minY = y;
            else if (y > maxY)
                maxY = y;
        }

        if (minX < 0)
            minX = 0;
        if (maxX > 4_000_000)
            maxX = 4_000_000;
        if (minY < 0)
            minY = 0;
        if (maxY > 4_000_000)
            maxY = 4_000_000;

        foreach (var sensor in cave.GetLimitsOfRange())
            foreach (var (x, y) in sensor)
                if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                    if (!cave.IsInRange(x, y))
                        return x * 4_000_000L + y;

        throw new UnreachableException();
    }
}
