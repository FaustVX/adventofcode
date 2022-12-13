#nullable enable
namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var pairs = ParsePairs(input.AsMemory().Split2Lines());
        return pairs
            .Select(static (pair, i) => pair.left.IsOrdered(pair.right) is true ? i + 1 : 0)
            .Sum();
    }

    private List<(List left, List right)> ParsePairs(Memory<ReadOnlyMemory<char>> pairs)
    {
        var packets = new List<(List left, List right)>(capacity: pairs.Length);
        foreach (var pair in pairs.Span)
        {
            var p = pair.SplitLine().Span;
            var left = List.Parse(p[0].Span, out _);
            var right = List.Parse(p[1].Span, out _);
            packets.Add((left, right));
        }
        return packets;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}

public interface IPacket
{
    public abstract bool? IsOrdered(IPacket right);
}

public sealed class List : IPacket
{
    public List(IReadOnlyList<IPacket> packets)
    {
        Packets = packets;
    }

    public IReadOnlyList<IPacket> Packets { get; }

    public static List Parse(ReadOnlySpan<char> input, out int length)
    {
        var packets = new List<IPacket>();
        length = 1;
        while (true)
            switch (input[length])
            {
                case ',':
                    length++;
                    break;
                case ']':
                    length++;
                    return new(packets);
                case '[':
                    {
                        packets.Add(Parse(input[length..], out var used));
                        length += used;
                        break;
                    }
                default:
                    {
                        packets.Add(Int.Parse(input[length..], out var used));
                        length += used;
                        break;
                    }
            }
    }

    public bool? IsOrdered(IPacket right)
    {
        if (right is List l)
        {
            for (int i = 0; i < Packets.Count; i++)
            {
                if (l.Packets.Count <= i)
                    return false;
                var isOrdered = Packets[i].IsOrdered(l.Packets[i]);
                if (isOrdered != null)
                    return isOrdered;
            }
            if (Packets.Count == l.Packets.Count)
                return null;
            return true;
        }
        else if (right is Int i)
            return IsOrdered(new List(new List<IPacket>() { i }));
        throw new UnreachableException();
    }
}

public sealed class Int : IPacket
{
    public Int(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Int Parse(ReadOnlySpan<char> input, out int length)
    {
        var last = 0;
        for (length = 1; int.TryParse(input[..length], out var o); length++)
            last = o;
        length--;
        return new(last);
    }

    public bool? IsOrdered(IPacket right)
    {
        if (right is Int i)
        {
            if (Value < i.Value)
                return true;
            if (Value > i.Value)
                return false;
            return null;
        }
        if (right is List l)
            return new List(new List<IPacket>() { this }).IsOrdered(l);
        throw new UnreachableException();
    }
}
