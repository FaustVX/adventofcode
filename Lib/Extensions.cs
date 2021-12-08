using System;
using System.Linq;
using System.Runtime.CompilerServices;

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
}