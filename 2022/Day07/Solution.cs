namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
class Solution : Solver //, IDisplay
{
    sealed class Dir
    {
        [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
        private Dir(ReadOnlyMemory<char> rootName)
        {
            Name = Name;
            Parent = this;
        }

        public Dir()
        { }

        public static Dir CreateRoot()
            => new("/".AsMemory());

        private int fileSize;
        public int Size => Datas.Sum(static data => data.Value.Size) + fileSize;

        public required ReadOnlyMemory<char> Name { get; init; }

        public Dictionary<ReadOnlyMemory<char>, Dir> Datas { get; } = new();
        public required Dir Parent { get; init; }
        public Dir CreateChild(ReadOnlyMemory<char> name)
        {
            var dir = new Dir()
            {
                Name = name,
                Parent = this,
            };
            Datas.Add(name, dir);
            return dir;
        }

        public void AddFile(int size)
        => fileSize += size;
    }

    public object PartOne(string input)
    {
        var allDir = ParseTree(input);
        return allDir.Where(static dir => dir.Size <= 100_000).Sum(static dir => dir.Size);
    }

    private static List<Dir> ParseTree(string input)
    {
        var root = Dir.CreateRoot();
        var currentDir = root;
        var isLS = false;
        var allDir = new List<Dir>()
        {
            root,
        };

        foreach (var line in input.AsMemory().SplitLine().Skip(1))
            if (line.Span.StartsWith("$ "))
            {
                isLS = false;
                var command = line[2..];
                if (command.Span.StartsWith("cd "))
                {
                    var args = command[3..];
                    if (args.Span is "/")
                        currentDir = root;
                    else if (args.Span is "..")
                        currentDir = currentDir.Parent;
                    else
                        currentDir = (Dir)currentDir.Datas.Values.First(dir => dir.Name.Span.Equals(args.Span, StringComparison.InvariantCulture));
                }
                else if (command.Span.StartsWith("ls"))
                    isLS = true;
            }
            else if (isLS)
            {
                var space = line.Span.IndexOf(' ') + 1;
                var name = line[space..];
                if (line.Span.StartsWith("dir"))
                    allDir.Add(currentDir.CreateChild(name));
                else if (int.TryParse(line.Span[..space], out var size))
                    currentDir.AddFile(size);
            }

        return allDir;
    }

    public object PartTwo(string input)
    {
        var allDir = ParseTree(input);
        var totalUsedSize = allDir[0].Size;
        var freeSpaceSize = 70_000_000 - totalUsedSize;
        var sizeToFreeUp = 30_000_000 - freeSpaceSize;
        return allDir.OrderBy(static dir => dir.Size).First(dir => dir.Size >= sizeToFreeUp).Size;
    }
}
