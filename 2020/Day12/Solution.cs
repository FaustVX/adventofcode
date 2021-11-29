using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day12;

[ProblemName("Rain Risk")]
class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine()
            .Select(line => line.Extract<(char, int)>(@"(\w)(\d+)"))
            .Aggregate((e: 0, n: 0, o: 0), (a, dir) => dir switch
            {
                ('N', var l) => (a.e, a.n + l, a.o),
                ('S', var l) => (a.e, a.n - l, a.o),
                ('E', var l) => (a.e + l, a.n, a.o),
                ('W', var l) => (a.e - l, a.n, a.o),
                ('L', var l) => (a.e, a.n, (a.o - l + 360) % 360),
                ('R', var l) => (a.e, a.n, (a.o + l) % 360),
                ('F', var l) => a.o switch
                {
                    0 => (a.e + l, a.n, a.o),
                    90 => (a.e, a.n - l, a.o),
                    180 => (a.e - l, a.n, a.o),
                    270 => (a.e, a.n + l, a.o),
                }
            }, a => Math.Abs(a.e) + Math.Abs(a.n));

    public object PartTwo(string input)
        => input.SplitLine()
            .Select(line => line.Extract<(char, int)>(@"(\w)(\d+)"))
            .Aggregate((e: 0, n: 0, w: (e: 10, n: 1)), (a, dir) => dir switch
            {
                ('N', var l) => (a.e, a.n, (a.w.e, a.w.n + l)),
                ('S', var l) => (a.e, a.n, (a.w.e, a.w.n - l)),
                ('E', var l) => (a.e, a.n, (a.w.e + l, a.w.n)),
                ('W', var l) => (a.e, a.n, (a.w.e - l, a.w.n)),
                ('L', 90) or ('R', 270) => (a.e, a.n, (-a.w.n, a.w.e)),
                ('R', 90) or ('L', 270) => (a.e, a.n, (a.w.n, -a.w.e)),
                ('L' or 'R', 180) => (a.e, a.n, (-a.w.e, -a.w.n)),
                ('F', var l) => (a.e + l * a.w.e, a.n + l * a.w.n, a.w),
            }, a => Math.Abs(a.e) + Math.Abs(a.n));
}
