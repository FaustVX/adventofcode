#nullable enable
namespace AdventOfCode.Y2022.Day21;

interface IMonkey
{
    public abstract long Value { get; }
    public static Dictionary<string, IMonkey> ParseMonkeys(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        var cache = new Dictionary<string, IMonkey>(capacity: input.Length);
        foreach (var line in input.AsEnumerable())
        {
            var name = line[..4].ToString();
            if (long.TryParse(line[6..].Span, out var value))
                cache[name] = new NumberMonkey() { Value = value };
            else
            {
                var leftName = line.Slice(6, 4).ToString();
                var op = line.Span[11];
                var rightName = line.Slice(13, 4).ToString();
                cache[name] = new OperationMonkey.Builder()
                {
                    Left = leftName,
                    Operation = op,
                    Right = rightName,
                };
            }
        }
        foreach (var name in cache.Keys)
            if (cache[name] is OperationMonkey.Builder b)
                cache[name] = b.Build(cache);
        return cache;
    }
}

sealed class NumberMonkey : IMonkey
{
    public required long Value { get; init; }
}

class OperationMonkey : IMonkey
{
    public class Builder : IMonkey
    {
        public required string Left { get; init; }
        public required char Operation { get; init; }
        public required string Right { get; init; }
        public long Value => throw new NotImplementedException();
        public IMonkey Build(IDictionary<string, IMonkey> cache)
        => new OperationMonkey()
        {
            Left = cache[Left] is Builder l ? (cache[Left] = l.Build(cache)) : cache[Left],
            Operation = Operation,
            Right = cache[Right] is Builder r ? (cache[Right] = r.Build(cache)) : cache[Right],
        };
    }
    public required IMonkey Left { get; init; }
    public required char Operation { get; init; }
    public required IMonkey Right { get; init; }
    private long? _value;
    public long Value
    => _value ??= Operation switch
    {
        '+' => Left.Value + Right.Value,
        '-' => Left.Value - Right.Value,
        '*' => Left.Value * Right.Value,
        '/' => Left.Value / Right.Value,
        _ => throw new UnreachableException(),
    };
}
