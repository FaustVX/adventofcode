using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AdventOfCode;

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

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArrayOfT<T>(this string input, Func<string, T> parser)
        => input.SplitLine().Select(parser).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static T[] ParseToArrayOfT<T>(this string[] inputs, Func<string, T> parser)
        => inputs.Select(parser).ToArray();

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string input, Func<string, T> parser)
        => input.SplitLine().Select(parser);

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> ParseToIEnumOfT<T>(this string[] inputs, Func<string, T> parser)
        => inputs.Select(parser);

    public static (T[,], int width, int height) Parse2D<T>(this string input, Func<char, T> creator)
    {
        var lines = input.SplitLine();
        int width = lines[0].Length;
        int height = lines.Length;
        var datas = new T[width, height];

        for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                datas[x, y] = creator(lines[y][x]);

        return (datas, width, height);
    }

    public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        => new(source);
}