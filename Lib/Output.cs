using System.Runtime.CompilerServices;

namespace AdventOfCode;

/// <summary>
/// A <see langword="union"/> accepting <see cref="string"/>, <see cref="long"/> or <see cref="ulong"/>
/// </summary>
/// <remarks>https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/union#non-boxing-access-pattern</remarks>
[Union]
public readonly struct Output : IUnion
{
    private enum Tag : byte
    {
        None,
        String,
        Long,
        Ulong
    }

    public Output(string s)
    => (_tag, _s) = (Tag.String, s);
    public Output(long l)
    => (_tag, _l) = (Tag.Long, l);
    public Output(ulong u)
    => (_tag, _u) = (Tag.Ulong, u);

    private readonly Tag _tag = Tag.None;
    private readonly string _s;
    private readonly long _l;
    private readonly ulong _u;
    public object Value
    => _tag switch
    {
        Tag.String => _s,
        Tag.Long => _l,
        Tag.Ulong => _u,
        _ => throw new TypeAccessException(),
    };
    public bool HasValue => _tag != 0;

    public bool TryGetValue(out string value)
    {
        value = _s;
        return _tag == Tag.String;
    }

    public bool TryGetValue(out long value)
    {
        value = _l;
        return _tag == Tag.Long;
    }

    public bool TryGetValue(out ulong value)
    {
        value = _u;
        return _tag == Tag.Ulong;
    }

    public override string ToString()
    => this switch
    {
        string s => s,
        long l => l.ToString(),
        ulong l => l.ToString()
    };
}
