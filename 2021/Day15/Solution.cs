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
        public uint Distance { readonly get; set; } = uint.MaxValue;
        public (int x, int y) Parent { readonly get; set; } = (0, 0);
    }

    private static (Node[,], int width, int height) Parse(string input)
        => input.Parse2D(static risk => new Node { Risk = (byte)(risk - '0') });

    public object PartOne(string input)
        => Solve(input);

    public object PartTwo(string input)
    {
        return 0;
    }

    private static uint Solve(string input)
    {
        var (risks, width, height) = Parse(input);

        return Dijkstra(risks, width, height);

        static uint Dijkstra(Node[,] risks, int width, int height)
        {
            ReadOnlySpan<(int x, int y)> offsets = stackalloc[]
            {
                (-1, 0),
                (0, -1),
                (+1, 0),
                (0, +1)
            };
            risks[0, 0] = risks[0, 0] with { Distance = 0, IsVisited = true };
            while (risks.Cast<Node>().Any(static node => !node.IsVisited))
            {
                ref var s1 = ref GetMin(risks, width, height, out var pos);
                s1.IsVisited = true;
                foreach (var (x, y) in offsets)
                    if (pos.x + x >= 0 && pos.x + x < width && pos.y + y >= 0 && pos.y + y < height)
                        UpdateWeight(in s1, ref risks[pos.x + x, pos.y + y], pos);
            }
            return risks[width - 1, height - 1].Distance;
        }

        static ref Node GetMin(Node[,] graph, int width, int height, out (int x, int y) pos)
        {
            var min = uint.MaxValue;
            pos = (0, 0);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (!graph[x, y].IsVisited && graph[x, y].Distance < min)
                        (pos, min) = ((x, y), graph[x, y].Distance);
            return ref graph[pos.x, pos.y];
        }

        static void UpdateWeight(in Node from, ref Node to, (int x, int y) posFrom)
             => to = (to.Distance > from.Distance + to.Risk)
                ? to with { Distance = from.Distance + to.Risk, Parent = posFrom }
                : to;
    }
}
