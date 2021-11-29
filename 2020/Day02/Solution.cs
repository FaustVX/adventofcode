using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using RegExtract;

namespace AdventOfCode.Y2020.Day02;

[ProblemName("Password Philosophy")]
class Solution : Solver
{
    public object PartOne(string input)
        => input.SplitLine().Select(PasswordPolicy.Parse).Count(PasswordPolicy.IsCountValid);

    public object PartTwo(string input)
        => input.SplitLine().Select(PasswordPolicy.Parse).Count(PasswordPolicy.IsPosValid);
}

struct PasswordPolicy
{
    private static readonly Regex _parser = new(@"^(\d+)-(\d+) ([a-z]): ([a-z]*)\r?$");
    public Range Quantity { get; init; }
    public char Letter { get; init; }
    public string Password { get; init; }

    public static bool IsCountValid(PasswordPolicy password)
        => password.Password.Count(c => c == password.Letter) is var c && IsCountValid(c, password.Quantity);

    private static bool IsCountValid(int count, Range range)
        => count >= range.Start.Value && count <= range.End.Value;

    public static bool IsPosValid(PasswordPolicy password)
        => password.Password[password.Quantity.Start.Value - 1] == password.Letter ^ password.Password[password.Quantity.End.Value - 1] == password.Letter;

    public static PasswordPolicy Parse(string input)
    {
        var (start, end, letter, password) = input.Extract<(int, int, char, string)>(_parser);
        return new(){ Quantity = new(start, end), Letter = letter, Password = password };
    }
}
