#nullable enable
using System.Buffers;
using System.Runtime.InteropServices;

namespace AdventOfCode;

[StructLayout(LayoutKind.Auto)]
public readonly partial struct ArrayRental<T>([Field]T[] array, [Field]int length)
{
    public static Func<ArrayPool<T>> PoolGenerator { get; set; } = static () => ArrayPool<T>.Shared;
    private static readonly Lazy<ArrayPool<T>> _pool = new(PoolGenerator);

    public readonly Span<T> Span => _array.AsSpan(0, _length);

    public static ArrayRental<T> Rent(int length)
    => new(_pool.Value.Rent(length), length);
    public readonly void Return()
    => _pool.Value.Return(_array);
}
