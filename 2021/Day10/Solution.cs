using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day10;

[ProblemName("Syntax Scoring")]
class Solution : Solver
{
    class Chunck
    {
        private static char[] _openning = "([{<".ToCharArray(),
                              _closing  = ")]}>'".ToCharArray();

        public char Openning { get; }
        public char Closing { get; }
        public bool IsCorrupted { get; }
        public bool IsIncomplete { get; }
        public bool ContainsCorrupted { get; }
        public int Length { get; } = 2;
        public List<Chunck> Inner { get; } = new();

        public Chunck(ReadOnlySpan<char> input)
        {
            Openning = input[0];
            try {
                while (!_closing.Contains(input[Length - 1]))
                {
                    var item = new Chunck(input[(Length - 1)..]);
                    Length += item.Length;
                    Inner.Add(item);
                    ContainsCorrupted |= item.ContainsCorrupted;
                }
                Closing = input[Length - 1];
                IsCorrupted = _openning.AsSpan().IndexOf(Openning) != _closing.AsSpan().IndexOf(Closing);
                if (IsCorrupted)
                    ContainsCorrupted = true;
            }
            catch (IndexOutOfRangeException)
            {
                IsIncomplete = true;
                Length--;
            }
        }

        public Chunck GetCorruptedChunck()
        {
            if (!ContainsCorrupted)
                return null;
            if (IsCorrupted)
                return this;
            foreach (var chunck in Inner)
                if (chunck.GetCorruptedChunck() is {} corrupted)
                    return corrupted;
            return null;
        }

        public override string ToString()
            => $"{Openning}{string.Concat(Inner)}{Closing}";

        public static List<Chunck> Parse(string input)
        {
            var list = new List<Chunck>();
            var span = input.AsSpan();
            while (span.Length > 0)
            {
                var chunck = new Chunck(span);
                list.Add(chunck);
                span = span[chunck.Length..];
            }
            return list;
        }
    }

    public object PartOne(string input)
    {
        var sum = 0;
        foreach (var chuncks in input.ParseToIEnumOfT(Chunck.Parse))
        {
            if (!chuncks.Any(static chuck => chuck.ContainsCorrupted))
                continue;
            var corruptedChunck = chuncks
                .First(static chunck => chunck.ContainsCorrupted)
                .GetCorruptedChunck();
            sum += corruptedChunck.Closing switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
            };
        }
        return sum;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
