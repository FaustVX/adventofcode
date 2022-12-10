#nullable enable

namespace AdventOfCode.Y2022.Day10;

[ProblemName("Cathode-Ray Tube")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    {
        var cpu = new CPU(input.AsMemory().SplitLine());
        var signalStrength = 0;
        foreach (var cycle in cpu.Run())
            if (cycle > 220)
                break;
            else if (cycle is >= 20 && ((cycle - 20) % 40) is 0)
                signalStrength += cycle * cpu.X;
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
        var lines = new char[6][];
        for (int y = 0; y < screen.Length; y++)
        {
            lines[y] = new char[40];
            for (int x = 0; x < 40; x++)
            {
                var c = lines[y][x] = GetPixel(screen, x, y) ? '#' : ' ';
                Console.Write(c);
            }
            Console.WriteLine();
        }
        try
        {
            return OCR.GetOCR<OCR.Char4x6>(lines, 1);
        }
        catch
        {
            return "";
        }

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

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Print", Print);
    }

    private void Print(string input)
    {
        var cpu = new CPU(input.AsMemory().SplitLine());
        foreach (var cycle in cpu.Run())
        {
            var line = Math.DivRem(cycle - 1, 40, out var index);
            Console.SetCursorPosition(index, line);
            if (index >= cpu.X - 1 && index <= cpu.X + 1)
            {
                Console.Write("#");
            }
            Thread.Sleep(TimeSpan.FromSeconds(.1));
        }
    }
}
