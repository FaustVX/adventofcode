#nullable enable
namespace AdventOfCode.Y2022.Day14;

public sealed class Cave
{
    private Cave()
    { }

    private bool[,] _cave { get; init; } = default!;
    public int OffsetX { get; private init; } = default!;
    public (int x, int y) Size { get; private init; } = default!;
    public bool this[int x, int y]
    {
        get => _cave[x - OffsetX, y];
        set => _cave[x - OffsetX, y] = value;
    }
    public bool this[(int x, int y) pos]
    {
        get => this[pos.x, pos.y];
        set => this[pos.x, pos.y] = value;
    }

    public bool DropSand()
    {
        ReadOnlySpan<(int x, int y)> dirs = stackalloc (int x, int y)[]
        {
            (0, 1),
            (-1, 1),
            (1, 1),
        };
        var sand = (x: 500, y: 0);
        while (true)
        {
            if (this[sand])
                return false;
            var hasFallen = false;
            foreach (var dir in dirs)
            {
                var newPos = (x: sand.x + dir.x, y: sand.y + dir.y);
                if (newPos.x - OffsetX < 0 || newPos.x - OffsetX >= Size.x || newPos.y >= Size.y)
                    return false;
                if (!this[newPos])
                {
                    sand = newPos;
                    hasFallen = true;
                    break;
                }
            }
            if (!hasFallen)
            {
                this[sand] = true;
                return true;
            }
        }
    }

    private void PlaceWalls(ReadOnlyMemory<(int x, int y)>[] pathes, bool addFLoor)
    {
        foreach (var path in pathes)
        {
            var previous = path.Span[0];
            foreach (var current in path.Span[1..])
            {
                if (previous.x == current.x)
                    for (var (y, max) = (Math.Min(previous.y, current.y), Math.Max(previous.y, current.y)); y <= max; y++)
                        this[current.x, y] = true;
                else if (previous.y == current.y)
                    for (var (x, max) = (Math.Min(previous.x, current.x), Math.Max(previous.x, current.x)); x <= max; x++)
                        this[x, current.y] = true;
                previous = current;
            }
        }
        var (minX, maxX, maxY) = (OffsetX, Size.x - 1 + OffsetX, Size.y - 1);
        if (addFLoor)
            for (int x = minX; x <= maxX; x++)
                this[x, maxY] = true;
    }

    public static Cave Parse(ReadOnlyMemory<ReadOnlyMemory<char>> input, bool addFloor)
    {
        var pathes = ParsePathes(input);
        var (minX, maxX, maxY) = GetMinMax(pathes, addFloor, (500, 0));
        var (width, height) = (maxX - minX + 1, maxY + 1);
        var c = new Cave()
        {
            _cave = new bool[width, height],
            OffsetX = minX,
            Size = (width, height),
        };

        c.PlaceWalls(pathes, addFloor);
        return c;
    }

    private static ReadOnlyMemory<(int x, int y)>[] ParsePathes(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        var pathes = new ReadOnlyMemory<(int x, int y)>[input.Length];
        for (int i = 0; i < input.Length; i++)
            pathes[i] = ParsePath(input.Span[i].Split(" -> "));
        return pathes;
    }

    private static ReadOnlyMemory<(int x, int y)> ParsePath(ReadOnlyMemory<ReadOnlyMemory<char>> line)
    {
        var path = new (int x, int y)[line.Length];
        for (var i = 0; i < line.Length; i++)
            path[i] = ParsePoint(line.Span[i]);
        return path;
    }

    private static (int x, int y) ParsePoint(ReadOnlyMemory<char> input)
    => input.Split(",").Span switch
    {
        [var x, var y] when int.TryParse(x.Span, out var a) && int.TryParse(y.Span, out var b) => (a, b),
        _ => throw new UnreachableException(),
    };

    private static (int minX, int maxX, int maxY) GetMinMax(ReadOnlyMemory<(int x, int y)>[] pathes, bool addFloor, (int x, int y) sandDropPos)
    {
        var (minX, maxX, maxY) = (sandDropPos.x, sandDropPos.x, sandDropPos.y);
        foreach (var path in pathes)
            foreach (var (x, y) in path.Span)
            {
                if (x < minX)
                    minX = x;
                else if (x > maxX)
                    maxX = x;
                if (y > maxY)
                    maxY = y;
            }
        if (addFloor)
        {
            maxY += 2;
            minX = Math.Min(minX, sandDropPos.x - 1 - maxY);
            maxX = Math.Max(maxX, sandDropPos.x + 1 + maxY);
        }
        return (minX, maxX, maxY);
    }
}
