namespace AdventOfCode;

public class DefaultableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly Func<TValue> _defaultValue;
    public DefaultableDictionary(int capacity, TValue defaultValue = default)
        : base(capacity)
        => _defaultValue = () => defaultValue;
    public DefaultableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, TValue defaultValue = default)
        : base(collection)
        => _defaultValue = () => defaultValue;

    public DefaultableDictionary(Func<TValue> create)
        : base()
        => _defaultValue = create;

    public DefaultableDictionary(TValue defaultValue = default)
        : base()
        => _defaultValue = () => defaultValue;

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

    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        get => this[key];
        set => this[key] = value;
    }

    TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
    {
        get => this[key];
    }
}
