#nullable enable
namespace AdventOfCode.Y2022.Day13;

static class Ext
{
    public static int Indent { get; set; }
    public static void WriteLine(string text)
    {
        if (Globals.CurrentRunMode is Mode.Display)
            Console.WriteLine(string.Concat(Enumerable.Repeat("  ", Indent)) + text);
    }
}

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
        Ext.WriteLine($"- Compare {this} vs {right}");
        Ext.Indent++;
        try
        {
            if (right is List l)
            {
                for (int i = 0; i < Packets.Count; i++)
                {
                    if (l.Packets.Count <= i)
                    {
                        Ext.WriteLine("- Right side ran out of items, so inputs are not in the right order");
                        return false;
                    }
                    var isOrdered = Packets[i].IsOrdered(l.Packets[i]);
                    if (isOrdered != null)
                        return isOrdered;
                }
                if (Packets.Count == l.Packets.Count)
                    return null;
                Ext.WriteLine("- Left side ran out of items, so inputs are in the right order");
                return true;
            }
            else if (right is Int i)
            {
                Ext.WriteLine($"- Mixed types; convert right to [{right}] and retry comparison");
                return IsOrdered(new List(new List<IPacket>() { i }));
            }
            throw new UnreachableException();
        }
        finally
        {
            Ext.Indent--;
        }
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
        Ext.WriteLine($"- Compare {this} vs {right}");
        Ext.Indent++;
        try
        {
            if (right is Int i)
            {
                if (Value < i.Value)
                {
                    Ext.WriteLine("- Left side is smaller, so inputs are in the right order");
                    return true;
                }
                if (Value > i.Value)
                {
                    Ext.WriteLine("- Right side is smaller, so inputs are not in the right order");
                    return false;
                }
                return null;
            }
            if (right is List l)
            {
                Ext.WriteLine($"- Mixed types; convert left to [{this}] and retry comparison");
                return new List(new List<IPacket>() { this }).IsOrdered(l);
            }
            throw new UnreachableException();
        }
        finally
        {
            Ext.Indent--;
        }
    }

    public override string ToString()
        => Value.ToString();
}
