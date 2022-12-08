namespace AdventOfCode.Y2020.Day01;

[ProblemName("Report Repair")]
public class Solution : Solver
{

    public object PartOne(string input)
        => Solve1(2020, input.SplitLine().Select(int.Parse).ToArray());

    public object PartTwo(string input)
        => Solve2(2020, input.SplitLine().Select(int.Parse).ToArray());

    private static long Solve1(int sum, int[] inputs)
    {
        for (int i = 0; i < inputs.Length - 1; i++)
        {
            for (int j = i + 1; j < inputs.Length; j++)
            {
                if(inputs[i] + inputs[j] == sum)
                    return inputs[i] * inputs[j];
            }
        }

        throw new();
    }

    private static long Solve2(int sum, int[] inputs)
    {
        for (int i = 0; i < inputs.Length - 2; i++)
        {
            for (int j = i + 1; j < inputs.Length - 1; j++)
            {
                for (int k = 0; k < inputs.Length; k++)
                {
                    if(inputs[i] + inputs[j] + inputs[k] == sum)
                        return inputs[i] * inputs[j] * inputs[k];
                }
            }
        }

        throw new();
    }
}
