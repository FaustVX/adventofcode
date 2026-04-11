using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static class Extensions
{
    extension<T>(Span<T> span)
    {
        public void Deconstruct(out T head, out Span<T> tail)
        {
            head = span[0];
            tail = span[1..];
        }
    }

    extension<T>(ReadOnlySpan<T> span)
    {
        public void Deconstruct(out T head, out ReadOnlySpan<T> tail)
        {
            head = span[0];
            tail = span[1..];
        }
    }

    extension(string span)
    {
        public void Deconstruct(out char head, out ReadOnlySpan<char> tail)
        {
            head = span[0];
            tail = span.AsSpan(1);
        }
    }

    extension<T>(T[] span)
    {
        public void Deconstruct(out T head, out Span<T> tail)
        {
            head = span[0];
            tail = span.AsSpan(1);
        }
    }

    extension<T>(IEnumerable<T> enumarable)
    {
        public void Deconstruct(out T head, out IEnumerator<T> tail)
        {
            tail = enumarable.GetEnumerator();
            tail.MoveNext();
            head = tail.Current;
        }
    }

    extension<T>(IEnumerator<T> enumerator)
    {
        public void Deconstruct(out T head, out IEnumerator<T> tail)
        {
            tail = enumerator;
            tail.MoveNext();
            head = tail.Current;
        }
    }

    extension(string input)
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T[] ParseToArray<T>()
        where T : IParsable<T>
        => [.. input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Select(static i => T.Parse(i, null))];

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T[] ParseToArrayOfT<T>(Func<string, T> parser)
            => [.. ParseToIEnumOfT(input, parser)];

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public IEnumerable<T> ParseToIEnumOfT<T>(Func<string, T> parser)
            => input.AsMemory().SplitLine().ToArray().Select(static m => new string(m.ToArray())).Select(parser);

        public (T[,], int width, int height) Parse2D<T>(Func<char, T> creator)
        {
            var lines = input.AsMemory().SplitLine();
            int width = lines.Span[0].Length;
            int height = lines.Length;
            var datas = new T[width, height];

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    datas[x, y] = creator(lines.Span[y].Span[x]);

            return (datas, width, height);
        }
    }

    extension(ReadOnlySpan<char> input)
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T[] ParseToArray<T>()
        where T : ISpanParsable<T>
        {
            var ranges = (stackalloc Range[System.MemoryExtensions.Count(input, '\n') + 1]);
            ranges = ranges[..input.SplitAny(ranges, ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)];
            T[] result = new T[ranges.Length];
            for (int i = 0; i < ranges.Length; i++)
                result[i] = T.Parse(input[ranges[i]], null);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public (T1[], T2[]) ParseToArray<T1, T2>()
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        {
            var ranges = (stackalloc Range[2]);
            input.Split(ranges, ['\n', '\n'], StringSplitOptions.RemoveEmptyEntries);
            return (ParseToArray<T1>(input[ranges[0]]), ParseToArray<T2>(input[ranges[1]]));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ParseToArray<T1, T2>(ref Span<T1> span1, ref Span<T2> span2)
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        {
            var ranges = (stackalloc Range[2]);
            input.Split(ranges, ['\n', '\n'], StringSplitOptions.RemoveEmptyEntries);
            span1 = ParseToArray<T1>(input[ranges[0]], span1);
            span2 = ParseToArray<T2>(input[ranges[1]], span2);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public (T1[], T2[], T3[]) ParseToArray<T1, T2, T3>()
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        where T3 : ISpanParsable<T3>
        {
            var ranges = (stackalloc Range[3]);
            input.Split(ranges, ['\n', '\n'], StringSplitOptions.RemoveEmptyEntries);
            return (ParseToArray<T1>(input[ranges[0]]), ParseToArray<T2>(input[ranges[1]]), ParseToArray<T3>(input[ranges[2]]));
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public void ParseToArray<T1, T2, T3>(ref Span<T1> span1, ref Span<T2> span2, ref Span<T3> span3)
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        where T3 : ISpanParsable<T3>
        {
            var ranges = (stackalloc Range[3]);
            input.Split(ranges, ['\n', '\n'], StringSplitOptions.RemoveEmptyEntries);
            span1 = ParseToArray<T1>(input[ranges[0]]);
            span2 = ParseToArray<T2>(input[ranges[1]]);
            span3 = ParseToArray<T3>(input[ranges[2]]);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> ParseToArray<T>(ref Span<T> span)
        where T : ISpanParsable<T>
        {
            var ranges = (stackalloc Range[System.MemoryExtensions.Count(input, '\n') + 1]);
            var length = input.SplitAny(ranges, ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            ranges = ranges[..length];
            span = span[..length];
            // T[] result = new T[values.Length];
            for (int i = 0; i < length; i++)
                span[i] = T.Parse(input[ranges[i]], null);
            return span;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public Span<T> ParseToArray<T>(Span<T> span)
        where T : ISpanParsable<T>
        {
            var ranges = (stackalloc Range[System.MemoryExtensions.Count(input, '\n') + 1]);
            var length = input.SplitAny(ranges, ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            ranges = ranges[..length];
            span = span[..length];
            // T[] result = new T[values.Length];
            for (int i = 0; i < length; i++)
                span[i] = T.Parse(input[ranges[i]], null);
            return span;
        }

        public int CountLinesWhere(CountFunction where)
        {
            var count = 0;
            foreach (var ip in input.EnumerateLines())
                if (where(ip))
                    count++;
            return count;
        }
    }

    extension(string[] inputs)
    {
        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public T[] ParseToArrayOfT<T>(Func<string, T> parser)
        => [.. ParseToIEnumOfT(inputs, parser)];

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public IEnumerable<T> ParseToIEnumOfT<T>(Func<string, T> parser)
            => inputs.Select(parser);
    }

    extension<T>(IEnumerable<T> source)
    {
        public Queue<T> ToQueue()
        => new(source);

        public IEnumerable<T> Loop()
        {
            while (true)
            {
                var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
        }
    }

    extension<T>(T[,] array)
    {
        public IReadOnlyDictionary<(int x, int y), T> ToDictionary()
        {
            var (w, h) = (array.GetLength(0), array.GetLength(1));
            return Enumerable.Range(0, w)
                .SelectMany(x => Enumerable.Range(0, h).Select(y => ((x, y), array[x, y])))
                .ToDictionary(static t => t.Item1, static t => t.Item2);
        }
    }

    extension<TFirst>(IEnumerable<TFirst> source)
    {
        public IEnumerable<(TFirst first, TSecond second)> ZipRepeated<TSecond>(TSecond other)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                yield return (enumerator.Current, other);
        }

        public IEnumerable<TResult> ZipRepeated<TSecond, TResult>(TSecond other, Func<TFirst, TSecond, TResult> converter)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                yield return converter(enumerator.Current, other);
        }
    }

    extension<T>(T value) where T : IComparisonOperators<T, T, bool>
    {
        public void SetMinMax(ref T min, ref T max)
        {
            if (value < min)
                min = value;
            else if (value > max)
                max = value;
        }

        public void SetMin(ref T max)
        {
            if (value < max)
                max = value;
        }

        public void SetMax(ref T max)
        {
            if (value > max)
                max = value;
        }
    }

    extension<T>(T) where T : IComparable<T>
    {
        public static bool operator <(T l, T r)
        => l.CompareTo(r) < 0;

        public static bool operator <=(T l, T r)
        => l.CompareTo(r) <= 0;

        public static bool operator ==(T l, T r)
        => l.CompareTo(r) == 0;

        public static bool operator !=(T l, T r)
        => l.CompareTo(r) != 0;

        public static bool operator >=(T l, T r)
        => l.CompareTo(r) >= 0;

        public static bool operator >(T l, T r)
        => l.CompareTo(r) > 0;
    }

    extension<T>(T value)
    {
        public void SetMinMaxBy<TValue>(ref T min, ref T max, Func<T, TValue> getValue)
    where TValue : IComparisonOperators<TValue, TValue, bool>
        {
            var v = getValue(value);
            if (v < getValue(min))
                min = value;
            else if (v > getValue(max))
                max = value;
        }

        public void SetMinBy<TValue>(ref T max, Func<T, TValue> getValue)
        where TValue : IComparisonOperators<TValue, TValue, bool>
        {
            if (getValue(value) < getValue(max))
                max = value;
        }

        public void SetMaxBy<TValue>(ref T max, Func<T, TValue> getValue)
        where TValue : IComparisonOperators<TValue, TValue, bool>
        {
            if (getValue(value) > getValue(max))
                max = value;
        }
    }

    extension<T1, T2>(IEnumerable<(T1 x, T2 y)> values)
        where T1 : IMinMaxValue<T1>, IComparisonOperators<T1, T1, bool>
        where T2 : IMinMaxValue<T2>, IComparisonOperators<T2, T2, bool>
    {
        public (T1 minX, T1 maxX, T2 minY, T2 maxY) GetMinMax()
        {
            (var minX, var maxX, var minY, var maxY) = (T1.MaxValue, T1.MinValue, T2.MaxValue, T2.MinValue);
            foreach (var (x, y) in values)
            {
                x.SetMinMax(ref minX, ref maxX);
                y.SetMinMax(ref minY, ref maxY);
            }
            return (minX, maxX, minY, maxY);
        }
    }

    extension<T1, T2, T3>(IEnumerable<(T1 x, T2 y, T3 z)> values)
        where T1 : IMinMaxValue<T1>, IComparisonOperators<T1, T1, bool>
        where T2 : IMinMaxValue<T2>, IComparisonOperators<T2, T2, bool>
        where T3 : IMinMaxValue<T3>, IComparisonOperators<T3, T3, bool>
    {
        public (T1 minX, T1 maxX, T2 minY, T2 maxY, T3 minZ, T3 maxZ) GetMinMax()
        {
            (var minX, var maxX, var minY, var maxY, var minZ, var maxZ) = (T1.MaxValue, T1.MinValue, T2.MaxValue, T2.MinValue, T3.MaxValue, T3.MinValue);
            foreach (var (x, y, z) in values)
            {
                x.SetMinMax(ref minX, ref maxX);
                y.SetMinMax(ref minY, ref maxY);
                z.SetMinMax(ref minZ, ref maxZ);
            }
            return (minX, maxX, minY, maxY, minZ, maxZ);
        }
    }

    extension<T>(ReadOnlyMemory<T> memory)
    {
        public IEnumerable<T> AsEnumerable()
        {
            for (int i = 0; i < memory.Length; i++)
                yield return memory.Span[i];
        }
    }

    public static Stream GetEmbededResource(string resourceName)
    => System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

    public delegate bool CountFunction(ReadOnlySpan<char> line);

    extension<T>(Span<T> span) where T : struct, Enum
    {
        /// <summary>
        /// Counts the number of times the specified value occurs in the span./// 
        /// </summary>
        /// <remarks>
        /// Similar to <see cref="System.MemoryExtensions.Count{T}(Span{T}, T)"/>, but for <see cref="Enum"/>
        /// </remarks>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value for which to search.</param>
        /// <typeparam name="T">The element type of the span.</typeparam>
        /// <returns>The number of times value was found in the span.</returns>
        public int Count(T value)
        => Count((ReadOnlySpan<T>)span, value);
    }

    extension<T>(ReadOnlySpan<T> span) where T : struct, Enum
    {
        /// <summary>
        /// Counts the number of times the specified value occurs in the span./// 
        /// </summary>
        /// <remarks>
        /// Similar to <see cref="System.MemoryExtensions.Count{T}(ReadOnlySpan{T}, T)"/>, but for <see cref="Enum"/>
        /// </remarks>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value for which to search.</param>
        /// <typeparam name="T">The element type of the span.</typeparam>
        /// <returns>The number of times value was found in the span.</returns>
        public int Count(T value)
        {
            var sum = 0;
            foreach (var item in span)
                if (EqualityComparer<T>.Default.Equals(item, value))
                    sum++;
            return sum;
        }
    }
}
