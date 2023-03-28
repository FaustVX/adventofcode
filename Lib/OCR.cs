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
        abstract static implicit operator T(byte[] array);
    }

    public struct Char5x5 : IChar<Char5x5>
    {
        public static IReadOnlyDictionary<char, Char5x5> Dictionary { get; } = new Dictionary<char, Char5x5>()
        {
            ['O'] = new byte[5]
            {
                0b11111,
                0b10001,
                0b10001,
                0b10001,
                0b11111,
            },
        };

        public static int Width => 5;

        public static int Height => 5;

        public int this[int y]
            => y switch
            {
                0 => data0,
                1 => data1,
                2 => data2,
                3 => data3,
                4 => data4,
                _ => throw new UnreachableException(),
            };

        private readonly byte data0, data1, data2, data3, data4;

        public Char5x5(byte d0, byte d1, byte d2, byte d3, byte d4)
            => (data0, data1, data2, data3, data4) = (d0, d1, d2, d3, d4);

        public static implicit operator Char5x5(byte[] array)
            => new(array[0], array[1], array[2], array[3], array[4]);
    }

    public struct Char4x6 : IChar<Char4x6>
    {
        public static IReadOnlyDictionary<char, Char4x6> Dictionary { get; } = new Dictionary<char, Char4x6>()
        {
            ['A'] = new byte[6]
            {
                0b0110,
                0b1001,
                0b1001,
                0b1111,
                0b1001,
                0b1001,
            },
            ['B'] = new byte[6]
            {
                0b1110,
                0b1001,
                0b1110,
                0b1001,
                0b1001,
                0b1110,
            },
            ['C'] = new byte[6]
            {
                0b0110,
                0b1001,
                0b1000,
                0b1000,
                0b1001,
                0b0110,
            },
            ['G'] = new byte[6]
            {
                0b0110,
                0b1001,
                0b1000,
                0b1011,
                0b1001,
                0b0111,
            },
            ['F'] = new byte[6]
            {
                0b1111,
                0b1000,
                0b1110,
                0b1000,
                0b1000,
                0b1000,
            },
            ['H'] = new byte[6]
            {
                0b1001,
                0b1001,
                0b1111,
                0b1001,
                0b1001,
                0b1001,
            },
            ['J'] = new byte[6]
            {
                0b0011,
                0b0001,
                0b0001,
                0b0001,
                0b1001,
                0b0110,
            },
            ['K'] = new byte[6]
            {
                0b1001,
                0b1010,
                0b1100,
                0b1010,
                0b1010,
                0b1001,
            },
            ['L'] = new byte[6]
            {
                0b1000,
                0b1000,
                0b1000,
                0b1000,
                0b1000,
                0b1111,
            },
            ['P'] = new byte[6]
            {
                0b1110,
                0b1001,
                0b1001,
                0b1110,
                0b1000,
                0b1000,
            },
            ['R'] = new byte[6]
            {
                0b1110,
                0b1001,
                0b1001,
                0b1110,
                0b1010,
                0b1001,
            },
            ['U'] = new byte[6]
            {
                0b1001,
                0b1001,
                0b1001,
                0b1001,
                0b1001,
                0b0110,
            },
            ['Z'] = new byte[6]
            {
                0b1111,
                0b0001,
                0b0010,
                0b0100,
                0b1000,
                0b1111,
            },
        };
        private readonly byte data0, data1, data2, data3, data4, data5;

        public static int Width => 4;
        public static int Height => 6;

        public int this[int y]
            => y switch
            {
                0 => data0,
                1 => data1,
                2 => data2,
                3 => data3,
                4 => data4,
                5 => data5,
                _ => throw new UnreachableException(),
            };

        public Char4x6(byte d0, byte d1, byte d2, byte d3, byte d4, byte d5)
            => (data0, data1, data2, data3, data4, data5) = (d0, d1, d2, d3, d4, d5);

        public static implicit operator Char4x6(byte[] array)
            => new(array[0], array[1], array[2], array[3], array[4], array[5]);
    }

    public static string GetOCR<T>(char[][] datas, int spacing)
        where T : IChar<T>
    {
        var sb = new StringBuilder();
        for (var x = 0; x < datas[0].Length; x += T.Width + spacing)
        {
            if (TryGetChar(datas, x, out var c))
                sb.Append(c);
            else
                throw new();
        }
        return sb.ToString();

        static bool TryGetChar(char[][] lines, int x, out char c)
        {
            foreach (var kvp in T.Dictionary)
            {
                (c, var d) = kvp;
                if (CheckChar(lines, x, d))
                    return true;
            }
            throw new();

            static bool CheckChar(char[][] lines, int x, T d)
            {
                for (int i = 0; i < T.Width; i++)
                    for (int j = 0; j < T.Height; j++)
                    {
                        var a = d[i, j];
                        var b = !char.IsWhiteSpace(lines[j][i + x]);
                        if (a != b)
                            return false;
                    }
                return true;
            }
        }
    }
}
