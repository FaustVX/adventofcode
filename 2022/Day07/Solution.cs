#nullable enable
namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
public class Solution : Solver , IDisplay
{
    public object PartOne(string input)
    {
        var span = input.AsMemory().SplitLine()[1..];
        var allDirSize = 0;
        ParseTree(span.Span, ImmutableStack.Create("/".AsMemory()),out _, dirClosed: (_, size) =>
        {
            if (size <= 100_000)
                allDirSize += size;
        });
        return allDirSize;
    }

    private static int ParseTree(ReadOnlySpan<ReadOnlyMemory<char>> input, ImmutableStack<ReadOnlyMemory<char>> currentDir, out int lineSkipped, Action<ImmutableStack<ReadOnlyMemory<char>>>? dirOpened = null!, Action<ImmutableStack<ReadOnlyMemory<char>>, int>? fileCreated = null!, Action<ImmutableStack<ReadOnlyMemory<char>>, int>? dirClosed = null!)
    {
        dirOpened?.Invoke(currentDir);
        lineSkipped = 0;
        var length = input.Length;
        var localDirs = new List<ReadOnlyMemory<char>>();
        var dirSize = 0;
        if (input[lineSkipped].Span is not "$ ls")
            throw new UnreachableException();
        lineSkipped++;
        for (; !input[lineSkipped..].IsEmpty && input[lineSkipped].Span[0] != '$'; lineSkipped++)
        {
            if (input[lineSkipped].Span.StartsWith("dir "))
            {
                localDirs.Add(input[lineSkipped][4..]);
            }
            else
            {
                var space = input[lineSkipped].Span.IndexOf(' ');
                var fileSize = int.Parse(input[lineSkipped][..space].Span);
                dirSize += fileSize;
                fileCreated?.Invoke(currentDir.Push(input[lineSkipped].Slice(space + 1)), fileSize);
            }
        }
        while (lineSkipped < input.Length)
        {
            if (input[lineSkipped].Span is "$ cd ..")
            {
                dirClosed?.Invoke(currentDir, dirSize);
                lineSkipped++;
                return dirSize;
            }
            var dirName = input[lineSkipped].Slice(5);
            dirSize += ParseTree(input.Slice(lineSkipped + 1), currentDir.Push(localDirs.Find(dir => dir.Span.Equals(dirName.Span, StringComparison.InvariantCulture))), out var lines, dirOpened, fileCreated, dirClosed);
            lineSkipped += lines;
            lineSkipped++;

        }
        dirClosed?.Invoke(currentDir, dirSize);
        lineSkipped++;
        return dirSize;
    }

    public object PartTwo(string input)
    {
        var span = input.AsMemory().SplitLine()[1..];
        var allDir = new List<int>();
        var totalUsedSize = ParseTree(span.Span, ImmutableStack.Create("/".AsMemory()), out _, dirClosed: (_, size) => allDir.Add(size));
        var freeSpaceSize = 70_000_000 - totalUsedSize;
        var sizeToFreeUp = 30_000_000 - freeSpaceSize;
        return allDir.Where(dir => dir >= sizeToFreeUp).Min();
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Tree-Like", TreeDisplay);
    }

    private void TreeDisplay(string input)
    {
        var span = input.AsMemory().SplitLine()[1..];
        var allDir = new List<int>();

        var indent = 0;
        var totalUsedSize = ParseTree(span.Span, ImmutableStack.Create("".AsMemory()), out _
        , dirOpened: name =>
        {
            Console.WriteLine((indent >= 1 ? string.Concat(Enumerable.Repeat("|  ", indent - 1)) + "|--" : "") + name.Peek() + "/");
            indent++;
        }
        , fileCreated: (name, _) => Console.WriteLine((indent >= 1 ? string.Concat(Enumerable.Repeat("|  ", indent - 1)) + "|--" : "") + name.Peek())
        , dirClosed: (_, _) => indent--
        );
    }
}
