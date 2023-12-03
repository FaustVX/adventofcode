#nullable enable
using System.Buffers;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static partial class Span2DExtensions
{
    public static bool ContainsAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAnyExcept(values))
                return true;
        return false;
    }

    public static bool ContainsAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAny(values))
                return true;
        return false;
    }

    public static (int ron, int column) IndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) IndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) LastIndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static (int ron, int column) LastIndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    public static bool ContainsAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAnyExcept(values))
                return true;
        return false;
    }

    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAny(values))
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
}
