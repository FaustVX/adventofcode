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
            var partOne = PartOne(input);
            yield return partOne;
            yield return PartTwo(input, partOne);
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

        long PartTwo(string input, long partOne)
        {
            var values = Find(input.SplitLine().Select(long.Parse).ToArray(), partOne).ToArray();
            return values.Min() + values.Max();

            static LinkedList<long> Find(Span<long> span, long value)
            {
                for (int i = 0; i < span.Length; i++)
                {
                    if(GetList(span[i..], value) is {} list)
                        return list;
                }
                throw new Exception();

                static LinkedList<long>? GetList(Span<long> span, long value)
                {
                    var current = span[0];
                    if (current > value)
                        return null;
                    if(current == value)
                        return new(new[] { value });
                    switch (GetList(span[1..], value - current))
                    {
                        case null:
                            return null;
                        case var list:
                            list.AddLast(current);
                            return list;
                    }
                }
            }
        }
    }
}