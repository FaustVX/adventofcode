#nullable enable

namespace AdventOfCode.Y2022.Day10;

[ProblemName("Cathode-Ray Tube")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var cpu = new CPU(input.AsMemory().SplitLine());
        var signalStrength = 0;
        foreach (var cycle in cpu.Run())
        {
            if (cycle is >= 20 and <= 220 && ((cycle - 20) % 40) is 0)
                signalStrength += cycle * cpu.X;
        }
        return signalStrength;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
