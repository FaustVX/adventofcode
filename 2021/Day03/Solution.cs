using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day03;

[ProblemName("Binary Diagnostic")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var datas = input.SplitLine()
            .Select(l => l.ToCharArray())
            .ToArray();
        var half = datas.Length / 2;

        var gamma = datas.Aggregate(Enumerable.Repeat(0, datas[0].Length),
            static (acc, data) => data
                .Zip(acc)
                .Select(static v => v.Second + (v.First - '0')))
        .Aggregate(0, (acc, sum) => (acc << 1) + (sum > half ? 1 : 0));
        return gamma * (~gamma & Convert.ToInt32(new string('1', datas[0].Length), 2));
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
