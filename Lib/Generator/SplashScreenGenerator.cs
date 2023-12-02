using AdventOfCode.Model;

namespace AdventOfCode.Generator;

#if !LIBRARY
[DebuggerStepThrough]
#endif
internal static partial class SplashScreenGenerator
{
    public static string Generate(Calendar calendar)
    => $$"""
    namespace AdventOfCode.Y{{calendar.Year}};

    class SplashScreenImpl : {{nameof(ISplashScreen)}}
    {
        public void Show()
        {
            var color = Console.ForegroundColor;
    {{CalendarPrinter(calendar, 8)}}
            Console.ForegroundColor = color;
            Console.WriteLine();
        }

        private static void Write(int rgb, bool bold, string text)
        => Console.Write($"\u001b[38;2;{(rgb >> 16) & 255};{(rgb >> 8) & 255};{rgb & 255}{(bold ? ";1" : "")}m{text}");
    }
    """;

    private static string CalendarPrinter(Calendar calendar, int indent)
    {
        var lines = calendar.Lines.Select(static line => line.Prepend(CalendarToken.FromText("           ")));

        var bw = new BufferWriter(indent);
        foreach (var line in lines)
        {
            foreach (var token in line)
                bw.Write(token.ConsoleColor, token.Text, token.Bold);
            bw.Write(-1, "\n", false);
        }
        return bw.GetContent();
    }

    private partial class BufferWriter([Field(Name = nameof(BufferWriter.indent), AssignFormat = "new(' ', {0})", Type = typeof(string))] int indent)
    {
        private readonly StringBuilder sb = new();
        private int bufferColor = -1;
        private string buffer = "";
        private bool bufferBold;

        public void Write(int color, string text, bool bold)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (!string.IsNullOrWhiteSpace(buffer) && (color != bufferColor || this.bufferBold != bold))
                    Flush();
                bufferColor = color;
                bufferBold = bold;
            }
            buffer += text;
        }

        private void Flush()
        {
            while (buffer.Length > 0)
            {
                var block = buffer[..Math.Min(100, buffer.Length)];
                buffer = buffer[block.Length..];
                block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
                sb.AppendLine(indent + $@"Write(0x{bufferColor.ToString("x")}, {bufferBold.ToString().ToLower()}, ""{block}"");");
            }
            buffer = "";
        }

        public string GetContent()
        {
            Flush();
            return sb.ToString();
        }
    }
}
