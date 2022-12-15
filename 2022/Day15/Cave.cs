#nullable enable
namespace AdventOfCode.Y2022.Day15;

public enum Location
{
    NotBeacon = -1,
    Empty,
    Sensor,
    Beacon,
}

public sealed class Cave
{
    private Cave(Dictionary<(int x, int y), int> sensors)
    => Sensors = sensors;

    public Dictionary<(int x, int y), int> Sensors { get; }
    public HashSet<(int x, int y)> Beacons { get; } = new();
    public (int x, int y) Offset { get; private init; } = default!;
    public (int x, int y) Size { get; private init; } = default!;
    public Location this[int x, int y]
    {
        get
        {
            if (Beacons.Contains((x, y)))
                return Location.Beacon;
            if (Sensors.ContainsKey((x, y)))
                return Location.Sensor;
            foreach (var (sensor, distance) in Sensors)
            {
                var manhattanDistance = Math.Abs(sensor.x - x) - 1 + Math.Abs(sensor.y - y) - 1;
                if (manhattanDistance < distance - 1)
                    return Location.NotBeacon;
            }
            return Location.Empty;
        }
    }

    public IEnumerable<IEnumerable<(int x, int y)>> GetLimitsOfRange()
    {
        foreach (var (sensor, d) in Sensors)
            yield return GetLimits(sensor, d + 1);

        static IEnumerable<(int x, int y)> GetLimits((int x, int y) sensor, int distance)
        {
            for (int d = 0; d <= distance; d++)
            {
                yield return (d + sensor.x, distance - d + sensor.y);
                yield return (-d + sensor.x, -(distance - d) + sensor.y);
                yield return (distance - d + sensor.x, -d + sensor.y);
                yield return (-(distance - d) + sensor.x, d + sensor.y);
            }
        }
    }

    public bool IsInRange(int x, int y)
    {
        foreach (var (sensor, distance) in Sensors)
        {
            var manhattanDistance = Math.Abs(sensor.x - x) - 1 + Math.Abs(sensor.y - y) - 1;
            if (manhattanDistance < distance - 1)
                return true;
        }
        return false;
    }

    private void PlaceLocations(ReadOnlyMemory<((int x, int y) sensor, (int x, int y) beacon)> pathes)
    {
        foreach (var (sensor, beacon) in pathes.Span)
        {
            var manhattanDistance = Math.Abs(sensor.x - beacon.x) + Math.Abs(sensor.y - beacon.y);
            Sensors.Add(sensor, manhattanDistance);
            Beacons.Add(beacon);
        }
    }

    public static Cave Parse(ReadOnlyMemory<char> input)
    {
        var pathes = ParsePathes(input.SplitLine());
        var (minX, maxX, minY, maxY) = GetMinMax(pathes);
        var (width, height) = (maxX - minX + 1, maxY - minY + 1);
        var c = new Cave(new(capacity: pathes.Length))
        {
            Offset = (minX, minY),
            Size = (width, height),
        };

        c.PlaceLocations(pathes);
        return c;
    }

    private static ReadOnlyMemory<((int x, int y) sensor, (int x, int y) beacon)> ParsePathes(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        var pathes = new ((int x, int y) sensor, (int x, int y) beacon)[input.Length];
        for (int i = 0; i < input.Length; i++)
            pathes[i] = ParsePath(input.Span[i].Split(",|=|:").Span);
        return pathes;
    }

    private static ((int x, int y) sensor, (int x, int y) beacon) ParsePath(ReadOnlySpan<ReadOnlyMemory<char>> line)
    => ((int.Parse(line[1].Span), int.Parse(line[3].Span)), (int.Parse(line[5].Span), int.Parse(line[7].Span)));

    private static (int minX, int maxX, int minY, int maxY) GetMinMax(ReadOnlyMemory<((int x, int y) sensor, (int x, int y) beacon)> pathes)
    {
        var (minX, maxX, minY, maxY) = (int.MaxValue, 0, int.MaxValue, 0);
        foreach (var (sensor, beacon) in pathes.Span)
        {
            var manhattanDistance = Math.Abs(sensor.x - beacon.x) + Math.Abs(sensor.y - beacon.y);
            SetMinMax(sensor.x + manhattanDistance, ref minX, ref maxX);
            SetMinMax(sensor.x - manhattanDistance, ref minX, ref maxX);
            SetMinMax(sensor.y + manhattanDistance, ref minY, ref maxY);
            SetMinMax(sensor.y - manhattanDistance, ref minY, ref maxY);
        }
        return (minX, maxX, minY, maxY);

        static void SetMinMax(int value, ref int min, ref int max)
        {
            if (value < min)
                min = value;
            else if (value > max)
                max = value;
        }
    }
}
