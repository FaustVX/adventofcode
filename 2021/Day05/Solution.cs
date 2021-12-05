using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day05;

[ProblemName("Hydrothermal Venture")]
class Solution : Solver
{
    private class DefaultableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TKey : notnull
    {
        private readonly TValue _defaultValue;
        public DefaultableDictionary(int capacity, TValue defaultValue = default)
            : base(capacity)
            => _defaultValue = defaultValue;

        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var value))
                    return value;
                return _defaultValue;
            }
            set => base[key] = value;
        }
    }

    public static IEnumerable<((int x, int y) pos1, (int x, int y) pos2)> Parse(string input)
    {
        foreach (var line in input.SplitLine())
        {
            var splitted = line.Split(" -> ")
                .SelectMany(v => v.Split(','))
                .Select(v => int.Parse(v))
                .ToArray();
            yield return ((splitted[0], splitted[1]), (splitted[2], splitted[3]));
        }
    }

    public object PartOne(string input)
    {
        var positions = new DefaultableDictionary<(int x, int y), int>(capacity: 1000);
        foreach (var line in Parse(input))
            if (line is var ((x1, y1), (x2, y2)))
                if (x1 == x2)
                    if (y1 < y2)
                        for (var y = y1; y <= y2; y++)
                            positions[(x1, y)]++;
                    else if (y1 > y2)
                        for (var y = y2; y <= y1; y++)
                            positions[(x1, y)]++;
                    else
                        positions[(x1, y1)]++;
                else if (y1 == y2)
                    if (x1 < x2)
                        for (var x = x1; x <= x2; x++)
                            positions[(x, y1)]++;
                    else if (x1 > x2)
                        for (var x = x2; x <= x1; x++)
                            positions[(x, y1)]++;
                    else
                        positions[(x1, y1)]++;
        return positions.Values.Count(v => v >= 2);
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
