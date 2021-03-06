using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2020.Day01 {

    [ProblemName("Report Repair")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve1(2020, input.SplitLine().Select(int.Parse).ToArray());
        long PartTwo(string input) => Solve2(2020, input.SplitLine().Select(int.Parse).ToArray());

        long Solve1(int sum, int[] inputs) {
            for (int i = 0; i < inputs.Length - 1; i++)
            {
                for (int j = i + 1; j < inputs.Length; j++)
                {
                    if(inputs[i] + inputs[j] == sum)
                        return inputs[i] * inputs[j];
                }
            }

            throw new();
        }

        long Solve2(int sum, int[] inputs) {
            for (int i = 0; i < inputs.Length - 2; i++)
            {
                for (int j = i + 1; j < inputs.Length - 1; j++)
                {
                    for (int k = 0; k < inputs.Length; k++)
                    {
                        if(inputs[i] + inputs[j] + inputs[k] == sum)
                            return inputs[i] * inputs[j] * inputs[k];
                    }
                }
            }

            throw new();
        }
    }
}