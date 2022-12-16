#nullable enable
namespace AdventOfCode.Y2022.Day16;

public class Dijkstra
{
    private readonly Dictionary<string, (int pressure, IEnumerable<string> leading)> _nodes;
    private readonly Dictionary<string, int> _dists;
    public Dictionary<string, string?> Previous { get; }

    public Dijkstra(IReadOnlyDictionary<string, (int pressure, List<string> leading)> graph, string start)
    {
        _nodes = new(capacity: graph.Count);
        _dists = new(capacity: graph.Count);
        Previous = new(capacity: graph.Count);
        foreach (var (key, value) in graph)
        {
            _nodes.Add(key, graph[key]);
            _dists.Add(key, int.MaxValue);
            Previous.Add(key, null);
        }
        _dists[start] = 0;
    }

    public void Calculate()
    {
        while (_nodes.Count > 0)
        {
            var u = _nodes.MinBy(GetMinDist);
            _nodes.Remove(u.Key);
            foreach (var pos in u.Value.leading)
            {
                if (!(_nodes.ContainsKey(pos)))
                    continue;
                var alternate = _dists[u.Key] + 1;
                if (alternate < _dists[pos])
                {
                    _dists[pos] = alternate;
                    Previous[pos] = u.Key;
                }
            }
        }
    }

    private int GetMinDist<T>(KeyValuePair<string, T> kvp)
    => _dists[kvp.Key];

    public IEnumerable<string> BackTrace(string? target)
    {
        for (; target != null; target = Previous[target])
            yield return target;
    }
}
