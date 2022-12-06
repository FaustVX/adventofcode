namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]
class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        for (int i = 0; i < input.Length; i++)
            if (IsUnique(input.AsSpan(i, 4)))
                return i + 4;
        throw new UnreachableException();

        bool IsUnique(ReadOnlySpan<char> span)
        {
            var set = new HashSet<char>(4);
            foreach (var letter in span)
                if (!set.Add(letter))
                    return false;
            return true;
        }
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
