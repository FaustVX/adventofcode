#nullable enable
namespace AdventOfCode.Y2022.Day16;

[ProblemName("Proboscidea Volcanium")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var tunnels = ParseTunnels(input.AsMemory().SplitLine());
        return Calcutate(tunnels, tunnels.Where(static kvp => kvp.Value.pressure != 0).Select(static kvp => kvp.Key).ToImmutableList(), 30, "AA");
    }

    private static int Calcutate(ImmutableDictionary<string, (int pressure, List<string> leading)> tunnels, ImmutableList<string> valvesToOpen, int minuteRemaining, string entrance)
    {
        if (minuteRemaining < 1 || valvesToOpen.IsEmpty)
            return 0;
        var dijkstra = new Dijkstra(tunnels, entrance);
        dijkstra.Calculate();
        if (valvesToOpen is [var l])
        {
            var backtrace = dijkstra.BackTrace(l).ToArray();
            var minute = minuteRemaining - backtrace.Count();
            if (minute >= 1)
                return tunnels[l].pressure * minute;
            return 0;
        }
        var maxPressure = 0;
        foreach (var label in valvesToOpen)
        {
            var backtrace = dijkstra.BackTrace(label).ToArray();
            var minute = minuteRemaining - backtrace.Count();
            var pressure = Calcutate(tunnels
                    , valvesToOpen.Remove(label)
                    , minute
                    , entrance: label)
                + tunnels[label].pressure * minute;
            if (pressure > maxPressure)
                maxPressure = pressure;
        }
        return maxPressure;
    }

    private static ImmutableDictionary<string, (int pressure, List<string> leading)> ParseTunnels(ReadOnlyMemory<ReadOnlyMemory<char>> lines)
    {
        var tunnels = new Dictionary<string, (int pressure, List<string> leading)>(capacity: lines.Length);
        foreach (var tunnel in lines.Span)
        {
            var infos = tunnel.Split(" |=|;");
            var leading = infos[11..];
            tunnels.Add(infos.Span[1].ToString(), (int.Parse(infos.Span[5].Span), Enumerable.Repeat(leading, leading.Length).Select((l, i) => l.Span[i][..2].ToString()).ToList()));
        }
        return tunnels.ToImmutableDictionary();
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
