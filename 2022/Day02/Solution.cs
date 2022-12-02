namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
class Solution : Solver
{

    public object PartOne(string input)
    {
        var sum = 0;
        foreach (var rule in input.SplitLine())
        {
            // A X : Rock
            // B Y : Paper
            // C Z : Scissors

            var me = rule[2] - 'X' + 1;
            sum += me + rule switch
            {
                ['A', _, 'X'] or ['B', _, 'Y'] or ['C', _, 'Z'] => 3,
                ['A', _, 'Y'] or ['B', _, 'Z'] or ['C', _, 'X'] => 6,
                ['A', _, 'Z'] or ['B', _, 'X'] or ['C', _, 'Y'] => 0,
            };
        }
        return sum;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
