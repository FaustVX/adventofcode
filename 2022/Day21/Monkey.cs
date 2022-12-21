#nullable enable
namespace AdventOfCode.Y2022.Day21;

interface IMonkey<T>
where T : struct, ISpanParsable<T>, System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.ISubtractionOperators<T, T, T>, System.Numerics.IDivisionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T>
{
    public abstract T Value { get; }
    public static Dictionary<string, IMonkey<T>> ParseMonkeys(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        var cache = new Dictionary<string, IMonkey<T>>(capacity: input.Length);
        foreach (var line in input.AsEnumerable())
        {
            var name = line[..4].ToString();
            if (T.TryParse(line[6..].Span, null, out var value))
                cache[name] = new NumberMonkey<T>() { Value = value };
            else
            {
                var leftName = line.Slice(6, 4).ToString();
                var op = line.Span[11];
                var rightName = line.Slice(13, 4).ToString();
                cache[name] = new OperationMonkey<T>.Builder()
                {
                    Left = leftName,
                    Operation = op,
                    Right = rightName,
                };
            }
        }
        foreach (var name in cache.Keys)
            if (cache[name] is OperationMonkey<T>.Builder b)
                cache[name] = b.Build(cache);
        return cache;
    }
}

sealed class NumberMonkey<T> : IMonkey<T>
where T : struct, ISpanParsable<T>, System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.ISubtractionOperators<T, T, T>, System.Numerics.IDivisionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T>
{
    public required T Value { get; init; }
}

class OperationMonkey<T> : IMonkey<T>
where T : struct, ISpanParsable<T>, System.Numerics.IAdditionOperators<T, T, T>, System.Numerics.ISubtractionOperators<T, T, T>, System.Numerics.IDivisionOperators<T, T, T>, System.Numerics.IMultiplyOperators<T, T, T>
{
    public class Builder : IMonkey<T>
    {
        public required string Left { get; init; }
        public required char Operation { get; init; }
        public required string Right { get; init; }
        public T Value => throw new NotImplementedException();
        public IMonkey<T> Build(IDictionary<string, IMonkey<T>> cache)
        => new OperationMonkey<T>()
        {
            Left = cache[Left] is Builder l ? (cache[Left] = l.Build(cache)) : cache[Left],
            Operation = Operation,
            Right = cache[Right] is Builder r ? (cache[Right] = r.Build(cache)) : cache[Right],
        };
    }
    public required IMonkey<T> Left { get; init; }
    public required char Operation { get; init; }
    public required IMonkey<T> Right { get; init; }
    private T? _value;
    public T Value
    => _value ??= Operation switch
    {
        '+' => Left.Value + Right.Value,
        '-' => Left.Value - Right.Value,
        '*' => Left.Value * Right.Value,
        '/' => Left.Value / Right.Value,
        _ => throw new UnreachableException(),
    };
}
