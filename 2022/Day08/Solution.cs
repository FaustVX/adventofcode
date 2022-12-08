namespace AdventOfCode.Y2022.Day08;

[ProblemName("Treetop Tree House")]
class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        var (trees, width, height) = input.Parse2D(static c => c - '0');
        var visible = (width + height) * 2 - 4;
        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
            {
                var tree = trees[x, y];
                if (IsVisibleinWidth(tree, trees, x, 0, y) || IsVisibleinWidth(tree, trees, x, y + 1, height)
                || IsVisibleinHeight(tree, trees, y, 0, x) || IsVisibleinHeight(tree, trees, y, x + 1, width))
                    visible++;
            }
        return visible;

        static bool IsVisibleinWidth(int height, int[,]  trees, int x, int startY, int endY)
        {
            for (var y = startY; y < endY; y++)
                if (trees[x, y] >= height)
                    return false;
            return true;
        }

        static bool IsVisibleinHeight(int height, int[,]  trees, int y, int startX, int endX)
        {
            for (var x = startX; x < endX; x++)
                if (trees[x, y] >= height)
                    return false;
            return true;
        }
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
