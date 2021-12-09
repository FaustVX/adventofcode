using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2021.Day09;

[ProblemName("Smoke Basin")]
class Solution : Solver
{
    private static (byte[,] datas, int width, int height) Parse(string input)
    {
        var lines = input.SplitLine();
        int width = lines[0].Length;
        int height = lines.Length;
        var datas = new byte[width, height];

        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                datas[x, y] = byte.Parse(lines[y][x].ToString());

        return (datas, width, height);
    }

    public object PartOne(string input)
    {
        var (datas, width, height) = Parse(input);

        var sum = 0L;
        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var isLowest = true;
                foreach (var neighbour in datas.GetNeighbours(x, y))
                    if (neighbour <= datas[x, y])
                    {
                        isLowest = false;
                        break;
                    }
                if (isLowest)
                    sum += datas[x, y] + 1;
            }
        return sum;
    }

    public object PartTwo(string input)
    {
        return 0;
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
