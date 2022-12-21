#nullable enable
namespace AdventOfCode.Y2022.Day20;

[ProblemName("Grove Positioning System")]
public class Solution : Solver //, IDisplay
{
    delegate T Parse<T>(ReadOnlySpan<char> input);
    private sealed class Node<T>
    {
        public required T Value { get; init; }
        public required LinkedListNode<Node<T>>? Next { get; init; }

        public override string ToString()
        => $"{Value} -> [{Next?.Value?.Value?.ToString()}]";
    }

    public object PartOne(string input)
    {
        var linked = ParseInput(input.AsMemory().SplitLine(), static span => int.Parse(span));
        var first = linked.First!;
        Mix(linked, first);
        RotateList(linked, static n => n.Value == 0);
        var list = linked.ToArray();
        return list[1000 % list.Length].Value + list[2000 % list.Length].Value + list[3000 % list.Length].Value;
    }

    private static void Mix<T>(LinkedList<Node<T>> linked, LinkedListNode<Node<T>> first)
    where T : System.Numerics.INumber<T>, System.Numerics.IModulusOperators<T, T, T>
    {
        var count = T.CreateChecked(linked.Count - 1);
        for (var node = first; node != null; node = node.Value.Next)
            if (!T.IsZero(node.Value.Value))
            {
                var value = node.Value.Value % count;
                if (T.IsNegative(node.Value.Value))
                    value += count;
                var after = node.Next ?? linked.First!;
                linked.Remove(node);
                for (var i = T.One; i < value; i++)
                    after = after?.Next ?? linked.First!;
                linked.AddAfter(after!, node);
            }
    }

    private static void RotateList<T>(LinkedList<T> list, Func<T, bool> predicate)
    {
        while (!predicate(list.First!.Value))
        {
            var first = list.First!;
            list.RemoveFirst();
            list.AddLast(first);
        }
    }

    private static LinkedList<Node<T>> ParseInput<T>(ReadOnlyMemory<ReadOnlyMemory<char>> input, Parse<T> parse)
    {
        var list = new LinkedList<Node<T>>();
        LinkedListNode<Node<T>>? next = default;
        for (var i = input.Length - 1; i >= 0 ; i--)
            next = list.AddFirst(new Node<T>() { Value = parse(input.Span[i].Span), Next = next });
        return list;
    }

    public object PartTwo(string input)
    {
        var linked = ParseInput(input.AsMemory().SplitLine(), static span => long.Parse(span) * 811_589_153);
        var first = linked.First!;
        for (var i = 0; i < 10; i++)
            Mix(linked, first);
        RotateList(linked, static n => n.Value == 0);
        var list = linked.ToArray();
        return list[1000 % list.Length].Value + list[2000 % list.Length].Value + list[3000 % list.Length].Value;
    }
}
