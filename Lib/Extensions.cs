using System.Numerics;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static class Extensions
{
    public static void Deconstruct<T>(this Span<T> span, out T head, out Span<T> tail)
    {
        head = span[0];
        tail = span[1..];
    }

    public static void Deconstruct<T>(this ReadOnlySpan<T> span, out T head, out ReadOnlySpan<T> tail)
    {
        head = span[0];
        tail = span[1..];
    }

    public static void Deconstruct(this string span, out char head, out ReadOnlySpan<char> tail)
    {
        head = span[0];
        tail = span.AsSpan(1);
    }

    public static void Deconstruct<T>(this T[] span, out T head, out Span<T> tail)
    {
        head = span[0];
        tail = span.AsSpan(1);
    }

    public static void Deconstruct<T>(this IEnumerable<T> enumarable, out T head, out IEnumerator<T> tail)
    {
        tail = enumarable.GetEnumerator();
        tail.MoveNext();
        head = tail.Current;
    }

    public static void Deconstruct<T>(this IEnumerator<T> enumerator, out T head, out IEnumerator<T> tail)
    {
        tail = enumerator;
        tail.MoveNext();
        head = tail.Current;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArray<T>(this string input)
    where T : IParsable<T>
    => [.. input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries).Select(static i => T.Parse(i, null))];

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArray<T>(this ReadOnlySpan<char> input)
    where T : ISpanParsable<T>
    {
        var values = (stackalloc Range[System.MemoryExtensions.Count(input, '\n') + 1]);
        values = values[.. input.SplitAny(values, ['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)];
        T[] result = new T[values.Length];
        for (int i = 0; i < values.Length; i++)
            result[i] = T.Parse(input[values[i]], null);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArrayOfT<T>(this string input, Func<string, T> parser)
        => [.. ParseToIEnumOfT(input, parser)];

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArrayOfT<T>(this string[] inputs, Func<string, T> parser)
        => [.. ParseToIEnumOfT(inputs, parser)];

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string input, Func<string, T> parser)
        => input.AsMemory().SplitLine().ToArray().Select(static m => new string(m.ToArray())).Select(parser);

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string[] inputs, Func<string, T> parser)
        => inputs.Select(parser);

    public static (T[,], int width, int height) Parse2D<T>(this string input, Func<char, T> creator)
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

    public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        => new(source);

    public static IReadOnlyDictionary<(int x, int y), T> ToDictionary<T>(this T[,] array)
    {
        var (w, h) = (array.GetLength(0), array.GetLength(1));
        return Enumerable.Range(0, w)
            .SelectMany(x => Enumerable.Range(0, h).Select(y => ((x, y), array[x, y])))
            .ToDictionary(static t => t.Item1, static t => t.Item2);
    }

    public static IEnumerable<(TFirst first, TSecond second)> ZipRepeated<TFirst, TSecond>(this IEnumerable<TFirst> source, TSecond other)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            yield return (enumerator.Current, other);
    }

    public static IEnumerable<TResult> ZipRepeated<TFirst, TSecond, TResult>(this IEnumerable<TFirst> source, TSecond other, Func<TFirst, TSecond, TResult> converter)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            yield return converter(enumerator.Current, other);
    }

    public static IEnumerable<T> Loop<T>(this IEnumerable<T> source)
    {
        while (true)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }

    public static void SetMinMax<T>(this T value, ref T min, ref T max)
    where T : IComparisonOperators<T, T, bool>
    {
        if (value < min)
            min = value;
        else if (value > max)
            max = value;
    }

    public static void SetMinMaxBy<T, TValue>(this T value, ref T min, ref T max, Func<T, TValue> getValue)
    where TValue : IComparisonOperators<TValue, TValue, bool>
    {
        var v = getValue(value);
        if (v < getValue(min))
            min = value;
        else if (v > getValue(max))
            max = value;
    }

    public static void SetMin<T>(this T value, ref T max)
    where T : IComparisonOperators<T, T, bool>
    {
        if (value < max)
            max = value;
    }

    public static void SetMax<T>(this T value, ref T max)
    where T : IComparisonOperators<T, T, bool>
    {
        if (value > max)
            max = value;
    }

    public static void SetMinBy<T, TValue>(this T value, ref T max, Func<T, TValue> getValue)
    where TValue : IComparisonOperators<TValue, TValue, bool>
    {
        if (getValue(value) < getValue(max))
            max = value;
    }

    public static void SetMaxBy<T, TValue>(this T value, ref T max, Func<T, TValue> getValue)
    where TValue : IComparisonOperators<TValue, TValue, bool>
    {
        if (getValue(value) > getValue(max))
            max = value;
    }

    public static (T1 minX, T1 maxX, T2 minY, T2 maxY) GetMinMax<T1, T2>(this IEnumerable<(T1 x, T2 y)> values)
    where T1 : IMinMaxValue<T1>, IComparisonOperators<T1, T1, bool>
    where T2 : IMinMaxValue<T2>, IComparisonOperators<T2, T2, bool>
    {
        (var minX, var maxX, var minY, var maxY) = (T1.MaxValue, T1.MinValue, T2.MaxValue, T2.MinValue);
        foreach (var (x, y) in values)
        {
            x.SetMinMax(ref minX, ref maxX);
            y.SetMinMax(ref minY, ref maxY);
        }
        return (minX, maxX, minY, maxY);
    }

    public static (T1 minX, T1 maxX, T2 minY, T2 maxY, T3 minZ, T3 maxZ) GetMinMax<T1, T2, T3>(this IEnumerable<(T1 x, T2 y, T3 z)> values)
    where T1 : IMinMaxValue<T1>, IComparisonOperators<T1, T1, bool>
    where T2 : IMinMaxValue<T2>, IComparisonOperators<T2, T2, bool>
    where T3 : IMinMaxValue<T3>, IComparisonOperators<T3, T3, bool>
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

    public static IEnumerable<T> AsEnumerable<T>(this ReadOnlyMemory<T> memory)
    {
        for (int i = 0; i < memory.Length; i++)
            yield return memory.Span[i];
    }

    public static Stream GetEmbededResource(string resourceName)
    => System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

    public delegate bool CountFunction(ReadOnlySpan<char> line);

    public static int CountLinesWhere(this ReadOnlySpan<char> input, CountFunction where)
    {
        var count = 0;
        foreach (var ip in input.EnumerateLines())
            if (where(ip))
                count++;
        return count;
    }

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
    public static int Count<T>(this Span<T> span, T value)
    where T : struct, Enum
    => Count((ReadOnlySpan<T>)span, value);

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
    public static int Count<T>(this ReadOnlySpan<T> span, T value)
    where T : struct, Enum
    {
        var sum = 0;
        foreach (var item in span)
            if (EqualityComparer<T>.Default.Equals(item, value))
                sum++;
        return sum;
    }
}
