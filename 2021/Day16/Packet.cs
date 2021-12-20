namespace AdventOfCode.Y2021.Day16;

interface IPacket
{
    abstract static byte TypeID { get; }
    abstract byte Version { get; }
    abstract long Evaluate();
    protected abstract static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead);
    public static IPacket Parse(ReadOnlySpan<char> packet, out int bitsRead)
    {
        var version = (byte)ParseBinary(packet[0..3]);
        var type = ParseBinary(packet[3..6]);
        packet = packet[6..];
        var result = type switch
        {
            0 => SumPacket.Parse(packet, version, out bitsRead),
            1 => ProductPacket.Parse(packet, version, out bitsRead),
            2 => MinimumPacket.Parse(packet, version, out bitsRead),
            3 => MaximumPacket.Parse(packet, version, out bitsRead),
            4 => LiteralPacket.Parse(packet, version, out bitsRead),
            5 => GreaterPacket.Parse(packet, version, out bitsRead),
            6 => LessPacket.Parse(packet, version, out bitsRead),
            7 => EqualsPacket.Parse(packet, version, out bitsRead),
        };
        bitsRead += 6;
        return result;
    }

    [DebuggerStepThrough]
    protected static int ParseBinary(ReadOnlySpan<char> input)
    {
        var value = 0;
        foreach (var item in input)
        {
            value <<= 1;
            value += item is '1' ? 1 : 0;
        }
        return value;
    }
}

sealed class LiteralPacket : IPacket
{
    public static byte TypeID => 4;

    public byte Version { get; init; }

    public long Value { get; init; }

    public long Evaluate()
        => Value;

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var value = 0;
        bitsRead = 0;
        do
        {
            value <<= 4;
            value += IPacket.ParseBinary(packet.Slice(bitsRead + 1, 4));
            bitsRead += 5;
        } while (packet[bitsRead - 5] is '1');

        return new LiteralPacket
        {
            Version = version,
            Value = value,
        };
    }
}

class Boxed<T>
{
    public T Value;
}

interface IOperatorPacket : IPacket
{
    abstract IReadOnlyList<IPacket> InnerPacket { get; }
    protected static IEnumerable<IPacket> ParseInner(ReadOnlySpan<char> packet, Boxed<int> bitsRead)
    {
            var read = bitsRead.Value;
            (var l, packet) = packet;
            var packets = new List<IPacket>();
            switch (l)
            {
                case '0':
                {
                    read += 15;
                    var length = read + ParseBinary(packet[..read]);
                    while (read < length)
                    {
                        packets.Add(Parse(packet[read..], out var bits));
                        read += bits;
                    }
                    break;
                }
                case '1':
                {
                    read += 11;
                    var length = ParseBinary(packet[..read]);
                    for (int i = 0; i < length; i++)
                    {
                        packets.Add(Parse(packet[read..], out var bits));
                        read += bits;
                    }
                    break;
                }
                default:
                    throw new();
            }
            bitsRead.Value = read + 1;
            return packets;
    }
}

sealed class SumPacket : IOperatorPacket
{
    public static byte TypeID => 0;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket.Sum(static packet => packet.Evaluate());

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new SumPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class ProductPacket : IOperatorPacket
{
    public static byte TypeID => 1;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket.Aggregate(1L, static (acc, packet) => packet.Evaluate() * acc);

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new ProductPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class MinimumPacket : IOperatorPacket
{
    public static byte TypeID => 2;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket.Min(static packet => packet.Evaluate());

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new MinimumPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class MaximumPacket : IOperatorPacket
{
    public static byte TypeID => 3;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket.Max(static packet => packet.Evaluate());

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new MaximumPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class GreaterPacket : IOperatorPacket
{
    public static byte TypeID => 5;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket[0].Evaluate() > InnerPacket[1].Evaluate() ? 1 : 0;

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new GreaterPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class LessPacket : IOperatorPacket
{
    public static byte TypeID => 6;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket[0].Evaluate() < InnerPacket[1].Evaluate() ? 1 : 0;

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new LessPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}

sealed class EqualsPacket : IOperatorPacket
{
    public static byte TypeID => 7;

    public IReadOnlyList<IPacket> InnerPacket { get; init; }

    public byte Version { get; init; }

    public long Evaluate()
        => InnerPacket[0].Evaluate() == InnerPacket[1].Evaluate() ? 1 : 0;

    public static IPacket Parse(ReadOnlySpan<char> packet, byte version, out int bitsRead)
    {
        var box = new Boxed<int>();
        var result =  new EqualsPacket
        {
            Version = version,
            InnerPacket = IOperatorPacket.ParseInner(packet, box).ToList(),
        };
        bitsRead = box.Value;
        return result;
    }
}
