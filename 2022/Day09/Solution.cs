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
    }

    void Move(ref (int x, int y) head, ref (int x, int y) tail, HashSet<(int x, int y)> visited, (int x, int y) dir)
    {
        var previousHead = head;
        head.x += dir.x;
        head.y += dir.y;
        if (head.x - tail.x is >= -1 and <= 1 && head.y - tail.y is >= -1 and <= 1)
            return;
        tail = previousHead;
        visited.Add(tail);
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
