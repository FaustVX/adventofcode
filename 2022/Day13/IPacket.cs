#nullable enable
namespace AdventOfCode.Y2022.Day13;

public interface IPacket
{
    protected delegate IPacket ParseDelegate(ReadOnlySpan<char> input, out int length);
    public abstract bool? IsOrdered(IPacket right);
    public abstract string ToString();
}

public sealed class List : IPacket
{
    public sealed class Comparer : IComparer<List>
    {
        public int Compare(List? x, List? y)
        => x!.IsOrdered(y!) switch
        {
            null => 0,
            true => -1,
            false => +1,
        };
    }
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
                case var token:
                    {
                        IPacket.ParseDelegate parser = token is '[' ? Parse : Int.Parse;
                        packets.Add(parser(input[length..], out var used));
                        length += used;
                        if (input[length] is ',')
                            length++;
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

    public override string ToString()
        => $"[{string.Join(',', Packets)}]";
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

    public override string ToString()
        => Value.ToString();
}
