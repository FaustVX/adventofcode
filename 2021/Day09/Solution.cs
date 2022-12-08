using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2021.Day09;

[ProblemName("Smoke Basin")]
public class Solution : Solver
{
    private static ((byte height, int? basin)[,] datas, int width, int height) Parse(string input)
        => input.Parse2D(static c => ((byte)(c - '0'), default(int?)));

    public object PartOne(string input)
    {
        var (datas, width, height) = Parse(input);

        var sum = 0L;
        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var isLowest = true;
                foreach (var neighbour in datas.GetNeighbours(x, y))
                    if (neighbour.height <= datas[x, y].height)
                    {
                        isLowest = false;
                        break;
                    }
                if (isLowest)
                    sum += datas[x, y].height + 1;
            }
        return sum;
    }

    public object PartTwo(string input)
    {
        var (datas, width, height) = Parse(input);
        var basinId = 0;
        var basins = new DefaultableDictionary<int, List<(int x, int y)>>(() => new());
        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                ref var data = ref datas[x, y];
                if (data.height is 9)
                    continue;
                var basin = datas.GetNeighbours(x, y).Where(static n => n is (height: not 9, basin: not null)).ToArray() switch
                {
                    { Length: 0 } => basinId++,
                    (head: (height: _, basin: int b), tail: { Length: 0 }) => b,
                    (head: (height: _, basin: int b0), tail: (head: (height: _, basin: int b1), tail: { Length: 0 })) when b0 == b1 => b0,
                    var neighbours => Merge(neighbours, basins, datas),
                };
                data.basin = basin;
                basins[basin].Add((x, y));
            }

        static int Merge((byte height, int? basin)[] array, DefaultableDictionary<int, List<(int x, int y)>> basins, (byte height, int? basin)[,] datas)
        {
            var targetBasin = array[0].basin.GetValueOrDefault();
            var target = basins[targetBasin];
            foreach (var (_, basin) in array.Skip(1))
            {
                var source = basins[basin.GetValueOrDefault()];
                foreach (var (x, y) in source)
                    datas[x, y].basin = targetBasin;
                target.AddRange(source);
                basins.Remove(basin.GetValueOrDefault());
            }
            return targetBasin;
        }

        return basins.Values.Select(static basin => basin.Count)
            .OrderByDescending(static basin => basin)
            .Take(3)
            .Aggregate(1L, static (acc, basin) => acc * basin);
    }
}

static class Extensions
{
    public static T? At<T>(this T[,] array, int x, int y)
        where T : struct
    {
        if (x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1))
            return array[x, y];
        return default;
    }

    public static IEnumerable<T> GetNeighbours<T>(this T[,] datas, int x, int y)
        where T : struct
    {
        var neighbours = new[]
        {
            (-1, 0),
            (+1, 0),
            (0, -1),
            (0, +1),
        };

        return Select(neighbours, datas, x, y);

        static IEnumerable<T> Select(IEnumerable<(int x, int y)> enumerable, T[,] datas, int x, int y)
        {
            foreach (var neighbour in enumerable)
                if (datas.At(neighbour.x + x, neighbour.y + y) is T value)
                    yield return value;
        }
    }
}
