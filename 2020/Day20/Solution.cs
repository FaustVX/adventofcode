namespace AdventOfCode.Y2020.Day20;

record Tile(int Id, string Top, string Bot, string Left, string Right, bool Flipped, int Rotated)
{
    public static Tile Parse(string[] lines)
        => new(int.Parse(lines[0][5..^1]),
            lines[1],
            lines[^1],
            string.Concat(lines[1..].Select(l => l[0])),
            string.Concat(lines[1..].Select(l => l[^1])),
            false, 0);

    public IEnumerable<Tile> GenerateVariations()
        => GenerateRot()
            .Concat((this with { Top = Reverse(Top), Bot = Reverse(Bot), Left = Right, Right = Left, Flipped = true }).GenerateRot());

    IEnumerable<Tile> GenerateRot()
    {
        var tile = this;
        yield return tile;
        yield return tile = tile.Rotate();
        yield return tile = tile.Rotate();
        yield return tile = tile.Rotate();
    }

    Tile Rotate()
        => this with { Right = Top, Left = Bot, Bot = Reverse(Right), Top = Reverse(Left), Rotated = Rotated + 1 };

    static string Reverse(string input)
        => string.Concat(input.Reverse());
}

[ProblemName("Jurassic Jigsaw")]
public class Solution : Solver
{
    public object PartOne(string input)
    {
        var lines = input.TrimEnd().Split2Lines();
        var grid = new Dictionary<(int x, int y), Tile>(lines.Length);
        var tiles = lines.Select(StringExtensions.SplitLine).Select(Tile.Parse).Where(t => !grid.Values.Any(v => t.Id == v.Id)).Select(t => t.GenerateVariations());
        Add((0, 0), tiles.First().First());

        while (grid.Count < lines.Length)
            AddGrid();

        var minX = grid.Keys.Min(pos => pos.x);
        var minY = grid.Keys.Min(pos => pos.y);
        var maxX = grid.Keys.Max(pos => pos.x);
        var maxY = grid.Keys.Max(pos => pos.y);

        return (long)grid[(minX, minY)].Id * grid[(minX, maxY)].Id * grid[(maxX, minY)].Id * grid[(maxX, maxY)].Id;

        void AddGrid()
        {
            foreach (var tile in tiles.SelectMany(t => t))
                foreach (var ((x, y), value) in grid)
                {
                    if (value.Top == tile.Bot)
                    {
                        Add((x, y + 1), tile);
                        return;
                    }
                    if (value.Bot == tile.Top)
                    {
                        Add((x, y - 1), tile);
                        return;
                    }
                    if (value.Left == tile.Right)
                    {
                        Add((x - 1, y), tile);
                        return;
                    }
                    if (value.Right == tile.Left)
                    {
                        Add((x + 1, y), tile);
                        return;
                    }
                }
        }

        void Add((int, int) pos, Tile tile)
            => grid[pos] = tile;
    }

    public object PartTwo(string input)
        => 0;
}
