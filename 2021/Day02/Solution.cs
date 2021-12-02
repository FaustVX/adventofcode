using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day02;

[ProblemName("Dive!")]
class Solution : Solver
{
    private enum Direction
    {
        Forward,
        Up,
        Down,
    }
    private static IEnumerable<(Direction direction, int value)> Parse(string input)
        => input.SplitLine()
            .Select(static line => line.Split(' '))
            .Select(static array => (Enum.Parse<Direction>(array[0], ignoreCase: true), int.Parse(array[1])));

    public object PartOne(string input)
    {
        var (position, depth) = (0, 0);
        foreach (var (direction, value) in Parse(input))
            switch (direction)
            {
                case Direction.Forward:
                    position += value;
                    break;
                case Direction.Up:
                    depth -= value;
                    break;
                case Direction.Down:
                    depth += value;
                    break;
            }
        return position * depth;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
