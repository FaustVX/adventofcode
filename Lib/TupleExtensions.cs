namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static class TupleExtensions
{
    public static (T, T) Add<T>(this (T, T) left, (T, T) right)
    where T : System.Numerics.IAdditionOperators<T, T, T>
    => (left.Item1 + right.Item1, left.Item2 + right.Item2);

    public static (T, T, T) Add<T>(this (T, T, T) left, (T, T, T) right)
    where T : System.Numerics.IAdditionOperators<T, T, T>
    => (left.Item1 + right.Item1, left.Item2 + right.Item2, left.Item3 + right.Item3);

    public static (T, T) Times<T>(this (T, T) tuple, T factor)
    where T : System.Numerics.IMultiplyOperators<T, T, T>
    => (tuple.Item1 * factor, tuple.Item2 * factor);

    public static (T, T, T) Times<T>(this (T, T, T) tuple, T factor)
    where T : System.Numerics.IMultiplyOperators<T, T, T>
    => (tuple.Item1 * factor, tuple.Item2 * factor, tuple.Item3 * factor);

    public static (T1 x, T2 y) ParseToTuple<T1, T2>(this ReadOnlyMemory<ReadOnlyMemory<char>> memory)
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default));

    public static (T1 x, T2 y, T3 z) ParseToTuple<T1, T2, T3>(this ReadOnlyMemory<ReadOnlyMemory<char>> memory)
    where T1 : ISpanParsable<T1>
    where T2 : ISpanParsable<T2>
    where T3 : ISpanParsable<T3>
    => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default), T3.Parse(memory.Span[2].Span, default));
}
