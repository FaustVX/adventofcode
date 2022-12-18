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
        minX--;
        maxX++;
        minY--;
        maxY++;
        minZ--;
        maxZ++;
        var dirs = new (int, int, int)[]
        {
            (-1, 0, 0),
            (+1, 0, 0),
            (0, -1, 0),
            (0, +1, 0),
            (0, 0, -1),
            (0, 0, +1),
        };
        var toCheck = new Queue<(int x, int y, int z)>(); // will contains only air voxels
        toCheck.Enqueue((minX, minY, minZ));
        toCheck.Enqueue((minX, maxY, minZ));
        toCheck.Enqueue((maxX, minY, minZ));
        toCheck.Enqueue((maxX, maxY, minZ));
        toCheck.Enqueue((minX, minY, maxZ));
        toCheck.Enqueue((minX, maxY, maxZ));
        toCheck.Enqueue((maxX, minY, maxZ));
        toCheck.Enqueue((maxX, maxY, maxZ));
        var airs = new HashSet<(int x, int y, int z)>(capacity: (maxX - minX + 1) * (maxY - minY + 1) * (maxZ - minZ + 1) - magmas.Count);
        for (var x = minX; x <= maxX; x++)
            for (var y = minY; y <= maxY; y++)
                for (var z = minZ; z <= maxZ; z++)
                    if (!magmas.Contains((x, y, z)))
                        airs.Add((x, y, z));
        while (toCheck.Count > 0)
        {
            var voxel = toCheck.Dequeue();
            airs.Remove(voxel);
            foreach (var dir in dirs)
            {
                var pos = voxel.Add(dir);
                if (airs.Contains(pos) && !toCheck.Contains(pos))
                    toCheck.Enqueue(pos);
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
