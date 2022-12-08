namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
public class Solution : Solver, IDisplay
{
    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Display", Display);
    }

    private static void Display(string input)
    {
        var length = 0;
        foreach (var assigment in input.SplitLine())
        {
            if (!assigment.AsMemory().TryParseFormated<(int endA, int endB)>($"{"\\d+"}-{0},{"\\d+"}-{0}", out var values))
                throw new UnreachableException();
            length = Math.Max(length, Math.Max(values.endA, values.endB));
        }

        foreach (var assigment in input.SplitLine())
        {
            if (!assigment.AsMemory().TryParseFormated<(int startA, int endA, int startB, int endB)>($"{0}-{0},{0}-{0}", out var values))
                throw new UnreachableException();
            Compute(values.startA, values.endA, values.startB, values.endB, length).TypeString(TimeSpan.FromSeconds(.15));
            Console.WriteLine();

            static IEnumerable<TypedString> Compute(int startA, int endA, int startB, int endB, int length)
            {
                var colorA = ConsoleColor.DarkBlue;
                var colorBoth = ConsoleColor.DarkYellow;
                var colorB = ConsoleColor.DarkRed;
                if (startA <= startB)
                    if (endA < startB)
                    {
                        if (startA > 0)
                            yield return new() { Input = new('*', startA - 1) };
                        yield return new() { Input = new('|', endA - startA + 1), Foregroung = colorA };
                        if (startB - endA > 1)
                            yield return new() { Input = new('*', startB - endA - 1) };
                        yield return new() { Input = new('|', endB - startB + 1), Foregroung = colorB };
                        if (endB < length)
                            yield return new() { Input = new('*', length - endB) };
                    }
                    else if (endA < endB)
                    {
                        if (startA > 0)
                            yield return new() { Input = new('*', startA - 1) };
                        yield return new() { Input = new('|', startB - startA), Foregroung = colorA };
                        yield return new() { Input = new('|', endA - startB + 1), Background = colorBoth };
                        yield return new() { Input = new('|', endB - endA), Foregroung = colorB };
                        if (endB < length)
                            yield return new() { Input = new('*', length - endB) };
                    }
                    else
                    {
                        if (startA > 0)
                            yield return new() { Input = new('*', startA - 1) };
                        yield return new() { Input = new('|', startB - startA), Foregroung = colorA };
                        yield return new() { Input = new('|', endB - startB + 1), Background = colorBoth };
                        if (endA - endB > 0)
                            yield return new() { Input = new('|', endA - endB), Foregroung = colorA };
                        if (endA < length)
                            yield return new() { Input = new('*', length - endA) };
                    }
                else if (startA <= endB)
                    if (endA < endB)
                    {
                        if (startB > 0)
                            yield return new() { Input = new('*', startB - 1) };
                        yield return new() { Input = new('|', startA - startB), Foregroung = colorB };
                        yield return new() { Input = new('|', endA - startA + 1), Background = colorBoth };
                        yield return new() { Input = new('|', endB - endA), Foregroung = colorB };
                        if (endB < length)
                            yield return new() { Input = new('*', length - endB) };
                    }
                    else
                    {
                        if (startB > 0)
                            yield return new() { Input = new('*', startB - 1) };
                        yield return new() { Input = new('|', startA - startB), Foregroung = colorB };
                        yield return new() { Input = new('|', endB - startA + 1), Background = colorBoth };
                        yield return new() { Input = new('|', endA - endB), Foregroung = colorA };
                        if (endA < length)
                            yield return new() { Input = new('*', length - endA) };
                    }
                else
                {
                        if (startB > 0)
                            yield return new() { Input = new('*', startB - 1) };
                        yield return new() { Input = new('|', endB - startB + 1), Foregroung = colorB };
                        if (startA - endB > 1)
                            yield return new() { Input = new('*', startA - endB - 1) };
                        yield return new() { Input = new('|', endA - startA + 1), Foregroung = colorA };
                        if (endA < length)
                            yield return new() { Input = new('*', length - endA) };
                }
                yield return new() { Input = $" [{startA}-{endA},{startB}-{endB}]\n" };
            }
        }
    }

    public object PartOne(string input)
    {
        var count = 0;
        foreach (var assigment in input.AsMemory().SplitLine())
        {
            if (!assigment.TryParseFormated<(int startA, int endA, int startB, int endB)>($"{0}-{0},{0}-{0}", out var values))
                throw new UnreachableException();
            if ((values.startA <= values.startB && values.endA >= values.endB) || (values.startA >= values.startB && values.endA <= values.endB))
                count++;
        }
        return count;
    }

    public object PartTwo(string input)
    {
        var count = 0;
        foreach (var assigment in input.AsMemory().SplitLine())
        {
            if (!assigment.TryParseFormated<(int startA, int endA, int startB, int endB)>($"{0}-{0},{0}-{0}", out var values))
                throw new UnreachableException();
            if ((values.startA <= values.startB && values.startB <= values.endA)
                || (values.startA <= values.endB && values.endB <= values.endA)
                || (values.startB <= values.startA && values.startA <= values.endB)
                || (values.startB <= values.endA && values.endA <= values.endB))
                count++;
        }
        return count;
    }
}
