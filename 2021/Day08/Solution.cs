using System.Runtime.InteropServices;

namespace AdventOfCode.Y2021.Day08;

[ProblemName("Seven Segment Search")]
public class Solution : Solver
{
    private class Entry
    {
        public Digit[] Patterns { get; init; }
        public Digit[] Values { get; init; }
        public Dictionary<char, char> Links { get; }
        private Entry(Digit[] patterns, Digit[] values)
        {
            (Patterns, Values) = (patterns, values);
            var remainingDigits = Patterns.ToList();
            var _1 = remainingDigits.Single(static digit => digit.Length is 2);
            remainingDigits.Remove(_1);
            var _cOr_f = _1.ToList();
            var _7 = remainingDigits.Single(static digit => digit.Length is 3);
            remainingDigits.Remove(_7);
            var _a = _7.Except(_1).Single();
            var _4 = remainingDigits.Single(static digit => digit.Length is 4);
            remainingDigits.Remove(_4);
            var _bOr_d = _4.Except(_1).ToList();
            var _abcdf = _bOr_d.Concat(_cOr_f).Append(_a).ToList();
            var _9 = remainingDigits.Where(static digit => digit.Length is 6)
                .Single(digit => digit.Except(_abcdf).Count() is 1);
            remainingDigits.Remove(_9);
            var _g = _9.Except(_abcdf).Single();
            var _8 = remainingDigits.Single(static digit => digit.Length is 7);
            remainingDigits.Remove(_8);
            var _e = _8.Except(_9).Single();
            var _0Or_6 = remainingDigits.Where(static digit => digit.Length is 6).ToList();
            var _cOr_d = _0Or_6[0].Except(_0Or_6[1]).ToList();
                _cOr_d.Add(_0Or_6[1].Except(_0Or_6[0]).Single());
            var _c = _cOr_d.Intersect(_cOr_f).Single();
            var _d = _cOr_d.Except(Enumerable.Repeat(_c, 1)).Single();
            var _f = _cOr_f.Except(Enumerable.Repeat(_c, 1)).Single();
            var _b = _bOr_d.Except(Enumerable.Repeat(_d, 1)).Single();

            Links = new(capacity: 7)
            {
                [_a] = 'a',
                [_b] = 'b',
                [_c] = 'c',
                [_d] = 'd',
                [_e] = 'e',
                [_f] = 'f',
                [_g] = 'g',
            };
        }
        public static Entry Parse(string input)
        {
            var (first, (end, _)) = input.Split(" | ");
            return new(
                first.Split(' ').ParseToArrayOfT(Digit.Parse),
                end.Split(' ').ParseToArrayOfT(Digit.Parse)
            );
        }
    }

    public class Digit : IEnumerable<char>
    {
        public char[] Segments { get; init; }
        public int Length => Segments.Length;

        public static Digit Parse(string input)
            => new() { Segments = input.ToCharArray() };

        public Digit Convert(IReadOnlyDictionary<char, char> links)
            => new()
            {
                Segments = Segments.Select(seg => links[seg]).ToArray()
            };

        public void Deconstruct(out bool a, out bool b, out bool c, out bool d, out bool e, out bool f, out bool g)
            => (a, b, c, d, e, f, g) = (Segments.Contains('a'), Segments.Contains('b'), Segments.Contains('c'), Segments.Contains('d'), Segments.Contains('e'), Segments.Contains('f'), Segments.Contains('g'));

        public IEnumerator<char> GetEnumerator()
            => ((IEnumerable<char>)Segments).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

    private static IEnumerable<Entry> Parse(string input)
        => input.ParseToIEnumOfT(Entry.Parse);

    public object PartOne(string input)
        => Parse(input).Sum(static entry => entry.Values.Count(static value => value.Length is 2 or 4 or 3 or 7));

    public object PartTwo(string input)
    {
        var entries = Parse(input).Select(static entry => entry.Values.Select(digit => digit.Convert(entry.Links)).ToArray());
        var sum = 0L;
        foreach (var entry in entries)
        {
            var value = 0;
            foreach (var digit in entry)
            {
                value *= 10;
#pragma warning disable CS8509 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value. For example, the pattern ‘...’ is not covered.
                value += digit switch
#pragma warning restore
                {
                    { Length: 6 } and (_, _, _, d: false, _, _, _) => 0,
                    { Length: 2 } => 1,
                    { Length: 5 } and (_, b: false, _, _, _, f: false, _) => 2,
                    { Length: 5 } and (_, b: false, _, _, e: false, _, _) => 3,
                    { Length: 4 } => 4,
                    { Length: 5 } and (_, _, c: false, _, e: false, _, _) => 5,
                    { Length: 6 } and (_, _, c: false, _, _, _, _) => 6,
                    { Length: 3 } => 7,
                    { Length: 7 } => 8,
                    { Length: 6 } and (_, _, _, _, e: false, _, _) => 9,
                };
            }
            sum += value;
        }
        return sum;
    }
}
