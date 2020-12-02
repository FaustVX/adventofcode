using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day02 {

    class Solution : Solver {

        public string Name => "Password Philosophy";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input.Split(new[]{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(PasswordPolicy.Parse));
            yield return PartTwo(input);
        }

        int PartOne(IEnumerable<PasswordPolicy> paswords)
            => paswords.Count(PasswordPolicy.IsValid);

        int PartTwo(string input) => 0;
    }

    struct PasswordPolicy
    {
        public Range Quantity { get; init; }
        public char Letter { get; init; }
        public string Password { get; init; }

        public static bool IsValid(PasswordPolicy password)
            => password.Password.Count(c => c == password.Letter) is var c && IsValid(c, password.Quantity);

        private static bool IsValid(int count, Range range)
            => count >= range.Start.Value && count <= range.End.Value;

        public static PasswordPolicy Parse(string input)
        {
            var splitted = Regex.Split(input, @"^(\d+)-(\d+) ([a-z]): ([a-z]*)$");
            return new(){ Quantity = new(int.Parse(splitted[1]), int.Parse(splitted[2])), Letter = splitted[3][0], Password = splitted[4] };
        }
    }
}