using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class StringExtensions
{
    [DebuggerStepThrough]
    public static string StripMargin(this string st, string margin = "|")
    {
#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
        return string.Join("\n",
            from line in st.SplitLine()
            select Regex.Replace(line, @"^\s*" + Regex.Escape(margin), "")
        );
#pragma warning restore CS0612
    }

    [DebuggerStepThrough]
    public static string Indent(this string st, int l)
    {
        return string.Join("\n" + new string(' ', l),
#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
            from line in st.SplitLine()
#pragma warning restore CS0612
            select Regex.Replace(line, @"^\s*\|", "")
        );
    }

    [DebuggerStepThrough, Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
    public static string[] SplitLine(this string st)
        => Regex.Split(st, "\r?\n");

    [DebuggerStepThrough]
    public static Memory<ReadOnlyMemory<char>> SplitLine(this ReadOnlyMemory<char> st)
    {
        var matches = Regex.EnumerateMatches(st.Span, "\r?\n");
        var start = 0;
        var list = new ReadOnlyMemory<char>[Regex.Count(st.Span, "\r?\n") + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }

    [DebuggerStepThrough, Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
    public static string[] SplitSpace(this string st)
        => Regex.Split(st, "\\s");

    [DebuggerStepThrough]
    public static Memory<ReadOnlyMemory<char>> SplitSpace(this ReadOnlyMemory<char> st)
    {
        var matches = Regex.EnumerateMatches(st.Span, "\\s");
        var start = 0;
        var list = new ReadOnlyMemory<char>[Regex.Count(st.Span, "\\s") + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }

    [DebuggerStepThrough, Obsolete, EditorBrowsable(EditorBrowsableState.Never)]
    public static string[] Split2Lines(this string st)
        => Regex.Split(st, "\r?\n\r?\n");

    [DebuggerStepThrough]
    public static Memory<ReadOnlyMemory<char>> Split2Lines(this ReadOnlyMemory<char> st)
    {
        var matches = Regex.EnumerateMatches(st.Span, "(\r?\n){2}");
        var start = 0;
        var list = new ReadOnlyMemory<char>[Regex.Count(st.Span, "(\r?\n){2}") + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }

    [DebuggerStepThrough]
    public static Memory<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> st, [StringSyntax(StringSyntaxAttribute.Regex)]string regexSplit)
    {
        var matches = Regex.EnumerateMatches(st.Span, regexSplit);
        var start = 0;
        var list = new ReadOnlyMemory<char>[Regex.Count(st.Span, regexSplit) + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
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

    public static bool TryParseFormated<T>(this ReadOnlyMemory<char> input, [InterpolatedStringHandlerArgument(nameof(input))] ParserInterpolatedHandler<T> handler, out T values, bool allowTrailling = false)
    where T : struct, ITuple
    {
        values = (T)handler.Values;
        return handler.IsValid && (allowTrailling || !handler.HasTrailling);
    }
}

[InterpolatedStringHandler]
public ref struct ParserInterpolatedHandler<T>
where T : struct, ITuple
{
    private static readonly List<System.Reflection.FieldInfo> _fields;
    public readonly object Values = default(T);
    private ReadOnlySpan<char> _input;
    private int _fieldIndex;

    static ParserInterpolatedHandler()
    {
        var type = typeof(T);
        _fields = new(default(T).Length);
        for (int i = 0; i < default(T).Length; i++)
            _fields.Add(type.GetField($"Item{i + 1}")!);
    }

    public ParserInterpolatedHandler(int literalLength, int formattedCount, ReadOnlyMemory<char> input)
    : this(literalLength, formattedCount, input.Span)
    { }

    public ParserInterpolatedHandler(int literalLength, int formattedCount, ReadOnlySpan<char> input)
    {
        _input = input;
    }

    private void SetValue<U>(U value)
    => _fields[_fieldIndex++].SetValue(Values, value);

    public bool IsValid { get; private set; } = true;
    public bool HasTrailling => _input.Length != 0;

    public bool AppendLiteral(string s)
    {
        if (!_input.StartsWith(s))
                return IsValid = false;
        _input = _input[s.Length..];
        return true;
    }

    public bool AppendFormatted(byte t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(sbyte t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(short t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(ushort t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(int t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(uint t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(long t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(ulong t)
    => AppendFormatted(t, """\d+""");

    public bool AppendFormatted(float t)
    => AppendFormatted(t, """[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?""");

    public bool AppendFormatted(double t)
    => AppendFormatted(t, """[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?""");

    public bool AppendFormatted<U>(U t, string format)
        where U : ISpanParsable<U>
    {
        var matches = Regex.EnumerateMatches(_input, format);
        if (!matches.MoveNext() || matches.Current.Index != 0)
            return IsValid = false;
        if (matches.Current.Length == 0)
            return true;
        if (!U.TryParse(_input.Slice(0, matches.Current.Length), null, out var o))
            return IsValid = false;

        _input = _input.Slice(matches.Current.Length);
        SetValue(o);
        return true;
    }

    public bool AppendFormatted(string pattern)
    {
        var matches = Regex.EnumerateMatches(_input, pattern);
        if (!matches.MoveNext() || matches.Current.Index != 0)
            return IsValid = false;
        if (matches.Current.Length == 0)
            return true;

        _input = _input.Slice(matches.Current.Length);
        return true;
    }
}

public class TypedString
{
    public ConsoleColor? Foregroung { get; init; }
    public ConsoleColor? Background { get; init; }
    public required string Input { get; init; }
}
