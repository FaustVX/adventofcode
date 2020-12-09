using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day03
{
    [ProblemName("Toboggan Trajectory")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            var terrain = input.SplitLine().Select(line => line.Select(c => c is '.').ToArray()).ToArray();
            yield return CalcutateTrees(1, 3, terrain);
            yield return new[]{ (1, 1), (1, 3), (1, 5), (1, 7), (2, 1) }.Select(t => CalcutateTrees(t.Item1, t.Item2, terrain)).Aggregate(1L, (a, c) => a * c);
        }

        int CalcutateTrees(int down, int right, bool[][] terrain)
        {
            var x = 0;
            var trees = 0;
            for (int y = 0; y < terrain.Length; y += down)
            {
                if(!terrain[y][x % terrain[y].Length])
                    trees++;
                x += right;
            }
            return trees;
        }
    }
}