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
        graph[target.x, target.y] = 25;
        graph[start.x, start.y] = 0;
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        dijkstra.Calculate(static pos => pos.to <= pos.from + 1);
        var backTrace = dijkstra.BackTrace(target);
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
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        graph[target.x, target.y] = 25;
        graph[start.x, start.y] = 0;
        var dijkstra = new Dijkstra<int>(graph, width, height, target);
        dijkstra.Calculate(static pos => pos.to + 1 >= pos.from);
        var distance = int.MaxValue;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (graph[x, y] != 1)
                    continue;
                dijkstra.BackTrace((x, y)).Count().SetMin(ref distance);
            }
        }
        return distance;
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Part1 with Colors", Part1Colors);
        yield return ("Part1 with Arrows", Part1Arrows);
        yield return ("Part2 with Colors", Part2Colors);
        yield return ("Part2 with Arrows", Part2Arrows);
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
        graph[target.x, target.y] = 25;
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        dijkstra.Calculate(static pos => pos.to <= pos.from + 1);
        var backTrace = dijkstra.BackTrace(target);
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
            Console.ReadLine();
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
        graph[target.x, target.y] = 25;
        var dijkstra = new Dijkstra<int>(graph, width, height, start);
        dijkstra.Calculate(static pos => pos.to <= pos.from + 1);
        var backTrace = dijkstra.BackTrace(target);
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
            Console.ReadLine();
            Console.CursorVisible = true;
        }
    }

    private static void Part2Colors(string input)
    {
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        graph[target.x, target.y] = 25;
        graph[start.x, start.y] = 0;
        var dijkstra = new Dijkstra<int>(graph, width, height, target);
        dijkstra.Calculate(static pos => pos.to + 1 >= pos.from);
        var (distance, e) = (int.MaxValue, Enumerable.Empty<(int x, int y)>().GetEnumerator());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (graph[x, y] != 1)
                    continue;
                var count = dijkstra.BackTrace((x, y));
                if (count.Count() < distance)
                    (distance, e) = (count.Count(), count.GetEnumerator());
            }
        }
        Display(graph, e);

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
            Console.ReadLine();
            (Console.ForegroundColor, Console.CursorVisible) = (fore, true);
        }
    }

    private static void Part2Arrows(string input)
    {
        var (graph, width, height) = input.Parse2D(static c => c switch
        {
            'S' => -1,
            'E' => 26,
            _ => c - 'a',
        });
        FindInOut(graph, width, height, out var start, out var target);
        graph[target.x, target.y] = 25;
        graph[start.x, start.y] = 0;
        var dijkstra = new Dijkstra<int>(graph, width, height, target);
        dijkstra.Calculate(static pos => pos.to + 1 >= pos.from);
        var (distance, e) = (int.MaxValue, Enumerable.Empty<(int x, int y)>().GetEnumerator());
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (graph[x, y] != 1)
                    continue;
                var count = dijkstra.BackTrace((x, y));
                if (count.Count() < distance)
                    (distance, e) = (count.Count(), count.Reverse().GetEnumerator());
            }
        }
        Display(e);

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
            Console.ReadLine();
            Console.CursorVisible = true;
        }
    }
}
