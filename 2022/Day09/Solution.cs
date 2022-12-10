namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    => Execute(input, 2).Count;

    public object PartTwo(string input)
    => Execute(input, 10).Count;

    private static HashSet<(int x, int y)> Execute(string input, int ropeLength)
    {
        Span<(int x, int y)> rope = stackalloc (int, int)[ropeLength];
        var visited = new HashSet<(int x, int y)>(capacity: 1300)
        {
            (0, 0),
        };
        foreach (var move in input.AsMemory().SplitLine().Span)
        {
            var dir = move.Span[0] switch
            {
                'U' => (0, -1),
                'D' => (0, 1),
                'L' => (-1, 0),
                'R' => (1, 0),
                _ => throw new UnreachableException(),
            };
            for (var i = int.Parse(move.Span[2..]) - 1; i >= 0; i--)
            {
                Move(ref rope[0], rope[1..], dir);
                visited.Add(rope[^1]);
            }
        }
        return visited;
    }

    private static void Move(ref (int x, int y) head, Span<(int x, int y)> tail, (int x, int y) dir)
    {
        head.x += dir.x;
        head.y += dir.y;
        if (tail.IsEmpty)
            return;
        var offset = (x: head.x - tail[0].x, y: head.y - tail[0].y);
        if (offset.x is >= -1 and <= 1)
            if (offset.y is >= -1 and <= 1)
                return;

        dir.x = offset.x switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0,
        };
        dir.y = offset.y switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0,
        };
        Move(ref tail[0], tail[1..], dir);
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Animate", Animate);
    }

    private void Animate(string _)
    {
        var length = GetLength();
        Span<(int x, int y)> rope = stackalloc (int, int)[length];
        var visited = new HashSet<(int x, int y)>();

        Console.CursorVisible = false;
        Draw(rope, visited);

        while (true)
            switch (Console.ReadKey(intercept: true).Key)
            {
                case ConsoleKey.UpArrow:
                    Move(ref rope[0], rope[1..], (0, -1));
                    visited.Add(rope[^1]);
                    Draw(rope, visited);
                    break;
                case ConsoleKey.DownArrow:
                    Move(ref rope[0], rope[1..], (0, 1));
                    visited.Add(rope[^1]);
                    Draw(rope, visited);
                    break;
                case ConsoleKey.LeftArrow:
                    Move(ref rope[0], rope[1..], (-1, 0));
                    visited.Add(rope[^1]);
                    Draw(rope, visited);
                    break;
                case ConsoleKey.RightArrow:
                    Move(ref rope[0], rope[1..], (1, 0));
                    visited.Add(rope[^1]);
                    Draw(rope, visited);
                    break;
                case ConsoleKey.Escape:
                    Console.CursorVisible = true;
                    return;
            }

        static void Draw(ReadOnlySpan<(int x, int y)> rope, HashSet<(int x, int y)> visited)
        {
            var middle = (x: Console.WindowWidth / 2, y: Console.WindowHeight / 2);
            Console.Clear();
            foreach (var (x, y) in visited)
            {
                Console.SetCursorPosition(x + middle.x, y + middle.y);
                Console.Write('#');
            }
            for (var i = rope.Length - 1; i >= 0; i--)
            {
                var ((x, y), c) = (rope[i], i == 0 ? 'H' : (char)(i + '0'));
                Console.SetCursorPosition(x + middle.x, y + middle.y);
                Console.Write(c);
            }
        }

        static int GetLength()
        {
            var length = 10;
            Console.Clear();
            Console.WriteLine($"Rope Length: {length}");

            while (true)
                switch (Console.ReadKey(intercept: true).Key)
                {
                    case ConsoleKey.OemPlus:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.RightArrow:
                        length++;
                        Console.Clear();
                        Console.WriteLine($"Rope Length: {length}");
                        break;
                    case ConsoleKey.OemMinus:
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.LeftArrow:
                        if (length <= 2)
                            break;
                        length--;
                        Console.Clear();
                        Console.WriteLine($"Rope Length: {length}");
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        return length;
                }
        }
    }
}
