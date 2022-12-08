namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    => Execute(input, 4);

    public object PartTwo(string input)
    => Execute(input, 14);

    public object Execute(ReadOnlySpan<char> input, int length)
    {
        for (int i = 0; i < input.Length; i++)
            if (IsUnique(input.Slice(i, length)))
                return i + length;
        throw new UnreachableException();
    }

    private static bool IsUnique(ReadOnlySpan<char> span)
    {
        var set = new HashSet<char>(span.Length);
        foreach (var letter in span)
            if (!set.Add(letter))
                return false;
        return true;
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Display", Display);
    }

    private void Display(string input)
    {
        Console.CursorVisible = false;
        if (input.Length <= Console.WindowWidth)
            DisplayShort(input, 14);
        else
            DisplayLong(input, 14);
        Console.CursorVisible = true;
        Console.WriteLine();

        static void DisplayShort(string input, int length)
        {
            var left = Console.CursorLeft;
            for (int i = 0; i < input.Length - length; i++)
            {
                Console.CursorLeft = left;
                Console.Write(input.AsMemory(0, i));
                (var back, Console.BackgroundColor) = (Console.BackgroundColor, ConsoleColor.Gray);
                (var fore, Console.ForegroundColor) = (Console.ForegroundColor, IsUnique(input.AsSpan(i, length)) ? ConsoleColor.Green : ConsoleColor.Red);
                Console.Write(input.AsMemory(i, length));
                Console.ForegroundColor = fore;
                Console.BackgroundColor = back;
                Console.Write(input.AsMemory(i + length));
                Thread.Sleep(TimeSpan.FromSeconds(.15));
                if (IsUnique(input.AsSpan(i, length)))
                    return;
            }
        }

        static void DisplayLong(string input, int length)
        {
            var left = Console.CursorLeft;
            var middle = Console.WindowWidth / 2 - 2;
            var size = input.Length;
            input = new string(' ', middle) + input;
            for (int i = 0; i < size - length; i++)
            {
                Console.CursorLeft = left;
                var span = input.AsMemory(i, Console.WindowWidth);
                Console.Clear();
                Console.Write(span.Slice(0, middle));
                (var back, Console.BackgroundColor) = (Console.BackgroundColor, ConsoleColor.Gray);
                (var fore, Console.ForegroundColor) = (Console.ForegroundColor, IsUnique(span.Slice(middle, length).Span) ? ConsoleColor.Green : ConsoleColor.Red);
                Console.Write(span.Slice(middle, length));
                Console.ForegroundColor = fore;
                Console.BackgroundColor = back;
                Console.Write(span.Slice(middle + length));
                Thread.Sleep(TimeSpan.FromSeconds(.02));
                if (IsUnique(span.Slice(middle, length).Span))
                    return;
            }
        }
    }
}
