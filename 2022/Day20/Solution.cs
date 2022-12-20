#nullable enable
namespace AdventOfCode.Y2022.Day20;

[ProblemName("Grove Positioning System")]
public class Solution : Solver //, IDisplay
{
    private sealed class Node<T>
    {
        public required T Value { get; init; }
        public required LinkedListNode<Node<T>>? Next { get; init; }

        public override string ToString()
        => $"{Value} -> [{Next?.Value?.Value?.ToString()}]";
    }

    public object PartOne(string input)
    {
        var linked = ParseInput<int>(input.AsMemory().SplitLine());
        Mix(linked);
        RotateList(linked, static n => n.Value == 0);
        var list = linked.ToArray();
        return list[1000 % list.Length].Value + list[2000 % list.Length].Value + list[3000 % list.Length].Value;
    }

    private static void Mix<T>(LinkedList<Node<T>> linked)
    where T : System.Numerics.INumber<T>
    {
        for (var node = linked.First; node != null; node = node.Value.Next)
            if (node.Value.Value > T.Zero)
            {
                var after = node.Next ?? linked.First!;
                linked.Remove(node);
                for (var i = T.One; i < node.Value.Value; i++)
                    after = after?.Next ?? linked.First!;
                if (after.Next is null)
                    linked.AddFirst(node);
                else
                    linked.AddAfter(after!, node);
            }
            else if (node.Value.Value < T.Zero)
            {
                var before = node.Previous ?? linked.Last!;
                linked.Remove(node);
                for (var i = T.One; i < -node.Value.Value; i++)
                    before = before?.Previous ?? linked.Last!;
                if (before!.Previous is null)
                    linked.AddLast(node);
                else
                    linked.AddBefore(before!, node);
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

    private static LinkedList<Node<T>> ParseInput<T>(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    where T : ISpanParsable<T>
    {
        var list = new LinkedList<Node<T>>();
        LinkedListNode<Node<T>>? next = default;
        for (var i = input.Length - 1; i >= 0 ; i--)
            next = list.AddFirst(new Node<T>() { Value = T.Parse(input.Span[i].Span, default), Next = next });
        return list;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
