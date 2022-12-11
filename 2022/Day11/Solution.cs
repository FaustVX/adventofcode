#nullable enable
namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var monkeys = ParseMonkeys(input);
        for (int i = 0; i < 20; i++)
            Round(monkeys, level => level / 3);
        return CalculateBusinessLevel(monkeys);
    }

    private static ReadOnlySpan<Monkey> ParseMonkeys(string input)
    {
        var span = input.AsMemory().Split2Lines().Span;
        var monkeys = new Monkey[span.Length];
        for (var i = 0; i < span.Length; i++)
        {
            monkeys[i] = new(span[i]);
        }
        return monkeys;
    }

    private static void Round(ReadOnlySpan<Monkey> monkeys, Func<long, long> reduceWorryLevel)
    {
        foreach (var monkey in monkeys)
            monkey.Turn(monkeys, reduceWorryLevel);
    }

    private long CalculateBusinessLevel(ReadOnlySpan<Monkey> monkeys)
    {
        var (first, second) = (0L, 0L);
        foreach (var monkey in monkeys)
            if (monkey.InspectedItems > first)
                (first, second) = (monkey.InspectedItems, first);
            else if (monkey.InspectedItems > second)
                second = monkey.InspectedItems;
        return first * second;
    }

    public object PartTwo(string input)
    {
        // https://www.reddit.com/r/adventofcode/comments/zih7gf/comment/izrck61

        var monkeys = ParseMonkeys(input);
        var modulo = GetModulo(monkeys);
        for (int i = 0; i < 10_000; i++)
            Round(monkeys, level => level % modulo);
        return CalculateBusinessLevel(monkeys);

        static int GetModulo(ReadOnlySpan<Monkey> monkeys)
        {
            var modulo = 1;
            foreach (var monkey in monkeys)
                modulo *= monkey.Test;
            return modulo;
        }
    }
}

sealed class Monkey
{
    public Monkey(ReadOnlyMemory<char> input)
    {
        var lines = input.SplitLine().Span;

        Items = ParseItem(lines[1].Span);
        Operation = ParseOperation(lines[2].Span);
        Test = int.Parse(lines[3].Span[^2..]); // Allow for 1 or 2 digits number
        ThrowToMonkeyIfTrue = int.Parse(lines[4].Span[^2..]); // Allow for 1 or 2 digits number
        ThrowToMonkeyIfFalse = int.Parse(lines[5].Span[^2..]); // Allow for 1 or 2 digits number

        static Queue<long> ParseItem(ReadOnlySpan<char> line)
        {
            var queue = new Queue<long>();
            for (var items = line[line.IndexOf(": ")..]; !items.IsEmpty; items = items[4..])
                queue.Enqueue(long.Parse(items[2..4]));
            return queue;
        }

        static (bool isAddition, int? value) ParseOperation(ReadOnlySpan<char> line)
        {
            var equalSign = line.IndexOf('=') + 6; // 6 = "= old "
            return (line[equalSign] == '+', line.EndsWith("old") ? null : int.Parse(line[(equalSign + 2)..]));
        }
    }

    public Queue<long> Items { get; }
    public (bool isAddition, int? value) Operation { get; }
    public int Test { get; }
    public int ThrowToMonkeyIfTrue { get; }
    public int ThrowToMonkeyIfFalse { get; }
    public int InspectedItems { get; private set; }

    public void Turn(ReadOnlySpan<Monkey> monkeys, Func<long, long> reduceWorryLevel)
    {
        while (Items.TryDequeue(out var worryLevel))
        {
            InspectedItems++;
            worryLevel = Operation switch
            {
                (isAddition: true, int qty) => worryLevel + qty,
                (isAddition: false, int qty) => worryLevel * qty,
                (isAddition: true, null) => worryLevel + worryLevel,
                (isAddition: false, null) => worryLevel * worryLevel,
            };
            worryLevel = reduceWorryLevel(worryLevel);
            monkeys[worryLevel % Test == 0 ? ThrowToMonkeyIfTrue : ThrowToMonkeyIfFalse].Items.Enqueue(worryLevel);
        }
    }
}
