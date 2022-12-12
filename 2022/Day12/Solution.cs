#nullable enable
namespace AdventOfCode.Y2022.Day12;

[ProblemName("Hill Climbing Algorithm")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    {
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        var backTrace = dijkstra.Calculate(target, static pos => pos.to <= pos.from + 1);
        return backTrace.Count() - 1;
    }

    private static void FindInOut(int[,] graph, int width, int height, out (int x, int y) start, out (int x, int y) target)
    {
        start = default;
        target = default;
        var setOne = false;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                switch (graph[x, y])
                {
                    case -1:
                        start = (x, y);
                        if (setOne)
                            return;
                        setOne = true;
                        break;
                    case 26:
                        target = (x, y);
                        if (setOne)
                            return;
                        setOne = true;
                        break;
                }
    }

    public object PartTwo(string input)
    {
        return 0;
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Part1 with Colors", Part1Colors);
        yield return ("Part1 with Arrows", Part1Arrows);
    }

    private static void Part1Colors(string input)
    {
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        var backTrace = dijkstra.Calculate(target, static pos => pos.to <= pos.from + 1);
        Display(graph, backTrace.GetEnumerator());

        static void Display(int[,] graph, IEnumerator<(int x, int y)> path)
        {
            var (width, height) = (graph.GetLength(0), graph.GetLength(1));
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    Console.Write(graph[x, y] switch
                    {
                        -1 => 'S',
                        26 => 'E',
                        var i => (char)(i + 'a'),
                    });
                Console.WriteLine();
            }
            (var fore, Console.ForegroundColor, Console.CursorVisible) = (Console.ForegroundColor, ConsoleColor.Green, false);
            while (path.MoveNext())
            {
                var current = path.Current;
                Console.SetCursorPosition(current.x, current.y);
                Console.Write(graph[current.x, current.y] switch
                    {
                        -1 => 'S',
                        26 => 'E',
                        var i => (char)(i + 'a'),
                    });
            }
            (Console.ForegroundColor, Console.CursorVisible) = (fore, true);
        }
    }

    private static void Part1Arrows(string input)
    {
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        var backTrace = dijkstra.Calculate(target, static pos => pos.to <= pos.from + 1);
        Display(backTrace.GetEnumerator());

        static void Display(IEnumerator<(int x, int y)> path)
        {
            path.MoveNext();
            var previous = path.Current;
            Console.SetCursorPosition(previous.x, previous.y);
            Console.Write('E');
            Console.CursorVisible = false;
            while (path.MoveNext())
            {
                var current = path.Current;
                var dir = (current.x - previous.x, current.y - previous.y) switch
                {
                    (-1, 0) => '>',
                    (1, 0) => '<',
                    (0, -1) => 'v',
                    (0, 1) => '^',
                    var d => throw new UnreachableException(d.ToString()),
                };
                Console.SetCursorPosition(current.x, current.y);
                Console.Write(dir);
                previous = current;
            }
            Console.CursorVisible = true;
        }
    }
}

public class Dijkstra<T>
{
    private readonly Dictionary<(int x, int y), T> _nodes;
    private readonly Dictionary<(int x, int y), int> _dists;
    private readonly Dictionary<(int x, int y), (int x, int y)?> _prevs;

    public Dijkstra(T[,] graph, int width, int height, (int x, int y) start)
    {
        _nodes = new(capacity: width * height);
        _dists = new(capacity: width * height);
        _prevs = new(capacity: width * height);
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                _nodes.Add((x, y), graph[x, y]);
                _dists.Add((x, y), int.MaxValue);
                _prevs.Add((x, y), null);
            }
        _dists[start] = 0;
    }

    public IEnumerable<(int x, int y)> Calculate((int x, int y) target, Func<(T from, T to), bool> predicate)
    {
        ReadOnlySpan<(int x, int y)> offset = stackalloc (int x, int y)[]
        {
            (-1, 0),
            (0, -1),
            (1, 0),
            (0, 1),
        };

        while (_nodes.Count > 0)
        {
            var u = _nodes.MinBy(GetMinDist);
            _nodes.Remove(u.Key);
            foreach (var dir in offset)
            {
                var pos = (x: u.Key.x + dir.x, y: u.Key.y + dir.y);
                if (!(_nodes.TryGetValue(pos, out var val) && predicate((u.Value, val))))
                    continue;
                var alternate = _dists[u.Key] + 1;
                if (alternate < _dists[pos])
                {
                    _dists[pos] = alternate;
                    _prevs[pos] = u.Key;
                }
            }
        }

        return BackTrace(target);
    }

    private int GetMinDist(KeyValuePair<(int x, int y), T> kvp)
    => _dists[kvp.Key];

    private IEnumerable<(int x, int y)> BackTrace((int x, int y)? target)
    {
        for (; target != null; target = _prevs[target.Value])
            yield return target.Value;
    }
}
