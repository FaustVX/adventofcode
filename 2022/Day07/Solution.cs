namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
class Solution : Solver //, IDisplay
{
    interface IData
    {
        int Size { get; }
        ReadOnlyMemory<char> Name { get; }
    }
    sealed class Dir : IData
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
        public int Size => Datas.Sum(static data => data.Value.Size);

        public required ReadOnlyMemory<char> Name { get; init; }

        public Dictionary<ReadOnlyMemory<char>, IData> Datas { get; } = new();
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

        public void AddFile(File file)
        => Datas.Add(file.Name, file);
    }

    sealed class File : IData
    {
        public required int Size { get; init; }

        public required ReadOnlyMemory<char> Name { get; init; }
    }

    public object PartOne(string input)
    {
        var root = Dir.CreateRoot();
        var allDir = ParseTree(input, root);
        return allDir.Where(static dir => dir.Size <= 100_000).Sum(static dir => dir.Size);
    }

    private static List<Dir> ParseTree(string input, Dir root)
    {
        var currentDir = root;
        var isLS = false;
        var allDir = new List<Dir>()
        {
            root,
        };

        foreach (var line in input.AsMemory().SplitLine().Skip(1))
        {
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
                    currentDir.AddFile(new() { Name = name, Size = size });
            }
        }

        return allDir;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
