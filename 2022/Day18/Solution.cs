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
        var magmas = ParseVoxels(input.AsMemory().SplitLine());
        var (minX, maxX, minY, maxY, minZ, maxZ) = magmas.GetMinMax();
        var dirs = new (int, int, int)[]
        {
            (-1, 0, 0),
            (+1, 0, 0),
            (0, -1, 0),
            (0, +1, 0),
            (0, 0, -1),
            (0, 0, +1),
        };
        var toCheck = new List<(int x, int y, int z)>() // will contains only air voxels
        {
            (minX, minY, minZ),
            (minX, maxY, minZ),
            (maxX, minY, minZ),
            (maxX, maxY, minZ),
            (minX, minY, maxZ),
            (minX, maxY, maxZ),
            (maxX, minY, maxZ),
            (maxX, maxY, maxZ),
        };
        var airs = new HashSet<(int x, int y, int z)>(capacity: (maxX - minX + 1) * (maxY - minY + 1) * (maxZ - minZ + 1) - magmas.Count);
        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                for (var z = minZ; z <= maxZ; z++)
                    if (!magmas.Contains((x, y, z)))
                        airs.Add((x, y, z));
        while (toCheck.Count > 0)
            for (var i = toCheck.Count - 1; i >= 0 ; i--)
            {
                var voxel = toCheck[i];
                toCheck.Remove(voxel);
                airs.Remove(voxel);
                foreach (var dir in dirs)
                {
                    var pos = voxel.Add(dir);
                    if (airs.Contains(pos))
                        toCheck.Add(pos);
                }
            }

        var emptyFaces = 0;
        foreach (var voxel in magmas)
            foreach (var dir in dirs)
                if (!magmas.Contains(voxel.Add(dir)) && ! airs.Contains(voxel.Add(dir)))
                    emptyFaces++;
        return emptyFaces;
    }
}
