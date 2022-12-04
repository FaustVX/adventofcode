namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
class Solution : Solver, IDisplay
{
    public void DisplayPartTwo(string input)
    {
        var length = 0;
        foreach (var assigment in input.SplitLine())
        {
            var sb = new StringBuilder();
            var sections = assigment.Split(',').SelectMany(static section => section.Split('-').Select(int.Parse)).ToArray();
            var (endA, endB) = (sections[1], sections[3]);
            length = Math.Max(length, Math.Max(endA, endB));
        }

        foreach (var assigment in input.SplitLine())
        {
            var sb = new StringBuilder();
            var sections = assigment.Split(',').SelectMany(static section => section.Split('-').Select(int.Parse)).ToArray();
            var (startA, endA, startB, endB) = (sections[0], sections[1], sections[2], sections[3]);
            Compute(startA, endA, startB, endB, length).TypeString(TimeSpan.FromSeconds(.1));
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

    public void DisplayPartOne(string input)
    {
        var sb = new StringBuilder();
        for (var sA = 1; sA < 9; sA++)
            for (var eA = sA; eA < 9; eA++)
                for (var sB = 1; sB < 9; sB++)
                    for (var eB = sB; eB < 9; eB++)
                        sb.AppendFormat("{0}-{1},{2}-{3}\r\n", sA, eA, sB, eB);
        // DisplayPartTwo(sb.ToString().TrimEnd());
    }

    public object PartOne(string input)
    {
        var count = 0;
        foreach (var assigment in input.SplitLine())
        {
            var sections = assigment.Split(',').SelectMany(static section => section.Split('-').Select(int.Parse)).ToArray();
            var (startA, endA, startB, endB) = (sections[0], sections[1], sections[2], sections[3]);
            if ((startA <= startB && endA >= endB) || (startA >= startB && endA <= endB))
                count++;
        }
        return count;
    }

    public object PartTwo(string input)
    {
        var count = 0;
        foreach (var assigment in input.SplitLine())
        {
            var sections = assigment.Split(',').SelectMany(static section => section.Split('-').Select(int.Parse)).ToArray();
            var (startA, endA, startB, endB) = (sections[0], sections[1], sections[2], sections[3]);
            if ((startA <= startB && startB <= endA)
                || (startA <= endB && endB <= endA)
                || (startB <= startA && startA <= endB)
                || (startB <= endA && endA <= endB))
                count++;
        }
        return count;
    }
}
