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
    private struct Node
    {
        public byte Risk { readonly get; init; }
        public bool IsVisited { readonly get; set; } = false;
        public ulong Distance { readonly get; set; } = ulong.MaxValue;
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

    private static ulong Solve((Node[,] risks, int width, int height) input)
    {
        return Dijkstra(input.risks, input.width, input.height);

        static ulong Dijkstra(Node[,] risks, int width, int height)
        {
            ReadOnlySpan<(int x, int y)> offsets = stackalloc[]
            {
                (+1, 0),
                (0, +1),
                (-1, 0),
                (0, -1),
            };
            risks[0, 0] = risks[0, 0] with { Distance = 0, IsVisited = true };
            var visitedNodes = width * height - 1;
            while (visitedNodes > 0)
            {
                ref var s1 = ref GetMin(risks, width, height, out var pos);
                s1.IsVisited = true;
                visitedNodes--;
                foreach (var (x, y) in offsets)
                    if (pos.x + x >= 0 && pos.x + x < width && pos.y + y >= 0 && pos.y + y < height)
                        UpdateWeight(in s1, ref risks[pos.x + x, pos.y + y]);
            }
            return risks[width - 1, height - 1].Distance;
        }

        static ref Node GetMin(Node[,] graph, int width, int height, out (int x, int y) pos)
        {
            var min = ulong.MaxValue;
            pos = (0, 0);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (!graph[x, y].IsVisited && graph[x, y].Distance < min)
                        (pos, min) = ((x, y), graph[x, y].Distance);
            return ref graph[pos.x, pos.y];
        }

        static void UpdateWeight(in Node from, ref Node to)
        {
            if (to.Distance > checked(from.Distance + to.Risk))
                to.Distance = checked(from.Distance + to.Risk);
        }
    }
}
