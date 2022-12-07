namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
class Solution : Solver //, IDisplay
{
    sealed class Dir
    {
        private int filesSize;
        public int Size => Datas.Sum(static data => data.Size) + filesSize;

        public required ReadOnlyMemory<char> Name { get; init; }

        public List<Dir> Datas { get; } = new();
        public Dir CreateChild(ReadOnlyMemory<char> name)
        {
            var dir = new Dir()
            {
                Name = name,
            };
            Datas.Add(dir);
            return dir;
        }

        public void AddFile(int size)
        => filesSize += size;
    }

    public object PartOne(string input)
    {
        var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan((List<ReadOnlyMemory<char>>)input.AsMemory().SplitLine())[1..];
        var root = new Dir() { Name = "/".AsMemory() };
        var allDir = new List<Dir>()
        {
            root,
        };
        ParseTree(span, root, allDir);
        return allDir.Where(static dir => dir.Size <= 100_000).Sum(static dir => dir.Size);
    }

    private static int ParseTree(ReadOnlySpan<ReadOnlyMemory<char>> input, Dir currentDir, List<Dir> allDir)
    {
        var length = input.Length;
        if (input[0].Span is not "$ ls")
            throw new UnreachableException();
        for (input = input[1..]; !input.IsEmpty && input[0].Span[0] != '$'; input = input[1..])
        {
            if (input[0].Span.StartsWith("dir "))
                allDir.Add(currentDir.CreateChild(input[0][4..]));
            else if (input[0].TryParseFormated<ValueTuple<int>>($"{0} {".*"}", out var values))
                currentDir.AddFile(values.Item1);
        }
        while (!input.IsEmpty)
        {
            if (input[0].Span is "$ cd ..")
                return length - input.Length + 1;
            var dirName = input[0].Slice(5);
            var newLength = ParseTree(input[1..], currentDir.Datas.Find(dir => dir.Name.Span.Equals(dirName.Span, StringComparison.InvariantCulture)) ?? throw new UnreachableException(), allDir);
            input = input[(newLength + 1)..];
        }
        return length;
    }

    public object PartTwo(string input)
    {
        var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan((List<ReadOnlyMemory<char>>)input.AsMemory().SplitLine())[1..];
        var root = new Dir() { Name = "/".AsMemory() };
        var allDir = new List<Dir>()
        {
            root,
        };
        ParseTree(span, root, allDir);
        var totalUsedSize = root.Size;
        var freeSpaceSize = 70_000_000 - totalUsedSize;
        var sizeToFreeUp = 30_000_000 - freeSpaceSize;
        return allDir.Where(dir => dir.Size >= sizeToFreeUp).Min(static dir => dir.Size);
    }
}
