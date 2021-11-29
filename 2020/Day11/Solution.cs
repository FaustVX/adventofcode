using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace AdventOfCode.Y2020.Day11;

[ProblemName("Seating System")]
class Solution : Solver
{
    private readonly string _location;

    public Solution()
    {
        _location = Path.Combine(this.WorkingDir(), "input");
    }

    enum Seat : byte
    {
        Floor = (byte)'.',
        Empty = (byte)'L',
        Occupied = (byte)'#',
    }

    public object PartOne(string input)
    {
        var location = _location + ".1";
        if (Directory.Exists(location))
            Directory.Delete(location, recursive: true);
        Directory.CreateDirectory(location);

        var inputs = input.SplitLine();
        var (w, h) = (inputs[0].Length, inputs.Length);
        var grid = new Seat[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                grid[x, y] = (Seat)inputs[y][x];

        for (int i = 0; ; i++)
        {
            using var file = File.CreateText(Path.Combine(location, i + ".txt"));
            grid = NextGeneration(grid, out var hasChanged, 1);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                    file.Write((char)grid[x, y]);
                file.WriteLine();
            }

            if (!hasChanged)
                return grid.Cast<Seat>().Count(s => s is Seat.Occupied);
        }
    }

    public object PartTwo(string input)
    {
        var location = _location + ".2";
        if (Directory.Exists(location))
            Directory.Delete(location, recursive: true);
        Directory.CreateDirectory(location);

        var inputs = input.SplitLine();
        var (w, h) = (inputs[0].Length, inputs.Length);
        var grid = new Seat[w, h];
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                grid[x, y] = (Seat)inputs[y][x];

        for (int i = 0; ; i++)
        {
            using var file = File.CreateText(Path.Combine(location, i + ".txt"));
            grid = NextGeneration(grid, out var hasChanged, 2);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                    file.Write((char)grid[x, y]);
                file.WriteLine();
            }

            if (!hasChanged)
                return grid.Cast<Seat>().Count(s => s is Seat.Occupied);
        }
    }

    static Seat[,] NextGeneration(Seat[,] seats, out bool hasChanged, int part)
    {
        var dirs = new (int x, int y)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        var (w, h) = (seats.GetLength(0), seats.GetLength(1));

        var newGen = new Seat[w, h];
        hasChanged = false;

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                newGen[x, y] = part is 1 ? Change1(x, y, seats[x, y], ref hasChanged) : Change2(x, y, seats[x, y], ref hasChanged);

        return newGen;

        Seat Change1(int x, int y, Seat seat, ref bool hasChanged)
        {
            if (seat is Seat.Floor)
                return seat;

            var occupied = dirs.Select(dir => (x: dir.x + x, y: dir.y + y))
                                .Where(pos => pos.x >= 0 && pos.x < w)
                                .Where(pos => pos.y >= 0 && pos.y < h)
                                .Select(pos => seats[pos.x, pos.y])
                                .Count(s => s is Seat.Occupied);

            var result = (seat, occupied) switch
            {
                (Seat.Empty, 0) => Seat.Occupied,
                (Seat.Occupied, >= 4) => Seat.Empty,
                _ => seat,
            };

            hasChanged |= seat != result;
            return result;
        }

        Seat Change2(int x, int y, Seat seat, ref bool hasChanged)
        {
            if (seat is Seat.Floor)
                return seat;

            var occupied = dirs
                                .Select(dir => GetOffsets(dir, (x, y)).Select(pos => seats[pos.x, pos.y]).FirstOrDefault(pos => pos is not Seat.Floor))
                                .Count(s => s is Seat.Occupied);

            var result = (seat, occupied) switch
            {
                (Seat.Empty, 0) => Seat.Occupied,
                (Seat.Occupied, >= 5) => Seat.Empty,
                _ => seat,
            };

            hasChanged |= seat != result;
            return result;

            IEnumerable<(int x, int y)> GetOffsets((int x, int y) dir, (int x, int y) pos)
            {
                pos = (pos.x + dir.x, pos.y + dir.y);
                if (pos.x < 0 || pos.x >= w || pos.y < 0 || pos.y >= h)
                    return Enumerable.Empty<(int, int)>();
                return GetOffsets(dir, pos).Prepend(pos);
            }
        }
    }
}
