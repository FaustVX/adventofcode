using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace AdventOfCode.Y2020.Day11
{
    [ProblemName("Seating System")]
    class Solution : Solver
    {
        enum Seat : byte
        {
            Floor = (byte)'.',
            Empty = (byte)'L',
            Occupied = (byte)'#',
        }

        public IEnumerable<object> Solve(string input, string? location)
        {
            yield return PartOne(input, location!);
            yield return PartTwo(input);
        }

        int PartOne(string input, string location)
        {
            location += ".1";
            if(Directory.Exists(location))
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
                grid = NextGeneration(grid, out var hasChanged);

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

        int PartTwo(string input)
        {
            return 0;
        }

        Seat[,] NextGeneration(Seat[,] seats, out bool hasChanged)
        {
            var dirs = new (int x, int y)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
            var (w, h) = (seats.GetLength(0), seats.GetLength(1));

            var newGen = new Seat[w, h];
            hasChanged = false;

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    newGen[x, y] = Change(x, y, seats[x, y], ref hasChanged);

            return newGen;

            Seat Change(int x, int y, Seat seat, ref bool hasChanged)
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
        }
    }
}