using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using static RegExtract.RegExtractExtensions;

namespace AdventOfCode.Y2020.Day14
{
    [ProblemName("Docking Data")]
    class Solution : Solver
    {
        public IEnumerable<object> Solve(string input)
        {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input)
        {
            var memory = new Dictionary<long, long>();
            foreach (var program in input.Split("mask = ").Skip(1))
            {
                var splitted = program.SplitLine();
                var mask = splitted[0];
                foreach (var (addr, val) in splitted.Skip(1).TakeWhile(l => l is not "").Select(l => l.Extract<(long, long)>(@"mem\[(\d+)\] = (\d+)")))
                {
                    var value = val;
                    for (int i = 0; i < mask.Length; i++)
                    {
                        switch (mask[mask.Length - i - 1])
                        {
                            case '0':
                                value &= ~(1L << i);
                                break;
                            case '1':
                                value |= 1L << i;
                                break;
                        }
                    }
                    memory[addr] = value;
                }
            }
            return memory.Values.Sum();
        }

        int PartTwo(string input)
        {
            return 0;
        }
    }
}