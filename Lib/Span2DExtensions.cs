#nullable enable
using System.Buffers;

namespace AdventOfCode;

#if !LIBRARY
[DebuggerStepThrough]
#endif
public static partial class Span2DExtensions
{
    /// <inheritdoc cref="ContainsAny{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static bool ContainsAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => ContainsAny((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="ContainsAnyExcept{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static bool ContainsAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => ContainsAnyExcept((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="Contains{T}(ReadOnlySpan2D{T}, T)"/>
    public static bool Contains<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => Contains((ReadOnlySpan2D<T>)span, value);

    /// <inheritdoc cref="IndexOfAny{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static (int row, int column) IndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => IndexOfAny((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="IndexOfAnyExcept{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static (int row, int column) IndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => IndexOfAnyExcept((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="IndexOf{T}(ReadOnlySpan2D{T}, T)"/>
    public static (int row, int column) IndexOf<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => IndexOf((ReadOnlySpan2D<T>)span, value);

    /// <inheritdoc cref="LastIndexOfAny{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static (int row, int column) LastIndexOfAny<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => LastIndexOfAny((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="LastIndexOfAnyExcept{T}(ReadOnlySpan2D{T}, SearchValues{T})"/>
    public static (int row, int column) LastIndexOfAnyExcept<T>(this Span2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    => LastIndexOfAnyExcept((ReadOnlySpan2D<T>)span, values);

    /// <inheritdoc cref="LastIndexOf{T}(ReadOnlySpan2D{T}, T)"/>
    public static (int row, int column) LastIndexOf<T>(this Span2D<T> span, T value)
    where T : IEquatable<T>?
    => LastIndexOf((ReadOnlySpan2D<T>)span, value);

    /// <inheritdoc cref="System.MemoryExtensions.ContainsAny{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static bool ContainsAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAny(values))
                return true;
        return false;
    }

    /// <inheritdoc cref="System.MemoryExtensions.ContainsAnyExcept{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static bool ContainsAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).ContainsAnyExcept(values))
                return true;
        return false;
    }

    /// <inheritdoc cref="System.MemoryExtensions.Contains{T}(ReadOnlySpan{T}, T)"/>
    public static bool Contains<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).Contains(value))
                return true;
        return false;
    }

    /// <returns>The first index of any of the specified values, or (-1, -1) if none are found.</returns>
    /// <inheritdoc cref="System.MemoryExtensions.IndexOfAny{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static (int row, int column) IndexOfAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    /// <returns>The index in the span of the first occurrence of any value other than those in values. If all of the values are in values, returns (-1, -1).</returns>
    /// <inheritdoc cref="System.MemoryExtensions.IndexOfAnyExcept{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static (int row, int column) IndexOfAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    /// <returns>The index of the occurrence of the value in the span. If not found, returns (-1, -1).</returns>
    /// <inheritdoc cref="System.MemoryExtensions.IndexOf{T}(ReadOnlySpan{T}, T)"/>
    public static (int row, int column) IndexOf<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).IndexOf(value) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    /// <returns>The last index of any of the specified values, or (-1, -1) if none are found.</returns>
    /// <inheritdoc cref="System.MemoryExtensions.LastIndexOfAny{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static (int row, int column) LastIndexOfAny<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAny(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    /// <returns>The index in the span of the last occurrence of any value other than those in values. If all of the values are in values, returns (-1, -1).</returns>
    /// <inheritdoc cref="System.MemoryExtensions.LastIndexOfAnyExcept{T}(ReadOnlySpan{T}, SearchValues{T})"/>
    public static (int row, int column) LastIndexOfAnyExcept<T>(this ReadOnlySpan2D<T> span, SearchValues<T> values)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOfAnyExcept(values) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }

    /// <returns>The index of the last occurrence of the value in the span. If not found, returns (-1, -1).</returns>
    /// <inheritdoc cref="System.MemoryExtensions.LastIndexOf{T}(ReadOnlySpan{T}, T)"/>
    public static (int row, int column) LastIndexOf<T>(this ReadOnlySpan2D<T> span, T value)
    where T : IEquatable<T>?
    {
        for (int y = 0; y < span.Height; y++)
            if (span.GetRowSpan(y).LastIndexOf(value) is > -1 and var i)
                return (y, i);
        return (-1, -1);
    }
}
