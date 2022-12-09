namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var head = (x: 0, y: 0);
        var tail = head;
        var visited = new HashSet<(int x, int y)>()
        {
            tail,
        };
        foreach (var move in input.AsMemory().SplitLine())
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
                Move(ref head, ref tail, visited, dir);
        }
        return visited.Count;

        static void Move(ref (int x, int y) head, ref (int x, int y) tail, HashSet<(int x, int y)> visited, (int x, int y) dir)
        {
            var previousHead = head;
            head.x += dir.x;
            head.y += dir.y;
            if (head.x - tail.x is >= -1 and <= 1 && head.y - tail.y is >= -1 and <= 1)
                return;
            tail = previousHead;
            visited.Add(tail);
        }
    }

    public object PartTwo(string input)
    {
        Span<(int x, int y)> rope = stackalloc (int, int)[10];
        var visited = new HashSet<(int x, int y)>()
        {
            (0, 0),
        };
        foreach (var move in input.AsMemory().SplitLine())
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
        return visited.Count;

        static void Move(ref (int x, int y) head, Span<(int x, int y)> tail, (int x, int y) dir)
        {
            if (dir is (0, 0))
                return;
            var knot = head;
            head.x += dir.x;
            head.y += dir.y;
            if (tail.IsEmpty)
                return;
            if (head.x - tail[0].x is >= -1 and <= 1 && head.y - tail[0].y is >= -1 and <= 1)
                return;
            dir.x = (head.x - tail[0].x) switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0,
            };
            dir.y = (head.y - tail[0].y) switch
            {
                > 0 => 1,
                < 0 => -1,
                _ => 0,
            };
            Move(ref tail[0], tail[1..], dir);
        }
    }
}
