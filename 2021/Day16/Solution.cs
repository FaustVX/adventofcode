using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Globalization;
using System.Diagnostics;

namespace AdventOfCode.Y2021.Day16;

[ProblemName("Packet Decoder")]
partial class Solution : Solver
{
    private static IPacket Parse(string input)
    {
        var packet = BigInteger.Parse(input, NumberStyles.HexNumber)
            .ToByteArray()
            .Reverse()
            .Aggregate(new StringBuilder(), [DebuggerStepThrough] static (sb, b) => sb.Append(Convert.ToString(b, 2).PadLeft(8, '0')))
            .ToString();
            return IPacket.Parse(packet, out _);
    }
    public object PartOne(string input)
    {
        return GetInner(Parse(input)).Sum([DebuggerStepThrough] static (p) => p.Version);

        static IEnumerable<IPacket> GetInner(IPacket packet)
            => packet is IOperatorPacket op
                ? op.InnerPacket.SelectMany(GetInner).Append(packet)
                : Enumerable.Repeat(packet, 1);
    }

    public object PartTwo(string input)
        => Parse(input).Evaluate();
}
