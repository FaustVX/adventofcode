#nullable enable
using System.Buffers;
using System.Runtime.InteropServices;

namespace AdventOfCode;

[StructLayout(LayoutKind.Auto)]
public readonly partial struct ArrayRental<T>([Field]T[] array, [Field]int length)
{
    private static readonly ArrayPool<T> _pool = ArrayPool<T>.Create(10, 1);

    public readonly Span<T> Span => _array.AsSpan(0, _length);

    public static ArrayRental<T> Rent(int length)
    => new(_pool.Rent(length), length);
    public readonly void Return()
    => _pool.Return(_array);
}
