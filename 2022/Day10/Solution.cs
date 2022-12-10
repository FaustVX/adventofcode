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
        var screen = new long[6];
        var cpu = new CPU(input.AsMemory().SplitLine());
        foreach (var cycle in cpu.Run())
        {
            var line = Math.DivRem(cycle - 1, 40, out var index);
            if (index >= cpu.X - 1 && index <= cpu.X + 1)
                SetPixel(screen, index, line);
        }
        for (int y = 0; y < screen.Length; y++)
        {
            for (int x = 0; x < 40; x++)
                Console.Write(GetPixel(screen, x, y) ? '#' : ' ');
            Console.WriteLine();
        }
        return Console.ReadLine()!;

        static void SetPixel(long[] screen, int x, int y)
        {
            ref var line = ref screen[y];
            line |= 1L << x;
        }

        static bool GetPixel(long[] screen, int x, int y)
        {
            var line = screen[y];
            return (line & (1L << x)) != 0;
        }
    }
}
