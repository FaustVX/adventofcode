namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
class Solution : Solver //, IDisplay
{
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
