using System;
using System.Collections.Generic;

namespace AdventOfCode;

public class DefaultableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Func<TValue> _defaultValue;
    public DefaultableDictionary(int capacity, TValue defaultValue = default)
        : base(capacity)
        => _defaultValue = () => defaultValue;

    public DefaultableDictionary(Func<TValue> create)
        : base()
        => _defaultValue = create;

    public new TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var value))
                return value;
            return base[key] = _defaultValue();
        }
        set => base[key] = value;
    }
}
