namespace AdventOfCode;

public static class StringExtensions {
    [DebuggerStepThrough]
    public static string StripMargin(this string st, string margin = "|") {
        return string.Join("\n",
            from line in st.SplitLine()
            select Regex.Replace(line, @"^\s*"+Regex.Escape(margin), "")
        );
    }

    [DebuggerStepThrough]
    public static string Indent(this string st, int l) {
        return string.Join("\n" + new string(' ', l),
            from line in st.SplitLine()
            select Regex.Replace(line, @"^\s*\|", "")
        );
    }

    [DebuggerStepThrough]
    public static string[] SplitLine(this string st)
        => Regex.Split(st, "\r?\n");

    [DebuggerStepThrough]
    public static string[] Split2Lines(this string st)
        => Regex.Split(st, "\r?\n\r?\n");

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
            if (chunk.Background is {} b)
                Console.BackgroundColor = b;
            if (chunk.Foregroung is {} f)
                Console.ForegroundColor = f;
            foreach (var letter in chunk.Input)
            {
                Thread.Sleep(Random.Shared.NextDouble() * offset);
                Console.Write(letter);
            }
            if (chunk.Background is {})
                Console.BackgroundColor = back;
            if (chunk.Foregroung is {})
                Console.ForegroundColor = fore;
        }
    }
}

public class TypedString
{
    public ConsoleColor? Foregroung { get; init; }
    public ConsoleColor? Background { get; init; }
    public required string Input { get; init; }
}
