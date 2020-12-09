using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day06
{
    [ProblemName("Custom Customs")]      
    class Solution : Solver {

        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input)
            => Regex.Split(input, "\r?\n\r?\n").Select(group => group.SplitLine().SelectMany(l => l).ToHashSet().Count).Sum();

        int PartTwo(string input)
        {
            var sum = 0;
            foreach (var group in Regex.Split(input, "\r?\n\r?\n"))
            {
                var dictionary = Enumerable.Range(0, 26).ToDictionary(i => (char)('a' + i), _ => 0);
                var people = group.SplitLine();
                foreach (var person in people)
                {
                    foreach (var answer in person)
                    {
                        dictionary[answer]++;
                    }
                }
                sum += dictionary.Values.Count(v => v == people.Length);
            }
            return sum;
        }
    }
}