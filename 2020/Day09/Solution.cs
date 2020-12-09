using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day09
{
    class Solution : Solver
    {
        public string Name => "Encoding Error";

        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input)
        {
            var inputs = input.SplitLine().Select(long.Parse).ToArray().AsSpan();
            var preamble = inputs.Length > 25 ? 25 : 5;

            for (var i = preamble; i < inputs.Length; i++)
            {
                var current = inputs[i];
                if(!Find(inputs[(i - preamble)..i], current))
                    return current;
            }
            throw new Exception();

            static bool Find(Span<long> previous, long current)
            {
                for (var i = 0; i < previous.Length; i++)
                {
                    var item = previous[i];
                    if(item >= current)
                        continue;
                    var other = current - item;
                    if(other == item)
                        continue;
                    if (previous[(i + 1)..].Contains(other))
                        return true;
                }
                return false;
            }
        }

        long PartTwo(string input) => 0;
    }
}