using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static partial class StringExtensions
{
    public static ReadOnlyMemory<ReadOnlyMemory<char>> SplitLine(this ReadOnlyMemory<char> st)
    {
        var matches = NewLineRegex().EnumerateMatches(st.Span);
        var start = 0;
        var list = new ReadOnlyMemory<char>[NewLineRegex().Count(st.Span) + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }
    public static ReadOnlyMemory<ReadOnlyMemory<char>> SplitSpace(this ReadOnlyMemory<char> st)
    {
        var matches = WhitespaceRegex().EnumerateMatches(st.Span);
        var start = 0;
        var list = new ReadOnlyMemory<char>[WhitespaceRegex().Count(st.Span) + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }
    public static ReadOnlyMemory<ReadOnlyMemory<char>> Split2Lines(this ReadOnlyMemory<char> st)
    {
        var matches = NewLine2Regex().EnumerateMatches(st.Span);
        var start = 0;
        var list = new ReadOnlyMemory<char>[NewLine2Regex().Count(st.Span) + 1];
        var index = 0;
        foreach (var match in matches)
        {
            list[index++] = st[start..match.Index];
            start = match.Index + match.Length;
        }
        list[index] = st[start..];
        return list;
    }

    public static ReadOnlyMemory<ReadOnlyMemory<char>> Split(this ReadOnlyMemory<char> st, [StringSyntax(StringSyntaxAttribute.Regex)] string regexSplit)
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

    public static SpanSplitEnumerator EnumerateSplits(this ReadOnlySpan<char> span, ReadOnlySpan<char> separator)
    => new SpanSplitEnumerator(span, separator);

    public static SpanSplitEnumerator EnumerateSplits(this string span, ReadOnlySpan<char> separator)
    => new SpanSplitEnumerator(span, separator);

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

    /// <summary>
    /// Returns an enumeration of lines over the provided memory.
    /// </summary>
    /// <remarks>
    /// It is recommended that protocol parsers not utilize this API. See the documentation
    /// for <see cref="string.ReplaceLineEndings"/> for more information on how newline
    /// sequences are detected.
    /// </remarks>
    public static MemoryLineEnumerator EnumerateLines(this ReadOnlyMemory<char> span)
    => new(span);

    /// <summary>
    /// Returns an enumeration of lines over the provided memory.
    /// </summary>
    /// <remarks>
    /// It is recommended that protocol parsers not utilize this API. See the documentation
    /// for <see cref="string.ReplaceLineEndings"/> for more information on how newline
    /// sequences are detected.
    /// </remarks>
    public static MemoryLineEnumerator EnumerateLines(this Memory<char> span)
    => new(span);
    [GeneratedRegex("\r?\n")]
    private static partial Regex NewLineRegex();
    [GeneratedRegex("\\s")]
    private static partial Regex WhitespaceRegex();
    [GeneratedRegex("(\r?\n){2}")]
    private static partial Regex NewLine2Regex();
}

#if !LIBRARY
[DebuggerStepThrough]
#endif
[InterpolatedStringHandler, StructLayout(LayoutKind.Auto)]
#pragma warning disable CS9113 // Parameter is unread.
public ref partial struct ParserInterpolatedHandler<T>([DoNotUse] int literalLength, [DoNotUse] int formattedCount, [Field(IsReadonly = false)] ReadOnlySpan<char> input)
#pragma warning restore CS9113 // Parameter is unread.
where T : struct, ITuple
{
    private static readonly List<System.Reflection.FieldInfo> _fields;
    public readonly object Values = default(T);
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

    private void SetValue<U>(U value)
    => _fields[_fieldIndex++].SetValue(Values, value);

    public bool IsValid { readonly get; private set; } = true;
    public readonly bool HasTrailling => _input.Length != 0;

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
        if (!U.TryParse(_input[..matches.Current.Length], null, out var o))
            return IsValid = false;

        _input = _input[matches.Current.Length..];
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

        _input = _input[matches.Current.Length..];
        return true;
    }
}

#if !LIBRARY
[DebuggerStepThrough]
#endif
public class TypedString
{
    public ConsoleColor? Foregroung { get; init; }
    public ConsoleColor? Background { get; init; }
    public required string Input { get; init; }
}
