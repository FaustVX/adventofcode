using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2021.Day15;

[ProblemName("Chiton")]
class Solution : Solver
{
    private class Node
    {
        public byte Risk { get; init; }
        public int Distance { get; set; } = int.MaxValue;
    }

    private static (Node[,], int width, int height) Parse(string input)
        => input.Parse2D(static risk => new Node { Risk = (byte)(risk - '0') });

    public object PartOne(string input)
        => Solve(Parse(input));

    public object PartTwo(string input)
    {
        return Solve(DuplicateIncrement(Parse(input), 5));

        static (Node[,] array, int width, int height) DuplicateIncrement((Node[,] array, int width, int height) input, int count)
        {
            var (array, width, height) = input;
            var output = new Node[width * count, height * count];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    SetValue(count, array, width, height, output, x, y);
            return (output, width * count, height * count);

            static void SetValue(int count, Node[,] array, int width, int height, Node[,] output, int x, int y)
            {
                for (int i = 0; i < count; i++)
                    for (int j = 0; j < count; j++)
                        output[width * i + x, height * j + y] = new()
                        {
                            Risk = (byte)(((array[x, y].Risk + i + j - 1) % 9) + 1),
                        };
            }
        }
    }

    private static int Solve((Node[,] risks, int width, int height) input)
    {
        Dijkstra((Dictionary<(int x, int y), Node>)input.risks.ToDictionary(), input.width, input.height);
        return input.risks[input.width - 1, input.height - 1].Distance;

        static void Dijkstra(Dictionary<(int x, int y), Node> nodes, int width, int height)
        {
            ReadOnlySpan<(int x, int y)> offsets = stackalloc[]
            {
                (+1, 0),
                (0, +1),
                (-1, 0),
                (0, -1),
            };
            nodes[(0, 0)].Distance = 0;
            while (nodes.Count > 0)
            {
                var s1 = GetMin(nodes, out var pos);
                nodes.Remove(pos);
                foreach (var (x, y) in offsets)
                    if (nodes.ContainsKey((pos.x + x, pos.y + y)))
                        UpdateWeight(s1, nodes[(pos.x + x, pos.y + y)]);
            }
        }

        static Node GetMin(IReadOnlyDictionary<(int, int), Node> nodes, out (int x, int y) pos)
        {
            (pos, var min) = nodes.First();
            foreach (var (position, node) in nodes)
                if (node.Distance < min.Distance)
                    (pos, min) = (position, node);
            return min;
        }

        static void UpdateWeight(Node from, Node to)
        {
            if (to.Distance > from.Distance + to.Risk)
                to.Distance = from.Distance + to.Risk;
        }
    }
}
