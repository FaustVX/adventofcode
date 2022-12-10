namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
        => Execute(input, CraneMover9000);

    private static void CraneMover9000(int qty, int from, int to, Stack<char>[] stacks)
    {
        for (int i = 0; i < qty; i++)
            stacks[to].Push(stacks[from].Pop());
    }

    Stack<char>[] ParseStacks(ReadOnlyMemory<ReadOnlyMemory<char>> lines)
    {
        var capacity = (lines.Span[^1].Length + 2) / 4; // ^1 and +2: if lines are trimmed
        var stacks = new Stack<char>[capacity];
        for (int i = 0; i < capacity; i++)
            stacks[i] = new();

        for (var l = lines.Length - 2; l >= 0; l--)
            for (int i = 0; i < capacity; i++)
                if (lines.Span[l].Slice(i * 4, 3).Span is ['[', var c, ']'])
                    stacks[i].Push(c);
        return stacks;
    }

    IEnumerable<(int qty, int from, int to)> ParseInstruction(ReadOnlyMemory<ReadOnlyMemory<char>> input)
    {
        for (var l = 0; l < input.Length; l++)
            if (input.Span[l].TryParseFormated<(int qty, int from, int to)>($"move {0} from {0} to {0}", out var values))
                yield return (values.qty, values.from - 1, values.to - 1);
            else
                throw new UnreachableException(input.Span[l].ToString());
    }

    string Execute(string input, Action<int, int, int, Stack<char>[]> action)
    {
        var groups = input.AsMemory().Split2Lines();
        var stacks = ParseStacks(groups.Span[0].SplitLine());
        foreach (var (qty, from, to) in ParseInstruction(groups.Span[1].SplitLine()))
            action(qty, from, to, stacks);
        return string.Concat(stacks.Select(static stack => stack.Peek()));
    }

    public object PartTwo(string input)
        => Execute(input, CraneMover9001);

    private static void CraneMover9001(int qty, int from, int to, Stack<char>[] stacks)
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

    private void DisplayPartOne(string input)
        => Display(input, CraneMover9000);

    private void DisplayPartTwo(string input)
        => Display(input, CraneMover9001);

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Part1", DisplayPartOne);
        yield return ("Part2", DisplayPartTwo);
    }
}
