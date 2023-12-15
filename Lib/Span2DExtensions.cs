#nullable enable
using System.Buffers;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static partial class Span2DExtensions
{

    public static bool ContainsAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => ContainsAny((ReadOnlySpan2D<T>)span, values);

    public static bool ContainsAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => ContainsAnyExcept((ReadOnlySpan2D<T>)span, values);

    public static bool Contains<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => Contains((ReadOnlySpan2D<T>)span, value);

    public static (int ron, int column) IndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => IndexOfAny((ReadOnlySpan2D<T>)span, values);

    public static (int ron, int column) IndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => IndexOfAnyExcept((ReadOnlySpan2D<T>)span, values);

    public static (int ron, int column) IndexOf<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => IndexOf((ReadOnlySpan2D<T>)span, value);

    public static (int ron, int column) LastIndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => LastIndexOfAny((ReadOnlySpan2D<T>)span, values);

    public static (int ron, int column) LastIndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => LastIndexOfAnyExcept((ReadOnlySpan2D<T>)span, values);

    public static (int ron, int column) LastIndexOf<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => LastIndexOf((ReadOnlySpan2D<T>)span, value);

    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAny(values))
                return true;
        return false;
    }

    public static bool ContainsAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAnyExcept(values))
                return true;
        return false;
    }

    public static bool Contains<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).Contains(value))
                return true;
        return false;
    }

    public static (int ron, int column) IndexOfAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) IndexOfAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) IndexOf<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOf(value) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) LastIndexOfAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) LastIndexOfAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) LastIndexOf<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOf(value) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }
}
