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
}
