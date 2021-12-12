using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Y2021.Day11;

[ProblemName("Dumbo Octopus")]
class Solution : Solver
{
    private struct Octopus
    {
        private byte _energy;

        public void IncreaseEnergy()
            => _energy++;

        public void Reset()
            => _energy = 0;

        public bool DoesFlash
            => _energy == 10;

        public static Octopus Parse(char value)
            => new() { _energy = (byte)(value - '0') };
    }

    private static (Octopus[,] octopuses, int width, int height) Parse(string input)
        => input.Parse2D(Octopus.Parse);

    public object PartOne(string input)
    {
        var (octopi, width, height) = Parse(input);
        var sum = 0L;
        for (var step = 0; step < 100; step++)
        {
            var flashed = new HashSet<(int x, int y)>();
            var propagate = new Queue<(int x, int y)>();

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    Propagate(octopi, (x, y), propagate);

            while (propagate.Count != 0)
            {
                var pos = propagate.Dequeue();
                flashed.Add(pos);
                foreach (var (x, y) in Neighbours(octopi, pos.x, pos.y))
                    Propagate(octopi, (x, y), propagate);
            }

            foreach (var (x, y) in flashed)
            {
                sum++;
                octopi[x, y].Reset();
            }
        }
        return sum;

        static void Propagate(Octopus[,] octopi, (int x, int y) pos, Queue<(int x, int y)> propagate)
        {
            octopi[pos.x, pos.y].IncreaseEnergy();
            if (octopi[pos.x, pos.y].DoesFlash)
                propagate.Enqueue(pos);
        }

        static IEnumerable<(int x, int y)> Neighbours(Octopus[,] octopi, int x, int y)
        {
            var (w, h) = (octopi.GetLength(0), octopi.GetLength(1));
            var neighbours = new (int x, int y)[]
            {
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, -1),
                (0, +1),
                (+1, -1),
                (+1, 0),
                (+1, +1),
            };

            foreach (var pos in neighbours)
            {
                var (i, j) = (pos.x + x, pos.y + y);
                if (i >= 0 && i < w && j >= 0 && j < h)
                    yield return (i, j);
            }
        }
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
