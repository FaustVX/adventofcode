using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day05
{
    class Solution : Solver
    {
        public string Name => "Binary Boarding";

        public IEnumerable<object> Solve(string input)
        {
            yield return input.SplitLine().Select(CalculateSeatId).Max();
            var hashSets = input.SplitLine().Select(CalculateSeatId).ToList();
            yield return Enumerable.Range(hashSets.Min(), hashSets.Max() - hashSets.Min()).First(bp => !hashSets.Contains(bp));
        }

        int CalculateSeatId(string boardingPass)
            => Convert.ToInt32(string.Concat(boardingPass.Select(l => l is 'B' or 'R' ? '1' : '0')), 2);
    }
}