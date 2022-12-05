namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var enumerator = input.SplitLine().AsEnumerable().GetEnumerator();
        var stacks = ParseStacks(enumerator);
        foreach (var (qty, from, to) in ParseInstruction(enumerator))
            for (int i = 0; i < qty; i++)
                stacks[to].Push(stacks[from].Pop());
        return string.Concat(stacks.Select(static stack => stack.Peek()));
    }

    Stack<char>[] ParseStacks(IEnumerator<string> input)
    {
        input.MoveNext();
        var capacity = (input.Current.Length + 1) / 4;
        var stacks = new Stack<char>[capacity];
        for (int i = 0; i < capacity; i++)
            stacks[i] = new();

        for (var line = input.Current; input.MoveNext(); line = input.Current)
            for (int i = 0; i < capacity; i++)
                switch (line.AsSpan(i * 4, 3))
                {
                    case [' ', ' ', ' ']:
                        continue;
                    case ['[', >= 'A' and <= 'Z' and var c, ']']:
                        stacks[i].Push(c);
                        break;
                    default:
                        for (int j = 0; j < capacity; j++)
                            stacks[j] = new(stacks[j]); // Reverse the stacks
                        input.MoveNext();
                        return stacks;
                }
        throw new();
    }

    IEnumerable<(int qty, int from, int to)> ParseInstruction(IEnumerator<string> input)
    {
        do
        {
            var line = input.Current.AsMemory();
            line = line[5..];
            var space = line.Span.IndexOf(' ');
            var qty = int.Parse(line[..space].Span);
            line = line[(space + 6)..];
            space = line.Span.IndexOf(' ');
            var from = int.Parse(line[..space].Span) - 1;
            line = line[(space + 4)..];
            var to = int.Parse(line.Span) - 1;
            yield return (qty, from, to);
        } while (input.MoveNext());
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
