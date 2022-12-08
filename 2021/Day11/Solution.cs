using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Y2021.Day11;

[ProblemName("Dumbo Octopus")]
public class Solution : Solver
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

    private static IEnumerable<IEnumerable<Octopus>> Solve(string input)
    {
        var (octopi, width, height) = Parse(input);
        while (true)
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

            yield return flashed.Select(pos => octopi[pos.x, pos.y]);

            foreach (var (x, y) in flashed)
                octopi[x, y].Reset();
        }

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

    public object PartOne(string input)
        => Solve(input).Take(100).Aggregate(0L, static (acc, step) => acc + step.Count());

    public object PartTwo(string input)
        => Solve(input)
            .TakeWhile(static step => step.Count() != 100)
            .Count() + 1;
}
