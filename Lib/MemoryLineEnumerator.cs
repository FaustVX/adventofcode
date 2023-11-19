#nullable enable
namespace AdventOfCode;

/// <summary>
/// Enumerates the lines of a <see cref="ReadOnlyMemory{Char}"/>.
/// </summary>
#if !LIBRARY
[DebuggerStepThrough]
#endif
public struct MemoryLineEnumerator
{
    private ReadOnlyMemory<char> _remaining;
    private ReadOnlyMemory<char> _current;
    private bool _isEnumeratorActive;

    internal MemoryLineEnumerator(ReadOnlyMemory<char> buffer)
    {
        _remaining = buffer;
        _current = default;
        _isEnumeratorActive = true;
    }

    /// <summary>
    /// Gets the line at the current position of the enumerator.
    /// </summary>
    public ReadOnlyMemory<char> Current => _current;

    /// <summary>
    /// Returns this instance as an enumerator.
    /// </summary>
    public MemoryLineEnumerator GetEnumerator() => this;

    /// <summary>
    /// Advances the enumerator to the next line of the span.
    /// </summary>
    /// <returns>
    /// True if the enumerator successfully advanced to the next line; false if
    /// the enumerator has advanced past the end of the span.
    /// </returns>
    public bool MoveNext()
    {
        if (!_isEnumeratorActive)
        {
            return false; // EOF previously reached or enumerator was never initialized
        }

        var remaining = _remaining;

        int idx = remaining.Span.IndexOfAny("\r\n");

        if ((uint)idx < (uint)remaining.Length)
        {
            int stride = 1;

            if (remaining.Span[idx] == '\r' && (uint)(idx + 1) < (uint)remaining.Length && remaining.Span[idx + 1] == '\n')
            {
                stride = 2;
            }

            _current = remaining.Slice(0, idx);
            _remaining = remaining.Slice(idx + stride);
        }
        else
        {
            // We've reached EOF, but we still need to return 'true' for this final
            // iteration so that the caller can query the Current property once more.

            _current = remaining;
            _remaining = default;
            _isEnumeratorActive = false;
        }

        return true;
    }
}
