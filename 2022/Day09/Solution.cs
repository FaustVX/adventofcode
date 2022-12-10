namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]
public class Solution : Solver //, IDisplay
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

        static void Move(ref (int x, int y) head, Span<(int x, int y)> tail, (int x, int y) dir)
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
    }
}
