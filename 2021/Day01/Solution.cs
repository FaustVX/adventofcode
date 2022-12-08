namespace AdventOfCode.Y2021.Day01;

[ProblemName("Sonar Sweep")]
public class Solution : Solver
{
    public object PartOne(string input)
        => Solve(input, i => i);

    public object PartTwo(string input)
        => Solve(input, static source => source.Zip(source[1..], source[2..]).Select(t => t.First + t.Second + t.Third));

    private static int Solve(string input, Func<int[], IEnumerable<int>> selector)
    {
        var inputs = input.ParseToArrayOfT(int.Parse);
        var (increasedCount, last) = (-1, int.MinValue);
        foreach (var item in selector(inputs))
            if (item > last)
                increasedCount++;
            else
                last = item;
        return increasedCount;
    }
}
