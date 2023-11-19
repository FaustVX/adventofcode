using System.Runtime.InteropServices;

namespace AdventOfCode;

// Copied from SpanLineEnumerator: https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/Text/SpanLineEnumerator.cs

[DebuggerStepThrough, StructLayout(LayoutKind.Auto)]
public readonly ref partial struct SpanSplitEnumerator([Field] ReadOnlySpan<char> buffer, [Field] ReadOnlySpan<char> separator, [Field] bool separateOnAny)
{
    public SpanSplitEnumerator(ReadOnlySpan<char> buffer, ReadOnlySpan<char> separator)
    : this(buffer, separator, false)
    { }

    public readonly int Length
    {
        get
        {
            var count = 0;
            var e = GetEnumerator();
            while (e.MoveNext())
                count++;
            return count;
        }
    }

    public readonly Enumerator GetEnumerator()
    => new(this);

    public readonly ReadOnlySpan<char> this[int index]
    {
        get
        {
            var e = GetEnumerator();
            e.MoveNext();
            for (var i = 0; i < index; i++)
                if (!e.MoveNext())
                    throw new IndexOutOfRangeException();
            return e.Current;
        }
    }

    public ref struct Enumerator([DontUse] SpanSplitEnumerator enumerator)
    {
        private ReadOnlySpan<char> _remaining = enumerator._buffer;
        private readonly ReadOnlySpan<char> _separator = enumerator._separator;
        private readonly bool _separateOnAny = enumerator._separateOnAny;
        private ReadOnlySpan<char> _current = default;
        private bool _isEnumeratorActive = true;

        /// <summary>
        /// Gets the line at the current position of the enumerator.
        /// </summary>
        public readonly ReadOnlySpan<char> Current => _current;

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
                _current = _remaining[..idx];
                _remaining = _remaining[(idx + (_separateOnAny ? 1 : _separator.Length))..];
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
}
