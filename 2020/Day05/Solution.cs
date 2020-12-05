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
            yield return input.SplitLine()
                .Select(CalculateSeatId)
                .OrderBy(id => id)
                .Aggregate(0, (o, c)
                    => (o, c) switch
                        {
                            (0, _) => c,
                            (_, _) when c - o == 1 => c,
                            (_, _) => o
                        }) + 1;
        }

        int CalculateSeatId(string boardingPass)
        {
            var (miR, maR, leR) = (0, 127, 128);
            var (miC, maC, leC) = (0, 7, 8);

            foreach (var c in boardingPass)
            {
                switch (c)
                {
                    case 'F':
                        leR /= 2;
                        maR = miR + (leR - 1);
                        break;
                    case 'B':
                        leR /= 2;
                        miR = maR - (leR - 1);
                        break;
                    case 'L':
                        leC /= 2;
                        maC = miC + (leC - 1);
                        break;
                    case 'R':
                        leC /= 2;
                        miC = maC - (leC - 1);
                        break;
                }
            }

            return miR * 8 + miC;
        }
    }
}