using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day03
{
    class Solution : Solver
    {
        public string Name => "Toboggan Trajectory";

        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(3, input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(line => line.Select(c => c is '.').ToArray()).ToArray());
            yield return PartTwo(input);
        }

        int PartOne(int right, bool[][] terrain)
        {
            var x = 0;
            var trees = 0;
            for (int y = 0; y < terrain.Length; y++)
            {
                if(!terrain[y][x % terrain[y].Length])
                    trees++;
                x += right;
            }
            return trees;
        }

        int PartTwo(string input) => 0;
    }
}