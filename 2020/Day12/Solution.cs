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
            return 0;
        }
    }
}