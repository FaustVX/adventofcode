using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day12
{
    [ProblemName("Rain Risk")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
        {
            var (e, n, o) = (0, 0, 0);
            foreach (var dir in input.SplitLine().Select(line => line.Extract<(char, int)>(@"(\w)(\d+)")))
            {
                (e, n, o) = dir switch
                {
                    ('N', var l) => (e, n + l, o),
                    ('S', var l) => (e, n - l, o),
                    ('E', var l) => (e + l, n, o),
                    ('W', var l) => (e - l, n, o),
                    ('L', var l) => (e, n, (o - l + 360) % 360),
                    ('R', var l) => (e, n, (o + l) % 360),
                    ('F', var l) => o switch
                        {
                            0 => (e + l, n, o),
                            90 => (e, n - l, o),
                            180 => (e - l, n, o),
                            270 => (e, n + l, o),
                        }
                };
            }
            return Math.Abs(e) + Math.Abs(n);
        }

        int PartTwo(string input)
        {
            var (e, n, w) = (0, 0, (e: 10, n: 1));
            foreach (var dir in input.SplitLine().Select(line => line.Extract<(char, int)>(@"(\w)(\d+)")))
            {
                (e, n, w) = dir switch
                {
                    ('N', var l) => (e, n, (w.e, w.n + l)),
                    ('S', var l) => (e, n, (w.e, w.n - l)),
                    ('E', var l) => (e, n, (w.e + l, w.n)),
                    ('W', var l) => (e, n, (w.e - l, w.n)),
                    ('L', 90) or ('R', 270) => (e, n, (-w.n, w.e)),
                    ('R', 90) or ('L', 270) => (e, n, (w.n, -w.e)),
                    ('L' or 'R', 180) => (e, n, (-w.e, -w.n)),
                    ('F', var l) => (e + l * w.e, n + l * w.n, w),
                };
            }
            return Math.Abs(e) + Math.Abs(n);
        }
    }
}