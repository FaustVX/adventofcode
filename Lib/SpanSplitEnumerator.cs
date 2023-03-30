namespace AdventOfCode;

// Copied from SpanLineEnumerator: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Text/SpanLineEnumerator.cs

[DebuggerStepThrough]
public ref struct SpanSplitEnumerator
{
    private ReadOnlySpan<char> _remaining;
    private readonly ReadOnlySpan<char> _separator;
    private readonly bool _separateOnAny;
    private ReadOnlySpan<char> _current;
    private bool _isEnumeratorActive;

    public SpanSplitEnumerator(ReadOnlySpan<char> buffer, ReadOnlySpan<char> separator, bool separateOnAny)
    {
        _remaining = buffer;
        _separator = separator;
        _separateOnAny = separateOnAny;
        _current = default;
        _isEnumeratorActive = true;
    }

    public SpanSplitEnumerator(ReadOnlySpan<char> buffer, ReadOnlySpan<char> separator)
    {
        _remaining = buffer;
        _separator = separator;
        _separateOnAny = false;
        _current = default;
        _isEnumeratorActive = true;
    }

    /// <summary>
    /// Gets the line at the current position of the enumerator.
    /// </summary>
    public ReadOnlySpan<char> Current => _current;

    /// <summary>
    /// Returns this instance as an enumerator.
    /// </summary>
    public SpanSplitEnumerator GetEnumerator() => this;

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
            return false; // EOF previously reached or enumerator was never initialized

        var idx = _separateOnAny ? _remaining.IndexOfAny(_separator) : _remaining.IndexOf(_separator);

        if ((uint)idx < (uint)_remaining.Length)
        {
            _current = _remaining.Slice(0, idx);
            _remaining = _remaining.Slice(idx + (_separateOnAny ? 1 : _separator.Length));
        }
        else
        {
            // We've reached EOF, but we still need to return 'true' for this final
            // iteration so that the caller can query the Current property once more.
            _current = _remaining;
            _remaining = default;
            _isEnumeratorActive = false;
        }

        return true;
    }
}
