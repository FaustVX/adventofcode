using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day04
{
    class Solution : Solver
    {
        public string Name => "Passport Processing";

        public IEnumerable<object> Solve(string input)
        {
            var passports = Regex.Split(input, "(?:\r?\n){2}")
                .Select(p => Regex.Split(p, "\r?\n|\\s")
                    .Select(l => l.Split(':'))
                    .ToDictionary(l => l[0], l => l[1]))
                .ToArray();

            yield return PartOne(passports);
            yield return PartTwo(input);
        }

        int PartOne(Dictionary<string, string>[] passports)
            => passports.Count(p => p.Count == 8 || (p.Count == 7 && !p.ContainsKey("cid")));

        int PartTwo(string input) => 0;
    }
}