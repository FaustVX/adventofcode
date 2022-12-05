namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var (map, (instructions, _)) = input.Split2Lines();
        var stacks = ParseStacks(map.SplitLine());
        foreach (var (qty, from, to) in ParseInstruction(instructions.SplitLine()))
            for (int i = 0; i < qty; i++)
                stacks[to].Push(stacks[from].Pop());
        return string.Concat(stacks.Select(static stack => stack.Peek()));
    }

    Stack<char>[] ParseStacks(string[] lines)
    {
        var capacity = (lines[0].Length + 1) / 4;
        var stacks = new Stack<char>[capacity];
        for (int i = 0; i < capacity; i++)
            stacks[i] = new();

        foreach (var line in lines)
            for (int i = 0; i < capacity; i++)
                switch (line.AsSpan(i * 4, 3))
                {
                    case ['[', var c, ']']:
                        stacks[i].Push(c);
                        break;
                    case not [' ', ' ', ' ']:
                        for (int j = 0; j < capacity; j++)
                            stacks[j] = new(stacks[j]); // Reverse the stacks
                        return stacks;
                }
        throw new UnreachableException();
    }

    IEnumerable<(int qty, int from, int to)> ParseInstruction(string[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i].AsMemory(5);
            var space = line.Span.IndexOf(' ');
            var qty = int.Parse(line[..space].Span);
            line = line[(space + 6)..];
            space = line.Span.IndexOf(' ');
            var from = int.Parse(line[..space].Span) - 1;
            line = line[(space + 4)..];
            var to = int.Parse(line.Span) - 1;
            yield return (qty, from, to);
        }
    }

    public object PartTwo(string input)
    {
        var (map, (instructions, _)) = input.Split2Lines();
        var stacks = ParseStacks(map.SplitLine());
        foreach (var (qty, from, to) in ParseInstruction(instructions.SplitLine()))
            if (qty == 1)
                stacks[to].Push(stacks[from].Pop());
            else
            {
                var queue = new Stack<char>(qty);
                for (int i = 0; i < qty; i++)
                    queue.Push(stacks[from].Pop());
                for (int i = 0; i < qty; i++)
                    stacks[to].Push(queue.Pop());
            }
        return string.Concat(stacks.Select(static stack => stack.Peek()));
    }
}
