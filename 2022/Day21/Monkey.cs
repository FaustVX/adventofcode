#nullable enable
namespace AdventOfCode.Y2022.Day21;

interface IMonkey
{
    public static int i = 0;
    public abstract long Value { get; }
    public abstract bool ContainsHuman { get; }
    public long GetHumanValue(long equalsTo);
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
                cache[name] = new OperationMonkey()
                {
                    Left = leftName,
                    Operation = op,
                    Right = rightName,
                    Cache = cache,
                };
            }
        }
        return cache;
    }
}

sealed class NumberMonkey : IMonkey
{
    public required long Value { get; init; }
    public bool ContainsHuman => false;

    long IMonkey.GetHumanValue(long equalsTo)
    {
        throw new NotImplementedException();
    }
}

abstract class RelationalMonkey : IMonkey
{
    public required string Left { get; init; }
    public required string Right { get; init; }
    public IMonkey LeftMonkey => Cache[Left];
    public IMonkey RightMonkey => Cache[Right];
    public required IReadOnlyDictionary<string, IMonkey> Cache { get; init; }
    public bool ContainsHuman => Cache[Left].ContainsHuman || Cache[Right].ContainsHuman;
    public abstract long Value { get; }

    public abstract long GetHumanValue(long equalsTo);
}

class OperationMonkey : RelationalMonkey
{
    public required char Operation { get; init; }
    private long? _value;
    public override long Value
    => _value ??= Operation switch
    {
        '+' => LeftMonkey.Value + RightMonkey.Value,
        '-' => LeftMonkey.Value - RightMonkey.Value,
        '*' => LeftMonkey.Value * RightMonkey.Value,
        '/' => LeftMonkey.Value / RightMonkey.Value,
        _ => throw new UnreachableException(),
    };
    public override long GetHumanValue(long equalsTo)
    {
        if (RightMonkey.ContainsHuman)
            return Operation switch
            {
                '+' => RightMonkey.GetHumanValue(equalsTo - LeftMonkey.Value),
                '-' => RightMonkey.GetHumanValue(LeftMonkey.Value - equalsTo),
                '*' => RightMonkey.GetHumanValue(equalsTo / LeftMonkey.Value),
                '/' => RightMonkey.GetHumanValue(LeftMonkey.Value / equalsTo),
                _ => throw new UnreachableException(),
            };
        else if (LeftMonkey.ContainsHuman)
            return Operation switch
            {
                '+' => LeftMonkey.GetHumanValue(equalsTo - RightMonkey.Value),
                '-' => LeftMonkey.GetHumanValue(equalsTo + RightMonkey.Value),
                '*' => LeftMonkey.GetHumanValue(equalsTo / RightMonkey.Value),
                '/' => LeftMonkey.GetHumanValue(equalsTo * RightMonkey.Value),
                _ => throw new UnreachableException(),
            };
        else
            throw new UnreachableException();
    }
}

sealed class Human : IMonkey
{
    private long? _value;
    public long Value => _value!.Value;

    public bool ContainsHuman => !_value.HasValue;

    public long GetHumanValue(long equalsTo)
    => (_value = equalsTo)!.Value;
}

sealed class RootMonkey : RelationalMonkey
{
    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public RootMonkey(OperationMonkey previous)
    => (Left, Right, Cache) = (previous.Left, previous.Right, previous.Cache);

    public override long Value => throw new NotImplementedException();
    public long GetHumanValue()
    {
        if (LeftMonkey.ContainsHuman)
            return LeftMonkey.GetHumanValue(RightMonkey.Value);
        else if (RightMonkey.ContainsHuman)
            return RightMonkey.GetHumanValue(LeftMonkey.Value);
        else
            throw new UnreachableException();
    }

    public override long GetHumanValue(long equalsTo)
    {
        throw new NotImplementedException();
    }
}
