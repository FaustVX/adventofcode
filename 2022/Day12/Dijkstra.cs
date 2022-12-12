#nullable enable
namespace AdventOfCode.Y2022.Day12;

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
            (0, -1),
            (-1, 0),
            (0, 1),
            (1, 0),
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
