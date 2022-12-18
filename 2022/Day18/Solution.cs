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

    static void CalculateP2((int x, int y, int z) xyz, HashSet<(int x, int y, int z)> voxels, List<LinkedList<(int x, int y, int z)>> groups, ReadOnlySpan<(int, int, int)> dirs)
    {
        var isInVoxels = voxels.Contains(xyz);
        if (isInVoxels || groups.Any(g => g.Contains(xyz)))
            return;

        var group = default(LinkedList<(int x, int y, int z)>);
        foreach (var dir in dirs)
        {
            var pos = xyz.Add(dir);
            if (voxels.Contains(pos) == isInVoxels)
            {
                var g = groups.FirstOrDefault(g => g.Contains(pos));
                if ((g?.Count ?? 0) > (group?.Count ?? 0))
                    if (group is null)
                        group = g;
                    else if (g is not null)
                    {
                        foreach (var item in g)
                            group.AddLast(item);
                        groups.Remove(g);
                    }
            }
        }

        if (group is null)
        {
            group = new();
            groups.Add(group);
        }
        group.AddLast(xyz);
    }

    public object PartTwo(string input)
    {
        var voxels = ParseVoxels(input.AsMemory().SplitLine());
        var (minX, maxX, minY, maxY, minZ, maxZ) = voxels.GetMinMax();
        var dirs = new (int, int, int)[]
        {
            (-1, 0, 0),
            (+1, 0, 0),
            (0, -1, 0),
            (0, +1, 0),
            (0, 0, -1),
            (0, 0, +1),
        };
        var groups = new List<LinkedList<(int x, int y, int z)>>();
        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                for (var z = minZ; z <= maxZ; z++)
                    CalculateP2((x, y, z), voxels, groups, dirs);

        return voxels.SelectMany(voxel => dirs.Select(dir => voxel.Add(dir))).Count(groups.First(g => g.Contains((minX, minY, minZ))).Contains);
    }
}
