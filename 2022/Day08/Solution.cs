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
                if (IsVisibleinHeight(tree, trees, x, 0, y) || IsVisibleinHeight(tree, trees, x, y + 1, height)
                || IsVisibleinWidth(tree, trees, y, 0, x) || IsVisibleinWidth(tree, trees, y, x + 1, width))
                    visible++;
            }
        return visible;

        static bool IsVisibleinHeight(int height, int[,]  trees, int x, int startY, int endY)
        {
            for (var y = startY; y < endY; y++)
                if (trees[x, y] >= height)
                    return false;
            return true;
        }

        static bool IsVisibleinWidth(int height, int[,]  trees, int y, int startX, int endX)
        {
            for (var x = startX; x < endX; x++)
                if (trees[x, y] >= height)
                    return false;
            return true;
        }
    }

    public object PartTwo(string input)
    {
        var (trees, width, height) = input.Parse2D(static c => c - '0');
        var maxScenicScore = 0;
        for (int x = 1; x < width - 1; x++)
            for (int y = 1; y < height - 1; y++)
                if (ScenicScore(trees, x, y, width, height) is var score && score > maxScenicScore)
                    maxScenicScore = score;
        return maxScenicScore;

        static int ScenicScore(int[,] trees, int x, int y, int width, int height)
        {
            var tree = trees[x, y];
            return TreesVisibleinHeight(tree, trees, x, y - 1, -1)
                 * TreesVisibleinHeight(tree, trees, x, y + 1, height)
                 * TreesVisibleinWidth(tree, trees, y, x - 1, -1)
                 * TreesVisibleinWidth(tree, trees, y, x + 1, width);
        }

        static int TreesVisibleinHeight(int height, int[,]  trees, int x, int startY, int endY)
        {
            var treesVisible = 0;
            var sign = endY > startY ? 1 : -1;
            for (var y = startY; Math.Abs(y - endY) >= 1; y += sign)
                if (trees[x, y] >= height)
                    return treesVisible + 1;
                else
                    treesVisible++;
            return treesVisible;;
        }

        static int TreesVisibleinWidth(int height, int[,]  trees, int y, int startX, int endX)
        {
            var treesVisible = 0;
            var sign = endX > startX ? 1 : -1;
            for (var x = startX; Math.Abs(x - endX) >= 1; x += sign)
                if (trees[x, y] >= height)
                    return treesVisible + 1;
                else
                    treesVisible++;
            return treesVisible;;
        }
    }
}
