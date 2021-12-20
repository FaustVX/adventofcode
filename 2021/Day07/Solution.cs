namespace AdventOfCode.Y2021.Day07;

[ProblemName("The Treachery of Whales")]
class Solution : Solver
{
    private static int[] Parse(string input)
        => input.Split(',').ParseToArrayOfT(int.Parse);

    public object PartOne(string input)
    {
        var crabs = Parse(input);
        return crabs.Min(pos => crabs.Aggregate(0L, (acc, crab) => acc + Math.Abs(crab - pos)));
    }

    public object PartTwo(string input)
    {
        var crabs = Parse(input);
        int min = crabs.Min();
        return Enumerable.Range(min, crabs.Max() - min).Min(pos => crabs.Aggregate(0L, (acc, crab) =>
        {
            var n = Math.Abs(crab - pos);
            return acc + (long)((n / 2f) * (n + 1));
        }));
    }
}
