using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day10
{
    [ProblemName("Adapter Array")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
            => input.SplitLine().Select(int.Parse).OrderBy(i => i)
                .Aggregate((one: 0, three: 0, last: 0), (a, c) => (c - a.last) switch {
                    1 => (a.one + 1, a.three, c),
                    3 => (a.one, a.three + 1, c),
                    _ => (a.one, a.three, c),
                }, a => a.one * (a.three + 1));

        long PartTwo(string input)
        {
            var inputs = input.SplitLine().Select(int.Parse).OrderBy(i => i);
            return GetLists(0, inputs.Append(inputs.Max() + 3).ToArray()).LongCount();

            static IEnumerable<object?> GetLists(int value, int[] nexts)
            {
                for (var i = 0; i < nexts.Length; i++)
                {
                    var next = nexts[i];
                    if (next - value > 3)
                        break;
                    if (nexts.Length >= 2)
                        foreach (var item in GetLists(next, nexts[(i + 1)..]))
                            yield return null;
                    else
                        yield return null;
                }
            }
        }
    }
}