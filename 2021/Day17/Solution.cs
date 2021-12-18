using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day17;

[ProblemName("Trick Shot")]
class Solution : Solver
{
    struct World
    {
        public readonly int Left, Right, Bottom, Top;

        public World(int left, int right, int bottom, int top)
            => (Left, Right, Bottom, Top) = (left, right, bottom, top);

        public IEnumerable<(int x, int y)> CalculateTrajectory(int xVelocity, int yVelocity)
        {
            var (x, y) = (0, 0);
            while (x <= Right && y >= Bottom)
            {
                if (xVelocity == 0 && (x < Left || x > Right))
                    yield break;
                 yield return (x, y);
                 x += xVelocity;
                 y += yVelocity;
                 if (xVelocity > 0)
                    xVelocity--;
                else if (xVelocity < 0)
                    xVelocity++;
                yVelocity--;
            }
        }
    }
    private static World Parse(string input)
    {
        var (_, (left, (right, (_, (bottom, (top, _)))))) = input.Split("=.,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        return new(int.Parse(left), int.Parse(right), int.Parse(bottom), int.Parse(top));
    }

    public object PartOne(string input)
    {
        var world = Parse(input);
        var maxY = 0;
        for (int yVelocity = 1; yVelocity < 1000; yVelocity++)
            for (int xVelocity = 1; xVelocity < 1000; xVelocity++)
            {
                var (max, isValid) = (0, false);
                foreach (var (x, y) in world.CalculateTrajectory(xVelocity, yVelocity))
                {
                    if (y > max)
                        max = y;
                    if (x >= world.Left && x <= world.Right && y >= world.Bottom && y <= world.Top)
                    {
                        isValid = true;
                        break;
                    }
                }
                if (isValid && max > maxY)
                    maxY = max;
            }
        return maxY;
    }

    public object PartTwo(string input)
    {
        var world = Parse(input);
        var count = 0;
        for (int yVelocity = -1000; yVelocity < 1000; yVelocity++)
            for (int xVelocity = 1; xVelocity < 1000; xVelocity++)
            {
                var (max, isValid) = (0, false);
                foreach (var (x, y) in world.CalculateTrajectory(xVelocity, yVelocity))
                {
                    if (y > max)
                        max = y;
                    if (x >= world.Left && x <= world.Right && y >= world.Bottom && y <= world.Top)
                    {
                        isValid = true;
                        break;
                    }
                }
                if (isValid)
                    count++;
            }
        return count;
    }
}
