﻿namespace AdventOfCode.Y2020.Day05;

[ProblemName("Binary Boarding")]
class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine().Select(CalculateSeatId).Max();

    public object PartTwo(string input)
    {
        var hashSets = input.SplitLine().Select(CalculateSeatId).ToList();
        return Enumerable.Range(hashSets.Min(), hashSets.Max() - hashSets.Min()).First(bp => !hashSets.Contains(bp));
    }

    int CalculateSeatId(string boardingPass)
        => Convert.ToInt32(string.Concat(boardingPass.Select(l => l is 'B' or 'R' ? '1' : '0')), 2);
}
