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
        => c[x + c.Offset.x, 10] is Location.NotBeacon;

        static bool GetForInput(Cave c, int x)
        => c[x + c.Offset.x, 2_000_000] is Location.NotBeacon;
    }

    public object PartTwo(string input)
    {
        var cave = Cave.Parse(input.AsMemory());

        var (minX, maxX, minY, maxY) = cave.Sensors.Keys.GetMinMax();

        0.SetMax(ref minX);
        4_000_000.SetMin(ref maxX);
        0.SetMax(ref minY);
        4_000_000.SetMin(ref maxY);

        foreach (var sensor in cave.GetLimitsOfRange())
            foreach (var (x, y) in sensor)
                if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                    if (!cave.IsInRange(x, y))
                        return x * 4_000_000L + y;

        throw new UnreachableException();
    }
}
