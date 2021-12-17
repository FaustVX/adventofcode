using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Globalization;

namespace AdventOfCode.Y2021.Day16;

[ProblemName("Packet Decoder")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var packet = BigInteger.Parse(input, NumberStyles.HexNumber)
            .ToByteArray()
            .Reverse()
            .Aggregate(new StringBuilder(), static (sb, b) => sb.Append(Convert.ToString(b, 2).PadLeft(8, '0')))
            .ToString();
        return GetPacketsVersion(packet, out _);

        static int GetPacketsVersion(ReadOnlySpan<char> packet, out int readBits)
        {
            var version = ParseBinary(packet[..3]);
            var type = ParseBinary(packet[3..6]);
            if (type is 4)
            {
                var i = 6;
                while (packet[i] is '1')
                     i += 5;
                readBits = i + 5;
                return version;
            }
            readBits = 7;
            switch (packet[6])
            {
                case '0':
                {
                    readBits += 15;
                    var length = readBits + ParseBinary(packet[7..readBits]);
                    while (readBits < length)
                    {
                        version += GetPacketsVersion(packet[readBits..], out var bits);
                        readBits += bits;
                    }
                    return version;
                }
                case '1':
                {
                    readBits += 11;
                    var length = ParseBinary(packet[7..readBits]);
                    for (int i = 0; i < length; i++)
                    {
                        version += GetPacketsVersion(packet[readBits..], out var bits);
                        readBits += bits;
                    }
                    return version;
                }
                default:
                    throw new();
            }
        }

        static int ParseBinary(ReadOnlySpan<char> input)
        {
            var value = 0;
            foreach (var item in input)
            {
                value <<= 1;
                value += item is '1' ? 1 : 0;
            }
            return value;
        }
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
