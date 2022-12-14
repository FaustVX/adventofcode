#nullable enable
namespace AdventOfCode.Y2022.Day14;

public sealed class Cave
{
    private bool[,] _cave { get; init; } = default!;
    public (int x, int y) Offset { get; private init; } = default!;
    public (int x, int y) Size { get; private init; } = default!;
    public bool this[int x, int y]
    {
        get => _cave[x - Offset.x, y - Offset.y];
        set => _cave[x - Offset.x, y - Offset.y] = value;
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
                if (newPos.x - Offset.x < 0 || newPos.x - Offset.x >= Size.x || newPos.y - Offset.y >= Size.y)
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

    public static Cave Parse(ReadOnlyMemory<ReadOnlyMemory<char>> input, bool isPart2)
    {
        var pathes = new ReadOnlyMemory<(int x, int y)>[input.Length];
        for (int i = 0; i < input.Length; i++)
            pathes[i] = ParsePath(input.Span[i].Split(" -> "));

        var (minX, maxX, minY, maxY) = (500, 500, 0, 0);
        foreach (var path in pathes)
            foreach (var (x, y) in path.Span)
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
        if (isPart2)
        {
            maxY += 2;
            minX = Math.Min(minX, 500 - maxY);
            maxX = Math.Max(maxX, 501 + maxY);
        }
        var (width, height) = (maxX - minX + 1, maxY - minY + 1);
        var c = new Cave()
        {
            _cave = new bool[width, height],
            Offset = (minX, minY),
            Size = (width, height),
        };

        foreach (var path in pathes)
        {
            var previous = path.Span[0];
            foreach (var current in path.Span[1..])
            {
                if (previous.x == current.x)
                    for (var (y, max) = (Math.Min(previous.y, current.y), Math.Max(previous.y, current.y)); y <= max; y++)
                        c[current.x, y] = true;
                else if (previous.y == current.y)
                    for (var (x, max) = (Math.Min(previous.x, current.x), Math.Max(previous.x, current.x)); x <= max; x++)
                        c[x, current.y] = true;
                previous = current;
            }
        }
        if (isPart2)
            for (int x = minX; x <= maxX; x++)
                c[x, maxY] = true;
        return c;
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
}
