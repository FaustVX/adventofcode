namespace AdventOfCode;

public static class OCR
{
    public interface IChar<T>
        where T : IChar<T>
    {
        abstract static IReadOnlyDictionary<char, T> Dictionary { get; }
        abstract static int Width { get; }
        abstract static int Height { get; }
        int this[int y] { get; }
        bool this[int x, int y]
            => x < T.Width
                ? (this[y] >> (T.Width - x - 1) & 1) == 1
                : throw new();
    }

    public struct Char5x5 : IChar<Char5x5>
    {
        public static IReadOnlyDictionary<char, Char5x5> Dictionary { get; } = new Dictionary<char, Char5x5>()
        {
            ['O'] = new(
                0b11111,
                0b10001,
                0b10001,
                0b10001,
                0b11111
            ),
        };

        public static int Width => 5;

        public static int Height => 5;

        public int this[int y]
            => y switch
            {
                0 => _line0,
                1 => _line1,
                2 => _line2,
                3 => _line3,
                4 => _line4,
                _ => throw new UnreachableException(),
            };

        private readonly byte _line0, _line1, _line2, _line3, _line4;

        private Char5x5(byte l0, byte l1, byte l2, byte l3, byte l4)
            => (_line0, _line1, _line2, _line3, _line4) = (l0, l1, l2, l3, l4);
    }

    public struct Char4x6 : IChar<Char4x6>
    {
        public static IReadOnlyDictionary<char, Char4x6> Dictionary { get; } = new Dictionary<char, Char4x6>()
        {
            ['A'] = new(
                0b0110,
                0b1001,
                0b1001,
                0b1111,
                0b1001,
                0b1001
            ),
            ['B'] = new(
                0b1110,
                0b1001,
                0b1110,
                0b1001,
                0b1001,
                0b1110
            ),
            ['C'] = new(
                0b0110,
                0b1001,
                0b1000,
                0b1000,
                0b1001,
                0b0110
            ),
            ['F'] = new(
                0b1111,
                0b1000,
                0b1110,
                0b1000,
                0b1000,
                0b1000
            ),
            ['G'] = new(
                0b0110,
                0b1001,
                0b1000,
                0b1011,
                0b1001,
                0b0111
            ),
            ['H'] = new(
                0b1001,
                0b1001,
                0b1111,
                0b1001,
                0b1001,
                0b1001
            ),
            ['J'] = new(
                0b0011,
                0b0001,
                0b0001,
                0b0001,
                0b1001,
                0b0110
            ),
            ['K'] = new(
                0b1001,
                0b1010,
                0b1100,
                0b1010,
                0b1010,
                0b1001
            ),
            ['L'] = new(
                0b1000,
                0b1000,
                0b1000,
                0b1000,
                0b1000,
                0b1111
            ),
            ['P'] = new(
                0b1110,
                0b1001,
                0b1001,
                0b1110,
                0b1000,
                0b1000
            ),
            ['R'] = new(
                0b1110,
                0b1001,
                0b1001,
                0b1110,
                0b1010,
                0b1001
            ),
            ['U'] = new(
                0b1001,
                0b1001,
                0b1001,
                0b1001,
                0b1001,
                0b0110
            ),
            ['Z'] = new(
                0b1111,
                0b0001,
                0b0010,
                0b0100,
                0b1000,
                0b1111
            ),
        };
        private readonly byte _line0, _line1, _line2, _line3, _line4, _line5;

        public static int Width => 4;
        public static int Height => 6;

        public int this[int y]
            => y switch
            {
                0 => _line0,
                1 => _line1,
                2 => _line2,
                3 => _line3,
                4 => _line4,
                5 => _line5,
                _ => throw new UnreachableException(),
            };

        private Char4x6(byte l0, byte l1, byte l2, byte l3, byte l4, byte l5)
            => (_line0, _line1, _line2, _line3, _line4, _line5) = (l0, l1, l2, l3, l4, l5);
    }

    public struct Char5x6 : IChar<Char5x6>
    {
        public static IReadOnlyDictionary<char, Char5x6> Dictionary { get; } = new Dictionary<char, Char5x6>()
        {
            ['E'] = new
            (
                0b11110,
                0b10000,
                0b11100,
                0b10000,
                0b10000,
                0b11110
            ),
            ['F'] = new(
                0b11110,
                0b10000,
                0b11100,
                0b10000,
                0b10000,
                0b10000
            ),
            ['I'] = new(
                0b01110,
                0b00100,
                0b00100,
                0b00100,
                0b00100,
                0b01110
            ),
            ['J'] = new
            (
                0b00110,
                0b00010,
                0b00010,
                0b00010,
                0b10010,
                0b01100
            ),
            ['K'] = new(
                0b10010,
                0b10100,
                0b11000,
                0b10100,
                0b10100,
                0b10010
            ),
            ['R'] = new(
                0b11100,
                0b10010,
                0b10010,
                0b11100,
                0b10100,
                0b10010
            ),
            ['Y'] = new(
                0b10001,
                0b10001,
                0b01010,
                0b00100,
                0b00100,
                0b00100
            ),
        };
        private readonly byte _line0, _line1, _line2, _line3, _line4, _line5;

        public static int Width => 5;
        public static int Height => 6;

        public int this[int y]
            => y switch
            {
                0 => _line0,
                1 => _line1,
                2 => _line2,
                3 => _line3,
                4 => _line4,
                5 => _line5,
                _ => throw new UnreachableException(),
            };

        private Char5x6(byte l0, byte l1, byte l2, byte l3, byte l4, byte l5)
            => (_line0, _line1, _line2, _line3, _line4, _line5) = (l0, l1, l2, l3, l4, l5);
    }

    public static string GetOCR<T>(ReadOnlySpan2D<bool> datas, int spacing)
        where T : IChar<T>
    {
        var sb = new StringBuilder();
        for (var x = 0; x < datas.Height; x += T.Width + spacing) // Width / Heigth is inversed in (RO)Span2D<>
        {
            if (TryGetChar(datas.Slice((x + spacing) * T.Width, 0, T.Width, T.Height), out var c))
                sb.Append(c);
            else
                throw new();
        }
        return sb.ToString();

        static bool TryGetChar(ReadOnlySpan2D<bool>  lines, out char c)
        {
            foreach (var kvp in T.Dictionary)
            {
                (c, var d) = kvp;
                if (CheckChar(lines, d))
                    return true;
            }
            throw new();

            static bool CheckChar(ReadOnlySpan2D<bool>  lines, T d)
            {
                for (int i = 0; i < T.Width; i++)
                    for (int j = 0; j < T.Height; j++)
                    {
                        var a = d[i, j];
                        var b = lines[i, j];
                        if (a != b)
                            return false;
                    }
                return true;
            }
        }
    }
}
