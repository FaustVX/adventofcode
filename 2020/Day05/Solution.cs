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
            var (minRow, maxRow, lenRow) = (0, 127, 128);
            var (minCol, maxCol, lenCol) = (0, 7, 8);

            foreach (var c in boardingPass)
            {
                switch (c)
                {
                    case 'F':
                        lenRow /= 2;
                        maxRow = minRow + (lenRow - 1);
                        break;
                    case 'B':
                        lenRow /= 2;
                        minRow = maxRow - (lenRow - 1);
                        break;
                    case 'L':
                        lenCol /= 2;
                        maxCol = minCol + (lenCol - 1);
                        break;
                    case 'R':
                        lenCol /= 2;
                        minCol = maxCol - (lenCol - 1);
                        break;
                }
            }

            return minRow * 8 + minCol;
        }
    }
}