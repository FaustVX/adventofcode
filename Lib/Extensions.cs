using System.Runtime.CompilerServices;

namespace AdventOfCode;

public static class Extensions
{
    [DebuggerStepThrough]
    public static void Deconstruct<T>(this Span<T> span, out T head, out Span<T> tail)
    {
        head = span[0];
        tail = span[1..];
    }

    [DebuggerStepThrough]
    public static void Deconstruct<T>(this ReadOnlySpan<T> span, out T head, out ReadOnlySpan<T> tail)
    {
        head = span[0];
        tail = span[1..];
    }

    [DebuggerStepThrough]
    public static void Deconstruct(this string span, out char head, out ReadOnlySpan<char> tail)
    {
        head = span[0];
        tail = span.AsSpan(1);
    }

    [DebuggerStepThrough]
    public static void Deconstruct<T>(this T[] span, out T head, out Span<T> tail)
    {
        head = span[0];
        tail = span.AsSpan(1);
    }

    [DebuggerStepThrough]
    public static void Deconstruct<T>(this IEnumerable<T> enumarable, out T head, out IEnumerator<T> tail)
    {
        tail = enumarable.GetEnumerator();
        tail.MoveNext();
        head = tail.Current;
    }

    [DebuggerStepThrough]
    public static void Deconstruct<T>(this IEnumerator<T> enumerator, out T head, out IEnumerator<T> tail)
    {
        tail = enumerator;
        tail.MoveNext();
        head = tail.Current;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T[] ParseToArrayOfT<T>(this string input, Func<string, T> parser)
#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
        => input.SplitLine().Select(parser).ToArray();
#pragma warning restore CS0612

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static T[] ParseToArrayOfT<T>(this string[] inputs, Func<string, T> parser)
        => inputs.Select(parser).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string input, Func<string, T> parser)
#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
        => input.SplitLine().Select(parser);
#pragma warning restore CS0612

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string[] inputs, Func<string, T> parser)
        => inputs.Select(parser);

    [DebuggerStepThrough]
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

    [DebuggerStepThrough]
    public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        => new(source);

    [DebuggerStepThrough]
    public static IReadOnlyDictionary<(int x, int y), T> ToDictionary<T>(this T[,] array)
    {
        var (w, h) = (array.GetLength(0), array.GetLength(1));
        return Enumerable.Range(0, w)
            .SelectMany(x => Enumerable.Range(0, h).Select(y => ((x, y), array[x, y])))
            .ToDictionary(static t => t.Item1, static t => t.Item2);
    }

    [DebuggerStepThrough]
    public static IEnumerable<(TFirst first, TSecond second)> ZipRepeated<TFirst, TSecond>(this IEnumerable<TFirst> source, TSecond other)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            yield return (enumerator.Current, other);
    }

    [DebuggerStepThrough]
    public static IEnumerable<TResult> ZipRepeated<TFirst, TSecond, TResult>(this IEnumerable<TFirst> source, TSecond other, Func<TFirst, TSecond, TResult> converter)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            yield return converter(enumerator.Current, other);
    }

    [DebuggerStepThrough]
    public static IEnumerable<T> Loop<T>(this IEnumerable<T> source)
    {
        while (true)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }

    [DebuggerStepThrough]
    public static (T, T) Add<T>(this (T, T) left, (T, T) right)
    where T : System.Numerics.IAdditionOperators<T, T, T>
    => (left.Item1 + right.Item1, left.Item2 + right.Item2);

    [DebuggerStepThrough]
    public static (T, T, T) Add<T>(this (T, T, T) left, (T, T, T) right)
    where T : System.Numerics.IAdditionOperators<T, T, T>
    => (left.Item1 + right.Item1, left.Item2 + right.Item2, left.Item3 + right.Item3);

    [DebuggerStepThrough]
    public static (T1 x, T2 y) ParseToTuple<T1, T2>(this ReadOnlyMemory<ReadOnlyMemory<char>> memory)
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default));

    [DebuggerStepThrough]
    public static (T1 x, T2 y, T3 z) ParseToTuple<T1, T2, T3>(this ReadOnlyMemory<ReadOnlyMemory<char>> memory)
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    where T3 : ISpanParsable<T3>
    => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default), T3.Parse(memory.Span[2].Span, default));

    [DebuggerStepThrough]
    public static void SetMinMax<T>(this T value, ref T min, ref T max)
    where T : System.Numerics.IComparisonOperators<T, T, bool>
    {
        if (value < min)
            min = value;
        else if (value > max)
            max = value;
    }

    [DebuggerStepThrough]
    public static void SetMinMaxBy<T, TValue>(this T value, ref T min, ref T max, Func<T, TValue> getValue)
    where TValue : System.Numerics.IComparisonOperators<TValue, TValue, bool>
    {
        var v = getValue(value);
        if (v < getValue(min))
            min = value;
        else if (v > getValue(max))
            max = value;
    }

    [DebuggerStepThrough]
    public static void SetMin<T>(this T value, ref T max)
    where T : System.Numerics.IComparisonOperators<T, T, bool>
    {
        if (value < max)
            max = value;
    }

    [DebuggerStepThrough]
    public static void SetMax<T>(this T value, ref T max)
    where T : System.Numerics.IComparisonOperators<T, T, bool>
    {
        if (value > max)
            max = value;
    }

    [DebuggerStepThrough]
    public static void SetMinBy<T, TValue>(this T value, ref T max, Func<T, TValue> getValue)
    where TValue : System.Numerics.IComparisonOperators<TValue, TValue, bool>
    {
        if (getValue(value) < getValue(max))
            max = value;
    }

    [DebuggerStepThrough]
    public static void SetMaxBy<T, TValue>(this T value, ref T max, Func<T, TValue> getValue)
    where TValue : System.Numerics.IComparisonOperators<TValue, TValue, bool>
    {
        if (getValue(value) > getValue(max))
            max = value;
    }

    [DebuggerStepThrough]
    public static (T1 minX, T1 maxX, T2 minY, T2 maxY) GetMinMax<T1, T2>(this IEnumerable<(T1 x, T2 y)> values)
    where T1 : System.Numerics.IMinMaxValue<T1>, System.Numerics.IComparisonOperators<T1, T1, bool>
    where T2 : System.Numerics.IMinMaxValue<T2>, System.Numerics.IComparisonOperators<T2, T2, bool>
    {
        (var minX, var maxX, var minY, var maxY) = (T1.MaxValue, T1.MinValue, T2.MaxValue, T2.MinValue);
        foreach (var (x, y) in values)
        {
            x.SetMinMax(ref minX, ref maxX);
            y.SetMinMax(ref minY, ref maxY);
        }
        return (minX, maxX, minY, maxY);
    }

    [DebuggerStepThrough]
    public static (T1 minX, T1 maxX, T2 minY, T2 maxY, T3 minZ, T3 maxZ) GetMinMax<T1, T2, T3>(this IEnumerable<(T1 x, T2 y, T3 z)> values)
    where T1 : System.Numerics.IMinMaxValue<T1>, System.Numerics.IComparisonOperators<T1, T1, bool>
    where T2 : System.Numerics.IMinMaxValue<T2>, System.Numerics.IComparisonOperators<T2, T2, bool>
    where T3 : System.Numerics.IMinMaxValue<T3>, System.Numerics.IComparisonOperators<T3, T3, bool>
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

    [DebuggerStepThrough]
    public static IEnumerable<T> AsEnumerable<T>(this ReadOnlyMemory<T> memory)
    {
        for (int i = 0; i < memory.Length; i++)
            yield return memory.Span[i];
    }

    public static Stream GetEmbededResource(string resourceName)
    => System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
}
