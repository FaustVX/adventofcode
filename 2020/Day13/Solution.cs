using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day13;

[ProblemName("Shuttle Search")]
class Solution : Solver
{
    public object PartOne(string input)
    {
        var (timeStamp, buses) = input.Extract<(int, List<int>)>(@"(\d+)\W+(?:(?:(\d+)|x),?)+")!;
        return buses.Select(bus => (bus, mod: -(timeStamp % bus) + bus))
            .OrderBy(t => t.mod)
            .Select(t => t.bus * t.mod)
            .First();
    }

    public object PartTwo(string input)
    {
        var (_, buses) = input.Extract<(int, List<string> buses)>(@"(\d+)\W+(?:(\d+|x),?)+")!;
        var list = buses.Select((bus, pos) => (bus, pos))
            .Select(t => ((int.TryParse(t.bus, out var bus), bus), t.pos))
            .Where(t => t.Item1.Item1)
            .Select(t => (bus: (ulong)t.Item1.bus, pos: (ulong)t.pos))
            .ToArray();

        var max = list.OrderByDescending(l => l.bus).First();
        var i = list.Length > 5 ? 100_000_000_000_000u : 1068_000;
        i = i + max.bus - (i % max.bus) - max.pos;
        // var i = max.bus - max.pos;
        while (Cond(i))
            i = checked(i + max.bus);

        return i;

        bool Cond(ulong ts)
            => !list.All(l => ((ts + l.pos) % l.bus) is 0);
    }
}

public static class Ext
{
    public static IEnumerable<(T last, T curr)> Slide<T>(this IEnumerable<T> source)
    {
        var enumerable = source.GetEnumerator();
        if (!enumerable.MoveNext())
            yield break;
        var last = enumerable.Current;
        while (enumerable.MoveNext())
            yield return (last, last = enumerable.Current);
    }
}
