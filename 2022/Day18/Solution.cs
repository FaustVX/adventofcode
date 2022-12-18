#nullable enable
namespace AdventOfCode.Y2022.Day18;

[ProblemName("Boiling Boulders")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var voxels = ParseVoxels(input.AsMemory().SplitLine());
        ReadOnlySpan<(int x, int y, int z)> dirs = stackalloc (int, int, int)[]
        {
            (-1, 0, 0),
            (+1, 0, 0),
            (0, -1, 0),
            (0, +1, 0),
            (0, 0, -1),
            (0, 0, +1),
        };
        var emptyFaces = 0;
        foreach (var voxel in voxels)
            foreach (var dir in dirs)
                if (!voxels.Contains(voxel.Add(dir)))
                    emptyFaces++;
        return emptyFaces;
    }

    private static HashSet<(int x, int y, int z)> ParseVoxels(ReadOnlyMemory<ReadOnlyMemory<char>> positions)
    {
        var voxels = new HashSet<(int x, int y, int z)>(capacity: positions.Length);
        foreach (var position in positions.Span)
            voxels.Add(Extensions.ParseToTuple<int, int, int>(position.Split(",")));
        return voxels;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
