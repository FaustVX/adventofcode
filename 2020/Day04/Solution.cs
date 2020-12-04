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
        private delegate bool TryParse<T>(string input, out T output);
        public string Name => "Passport Processing";

        public IEnumerable<object> Solve(string input)
        {
            var pattern = new Regex("\r?\n|\\s");

            var passports = Regex.Split(input, "(?:\r?\n){2}")
                .Select(p => pattern.Split(p)
                    .Select(l => l.Split(':'))
                    .ToDictionary(l => l[0], l => l[1]))
                .ToArray();

            yield return PartOne(passports);
            yield return PartTwo(passports);
        }

        int PartOne(Dictionary<string, string>[] passports)
            => passports.Count(p => p.Count == 8 || (p.Count == 7 && !p.ContainsKey("cid")));

        int PartTwo(Dictionary<string, string>[] passports)
        {
            var hgtcm = new Regex(@"^(\d+)cm$");
            var hgtin = new Regex(@"^(\d+)in$");
            var hcl = new Regex(@"^#[0-9a-f]{6}$");
            var pid = new Regex(@"^[0-9]{9}$");

            return passports
                .Where(p => p.Count >= 7)
                .Where(IsValid("byr", int.TryParse, 1920, 2002))
                .Where(IsValid("iyr", int.TryParse, 2010, 2020))
                .Where(IsValid("eyr", int.TryParse, 2020, 2030))
                .Where(Or(IsValid("hgt", hgtcm, int.TryParse, 150, 193), IsValid("hgt", hgtin, int.TryParse, 59, 76)))
                .Where(IsValid("hcl", hcl))
                .Where(IsValid("ecl", "amb", "blu", "brn", "gry", "grn", "hzl", "oth"))
                .Where(IsValid("pid", pid))
                .Count();
        }

        static Func<T, bool> Or<T>(Func<T, bool> left, Func<T, bool> right)
            => val
                => left(val) || right(val);

        static Func<Dictionary<string, string>, bool> IsValid<T>(string key, TryParse<T> parser, T min, T max)
            where T : IComparable<T>
            => passport
                => passport.TryGetValue(key, out var v) && parser(v, out var k) && k.CompareTo(min) >= 0 && k.CompareTo(max) <= 0;

        static Func<Dictionary<string, string>, bool> IsValid<T>(string key, Regex regex, TryParse<T> parser, T min, T max)
            where T : IComparable<T>
            => passport
                => passport.TryGetValue(key, out var v) && parser(regex.Match(v).Groups[1].Value, out var k) && k.CompareTo(min) >= 0 && k.CompareTo(max) <= 0;

        static Func<Dictionary<string, string>, bool> IsValid(string key, Regex regex)
            => passport
                => passport.TryGetValue(key, out var v) && regex.IsMatch(v);

        static Func<Dictionary<string, string>, bool> IsValid(string key, params string[] values)
            => passport
                => passport.TryGetValue(key, out var v) && values.Contains(v);
    }
}