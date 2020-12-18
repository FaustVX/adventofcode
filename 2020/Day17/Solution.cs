using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day17
{
    [ProblemName("Conway Cubes")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            var dimension = GenerateDimension(input.SplitLine());
            for (int i = 0; i < 6; i++)
                dimension = NextGen(dimension);
            return dimension.Count(kvp => kvp.Value);

            static Dictionary<(int x, int y, int z), bool> GenerateDimension(string[] lines)
            {
                var dim = new Dictionary<(int x, int y, int z), bool>();
                for (int y = 0; y < lines.Length; y++)
                    for (int x = 0; x < lines[y].Length; x++)
                        dim[(x, y, 0)] = lines[y][x] is '#';
                return dim;//.Where(kvp => kvp.Value).ToDictionary(pos => pos.Key, _ => true);
            }

            static Dictionary<(int x, int y, int z), bool> NextGen(Dictionary<(int x, int y, int z), bool> dim)
            {
                dim = dim.Keys.SelectMany(Neighbours).ToHashSet().ToDictionary(pos => pos, pos => dim.TryGetValue(pos, out var alive) && alive);

                var newGen = new Dictionary<(int x, int y, int z), bool>();
                foreach (var kvp in dim)
                {
                    var aliveNeighbours = Neighbours(kvp.Key).Count(n => dim.TryGetValue(n, out var alive) && alive);
                    newGen[kvp.Key] = kvp.Value ? aliveNeighbours is 2 or 3 : aliveNeighbours is 3;
                }
                return newGen;//.Where(kvp => kvp.Value).ToDictionary(pos => pos.Key, _ => true);

                static IEnumerable<(int x, int y, int z)> Neighbours((int x, int y, int z) pos)
                {
                    for (var x = -1; x <= 1; x++)
                        for (var y = -1; y <= 1; y++)
                            for (var z = -1; z <= 1; z++)
                                if ((x, y, z) is not (0, 0, 0))
                                    yield return (x + pos.x, y + pos.y, z + pos.z);
                }
            }
        }

        int PartTwo(string input)
        {
            var dimension = GenerateDimension(input.SplitLine());
            for (int i = 0; i < 6; i++)
                dimension = NextGen(dimension);
            return dimension.Count(kvp => kvp.Value);

            static Dictionary<(int x, int y, int z, int w), bool> GenerateDimension(string[] lines)
            {
                var dim = new Dictionary<(int x, int y, int z, int w), bool>();
                for (int y = 0; y < lines.Length; y++)
                    for (int x = 0; x < lines[y].Length; x++)
                        dim[(x, y, 0, 0)] = lines[y][x] is '#';
                return dim;//.Where(kvp => kvp.Value).ToDictionary(pos => pos.Key, _ => true);
            }

            static Dictionary<(int x, int y, int z, int w), bool> NextGen(Dictionary<(int x, int y, int z, int w), bool> dim)
            {
                dim = dim.Keys.SelectMany(Neighbours).ToHashSet().ToDictionary(pos => pos, pos => dim.TryGetValue(pos, out var alive) && alive);

                var newGen = new Dictionary<(int x, int y, int z, int w), bool>();
                foreach (var kvp in dim)
                {
                    var aliveNeighbours = Neighbours(kvp.Key).Count(n => dim.TryGetValue(n, out var alive) && alive);
                    newGen[kvp.Key] = kvp.Value ? aliveNeighbours is 2 or 3 : aliveNeighbours is 3;
                }
                return newGen;//.Where(kvp => kvp.Value).ToDictionary(pos => pos.Key, _ => true);

                static IEnumerable<(int x, int y, int z, int w)> Neighbours((int x, int y, int z, int w) pos)
                {
                    for (var x = -1; x <= 1; x++)
                        for (var y = -1; y <= 1; y++)
                            for (var z = -1; z <= 1; z++)
                                for (var w = -1; w <= 1; w++)
                                    if ((x, y, z, w) is not (0, 0, 0, 0))
                                        yield return (x + pos.x, y + pos.y, z + pos.z, w + pos.w);
                }
            }
        }
    }
}