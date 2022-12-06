namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
class Solution : Solver, IDisplay
{
    public object PartOne(string input)
        => Execute(input, Action1);

    private static void Action1(int qty, int from, int to, Stack<char>[] stacks)
    {
        for (int i = 0; i < qty; i++)
            stacks[to].Push(stacks[from].Pop());
    }

    Stack<char>[] ParseStacks(IReadOnlyList<ReadOnlyMemory<char>> lines)
    {
        var capacity = (lines[0].Length + 1) / 4;
        var stacks = new Stack<char>[capacity];
        for (int i = 0; i < capacity; i++)
            stacks[i] = new();

        foreach (var line in lines)
            for (int i = 0; i < capacity; i++)
                switch (line.Slice(i * 4, 3).Span)
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

    IEnumerable<(int qty, int from, int to)> ParseInstruction(IEnumerable<ReadOnlyMemory<char>> input)
    {
        foreach (var line in input)
        {
            var instruction = line[5..];
            var space = instruction.Span.IndexOf(' ');
            var qty = int.Parse(instruction[..space].Span);
            instruction = instruction[(space + 6)..];
            space = instruction.Span.IndexOf(' ');
            var from = int.Parse(instruction[..space].Span) - 1;
            instruction = instruction[(space + 4)..];
            var to = int.Parse(instruction.Span) - 1;
            yield return (qty, from, to);
        }
    }

    string Execute(string input, Action<int, int, int, Stack<char>[]> action)
    {
        var groups = input.AsMemory().Split2Lines();
        var stacks = ParseStacks(groups[0].SplitLine());
        foreach (var (qty, from, to) in ParseInstruction(groups[1].SplitLine()))
            action(qty, from, to, stacks);
        return string.Concat(stacks.Select(static stack => stack.Peek()));
    }

    public object PartTwo(string input)
        => Execute(input, Action2);

    private static void Action2(int qty, int from, int to, Stack<char>[] stacks)
    {
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
    }

    private void Display(string input, Action<int, int, int, Stack<char>[]> action)
    {
        Console.CursorVisible = false;
        Console.WriteLine(Execute(input, Action));
        Console.CursorVisible = true;

        void Action(int qty, int from, int to, IEnumerable<char>[] stacks)
        {
            action(qty, from, to, (Stack<char>[])stacks);
            stacks = stacks.Select(static stack => stack.Reverse().ToArray()).ToArray();
            var totalCrates = stacks.Sum(static stack => stack.Count());
            Console.Clear();
            var back = Console.BackgroundColor;
            for (int i = totalCrates - 1; i >= 0 ; i--)
            {
                for (int s = 0; s < stacks.Length; s++)
                {
                    var stack = (char[])stacks[s];
                    if (stack.Length <= i)
                        Console.Write(' ');
                    else
                    {
                        Console.BackgroundColor
                            = s == from ? ConsoleColor.Red
                            : s == to ? ConsoleColor.Green
                            : back;
                        Console.Write(stack[i]);
                        Console.BackgroundColor = back;
                    }
                }
                Console.WriteLine();
            }
            Thread.Sleep(TimeSpan.FromSeconds(.5));
        }
    }

    public void DisplayPartOne(string input)
        => Display(input, Action1);

    public void DisplayPartTwo(string input)
        => Display(input, Action2);
}
