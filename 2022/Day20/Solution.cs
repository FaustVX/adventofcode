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
        var linked = ParseInput(input.AsMemory().SplitLine());
        for (var node = linked.First; node != null; node = node.Value.Next)
        {
            if (node.Value.Value > 0)
            {
                var after = node.Next ?? linked.First!;
                linked.Remove(node);
                for (int i = 1; i < node.Value.Value; i++)
                    after = after?.Next ?? linked.First!;
                if (after.Next is null)
                    linked.AddFirst(node);
                else
                    linked.AddAfter(after!, node);
            }
            else if (node.Value.Value < 0)
            {
                var before = node.Previous ?? linked.Last!;
                linked.Remove(node);
                for (var i = 1; i < -node.Value.Value; i++)
                    before = before?.Previous ?? linked.Last!;
                if (before!.Previous is null)
                    linked.AddLast(node);
                else
                    linked.AddBefore(before!, node);
            }
        }

        var list = RotateList(linked, static n => n.Value == 0).ToArray();
        return list[1000 % list.Length].Value + list[2000 % list.Length].Value + list[3000 % list.Length].Value;

        static LinkedList<T> RotateList<T>(LinkedList<T> list, Func<T, bool> predicate)
        {
            while (!predicate(list.First!.Value))
            {
                var first = list.First!;
                list.RemoveFirst();
                list.AddLast(first);
            }
            return list;
        }
    }

    private static LinkedList<Node<int>> ParseInput(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        var list = new LinkedList<Node<int>>();
        LinkedListNode<Node<int>>? next = default;
        for (int i = input.Length - 1; i >= 0 ; i--)
            next = list.AddFirst(new Node<int>() { Value = int.Parse(input.Span[i].Span), Next = next });
        return list;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
