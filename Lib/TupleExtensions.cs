namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static class TupleExtensions
{
    extension<T>((T, T) left) where T : System.Numerics.IAdditionOperators<T, T, T>
    {
        public (T, T) Add((T, T) right)
        => (left.Item1 + right.Item1, left.Item2 + right.Item2);
    }

    extension<T>((T, T, T) left) where T : System.Numerics.IAdditionOperators<T, T, T>
    {
        public (T, T, T) Add((T, T, T) right)
        => (left.Item1 + right.Item1, left.Item2 + right.Item2, left.Item3 + right.Item3);
    }

    extension<T>((T, T) tuple) where T : System.Numerics.IMultiplyOperators<T, T, T>
    {
        public (T, T) Times(T factor)
        => (tuple.Item1 * factor, tuple.Item2 * factor);
    }

    extension<T>((T, T, T) tuple) where T : System.Numerics.IMultiplyOperators<T, T, T>
    {
        public (T, T, T) Times(T factor)
        => (tuple.Item1 * factor, tuple.Item2 * factor, tuple.Item3 * factor);
    }

    extension(ReadOnlyMemory<ReadOnlyMemory<char>> memory)
    {
        public (T1 x, T2 y) ParseToTuple<T1, T2>()
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default));

        public (T1 x, T2 y, T3 z) ParseToTuple<T1, T2, T3>()
        where T1 : ISpanParsable<T1>
        where T2 : ISpanParsable<T2>
        where T3 : ISpanParsable<T3>
        => (T1.Parse(memory.Span[0].Span, default), T2.Parse(memory.Span[1].Span, default), T3.Parse(memory.Span[2].Span, default));
    }
}
