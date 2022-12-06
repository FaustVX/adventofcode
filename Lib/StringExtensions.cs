using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class StringExtensions
{
    [DebuggerStepThrough]
    public static string StripMargin(this string st, string margin = "|")
    {
        return string.Join("\n",
            from line in st.SplitLine()
            select Regex.Replace(line, @"^\s*" + Regex.Escape(margin), "")
        );
    }

    [DebuggerStepThrough]
    public static string Indent(this string st, int l)
    {
        return string.Join("\n" + new string(' ', l),
            from line in st.SplitLine()
            select Regex.Replace(line, @"^\s*\|", "")
        );
    }

    [DebuggerStepThrough]
    public static string[] SplitLine(this string st)
        => Regex.Split(st, "\r?\n");

    [DebuggerStepThrough]
    public static IReadOnlyList<ReadOnlyMemory<char>> SplitLine(this ReadOnlyMemory<char> st)
    {
        var matches = Regex.EnumerateMatches(st.Span, "\r?\n");
        var start = 0;
        var list = new List<ReadOnlyMemory<char>>();
        foreach (var match in matches)
        {
            list.Add(st[start..match.Index]);
            start = match.Index + match.Length;
        }
        list.Add(st[start..]);
        return list.AsReadOnly();
    }

    [DebuggerStepThrough]
    public static string[] Split2Lines(this string st)
        => Regex.Split(st, "\r?\n\r?\n");

    [DebuggerStepThrough]
    public static IReadOnlyList<ReadOnlyMemory<char>> Split2Lines(this ReadOnlyMemory<char> st)
    {
        var matches = Regex.EnumerateMatches(st.Span, "(\r?\n){2}");
        var start = 0;
        var list = new List<ReadOnlyMemory<char>>();
        foreach (var match in matches)
        {
            list.Add(st[start..match.Index]);
            start = match.Index + match.Length;
        }
        list.Add(st[start..]);
        return list.AsReadOnly();
    }

    [DebuggerStepThrough]
    public static void TypeString(this StringBuilder input, TimeSpan maxTotalDuration)
    {
        var offset = maxTotalDuration / input.Length;
        foreach (var chunk in input.GetChunks())
            foreach (var letter in chunk.Span)
            {
                Thread.Sleep(Random.Shared.NextDouble() * offset);
                Console.Write(letter);
            }
    }

    [DebuggerStepThrough]
    public static void TypeString(this IEnumerable<TypedString> input, TimeSpan maxTotalDuration)
    {
        var strings = input.ToArray();
        var offset = maxTotalDuration / strings.Sum(static s => s.Input.Length);
        var (fore, back) = (Console.ForegroundColor, Console.BackgroundColor);
        foreach (var chunk in input)
        {
            if (chunk.Background is { } b)
                Console.BackgroundColor = b;
            if (chunk.Foregroung is { } f)
                Console.ForegroundColor = f;
            foreach (var letter in chunk.Input)
            {
                Thread.Sleep(Random.Shared.NextDouble() * offset);
                Console.Write(letter);
            }
            if (chunk.Background is { })
                Console.BackgroundColor = back;
            if (chunk.Foregroung is { })
                Console.ForegroundColor = fore;
        }
    }

    public static bool ParseFormated(this ReadOnlyMemory<char> input, [InterpolatedStringHandlerArgument(nameof(input))] ParserInterpolatedHandler handler, bool allowTrailling)
    => handler.IsValid && allowTrailling || handler.Trailling == 0;

    public static bool ParseFormated(this ReadOnlySpan<char> input, [InterpolatedStringHandlerArgument(nameof(input))] ParserInterpolatedHandler handler, bool allowTrailling)
    => handler.IsValid && allowTrailling || handler.Trailling == 0;
}

[InterpolatedStringHandler]
public ref struct ParserInterpolatedHandler
{
    private ReadOnlySpan<char> _input;
    public ParserInterpolatedHandler(int literalLength, int formattedCount, ReadOnlyMemory<char> input)
    {
        _input = input.Span;
    }
    public ParserInterpolatedHandler(int literalLength, int formattedCount, ReadOnlySpan<char> input)
    {
        _input = input;
    }

    public bool IsValid { get; private set; } = true;
    public int Trailling => _input.Length;

    public bool AppendLiteral(string s)
    {
        if (!_input.StartsWith(s))
                return IsValid = false;
        _input = _input[s.Length..];
        return true;
    }

    public bool AppendFormatted<T>(in T t)
        where T : INumber<T>
    {
        for (int i = _input.Length - 1; i >= 0; i--)
            if (T.TryParse(_input.Slice(0, i), null, out var o))
            {
                _input = _input.Slice(i);
                Unsafe.AsRef(in t) = o;
                return true;
            }
        return IsValid = false;
    }

    public bool AppendFormatted<T>(in T t, string format)
        where T : INumber<T>
    {
        var matches = Regex.EnumerateMatches(_input, format);
        if (!matches.MoveNext() || matches.Current.Index != 0)
            return IsValid = false;
        if (matches.Current.Length == 0)
            return true;
        if (!T.TryParse(_input.Slice(0, matches.Current.Length), null, out var o))
            return IsValid = false;

        _input = _input.Slice(matches.Current.Length);
        Unsafe.AsRef(in t) = o;
        return true;
    }
}

public class TypedString
{
    public ConsoleColor? Foregroung { get; init; }
    public ConsoleColor? Background { get; init; }
    public required string Input { get; init; }
}
