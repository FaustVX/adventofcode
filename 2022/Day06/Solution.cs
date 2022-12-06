namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]
class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    => Execute(input, 4);
    public object Execute(ReadOnlySpan<char> input, int length)
    {
        for (int i = 0; i < input.Length; i++)
            if (IsUnique(input.Slice(i, length)))
                return i + length;
        throw new UnreachableException();

        bool IsUnique(ReadOnlySpan<char> span)
        {
            var set = new HashSet<char>(span.Length);
            foreach (var letter in span)
                if (!set.Add(letter))
                    return false;
            return true;
        }
    }

    public object PartTwo(string input)
    => Execute(input, 14);
}
