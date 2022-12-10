namespace AdventOfCode.Y2021.Day05;

#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
[ProblemName("Hydrothermal Venture")]
public class Solution : Solver
{
    public static IEnumerable<((int x, int y) pos1, (int x, int y) pos2)> Parse(string input)
    {
        foreach (var line in input.SplitLine())
        {
            var splitted = line.Split(" ->,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .ParseToArrayOfT(int.Parse);
            yield return ((splitted[0], splitted[1]), (splitted[2], splitted[3]));
        }
    }

    public object PartOne(string input)
        => Solve(input, solveDiagonals: false);

    public object PartTwo(string input)
        => Solve(input, solveDiagonals: true);

    private static int Solve(string input, bool solveDiagonals)
    {
        var positions = new DefaultableDictionary<(int x, int y), int>(capacity: 1000);
        foreach (var ((x1, y1), (x2, y2)) in Parse(input))
            if (x1 == x2)
                if (y1 < y2)
                    for (var y = y1; y <= y2; y++)
                        positions[(x1, y)]++;
                else if (y1 > y2)
                    for (var y = y2; y <= y1; y++)
                        positions[(x1, y)]++;
                else
                    positions[(x1, y1)]++;
            else if (y1 == y2)
                if (x1 < x2)
                    for (var x = x1; x <= x2; x++)
                        positions[(x, y1)]++;
                else if (x1 > x2)
                    for (var x = x2; x <= x1; x++)
                        positions[(x, y1)]++;
                else
                    positions[(x1, y1)]++;
            else if (solveDiagonals)
            {
                var (x, y) = (x2.CompareTo(x1), y2.CompareTo(y1));
                var length = Math.Abs(x1 - x2);
                for (int i = 0; i <= length; i++)
                    positions[(x1 + i * x, y1 + i * y)]++;
            }
        return positions.Values.Count(static v => v >= 2);
    }
}
