namespace AdventOfCode.Y2021.Day04;

[ProblemName("Giant Squid")]
class Solution : Solver
{
    class Board
    {
        public (byte value, bool marked)[,] Grid { get; } = new (byte, bool)[5, 5];

        public Board(string[] input)
        {
            for (int y = 0; y < 5; y++)
            {
                var splitted = input[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < 5; x++)
                    Grid[x, y] = (byte.Parse(splitted[x]), false);
            }
        }

        public bool IsBingo()
        {
            if (Enumerable.Range(0, 5).Any(CheckColumn))
                return true;
            if (Enumerable.Range(0, 5).Any(CheckLine))
                return true;

            return false;

            bool CheckLine(int y)
            {
                for (int x = 0; x < 5; x++)
                    if (!Grid[x, y].marked)
                        return false;
                return true;
            }

            bool CheckColumn(int x)
            {
                for (int y = 0; y < 5; y++)
                    if (!Grid[x, y].marked)
                        return false;
                return true;
            }
        }

        public void Mark(byte number)
        {
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 5; y++)
                {
                    ref var value = ref Grid[x, y];
                    if (value.value == number)
                        value.marked = true;
                }
        }

        public static Board Parse(string input)
            => new(input.SplitLine());

        internal IEnumerable<byte> GetUnmarkedValues()
            => Grid.Cast<(byte number, bool marked)>()
                .Where(static cell => !cell.marked)
                .Select(static cell => cell.number);

    }

    private static (byte[] numbers, Board[] boards) Parse(string input)
    {
        var datas = input.Split2Lines();
        var numbers = datas[0].Split(',')
            .Select(static n => byte.Parse(n))
            .ToArray();

        var boards = datas[1..]
            .ParseToArrayOfT(Board.Parse);

        return (numbers, boards);
    }

    public object PartOne(string input)
    {
        var (numbers, boards) = Parse(input);

        foreach (var number in numbers)
            foreach (var board in boards)
            {
                board.Mark(number);
                if (board.IsBingo())
                    return number * board.GetUnmarkedValues().Sum(static v => (int)v);
            }
        throw new();
    }

    public object PartTwo(string input)
    {
        var (numbers, boards) = Parse(input);

        var wonBoards = 0;
        foreach (var number in numbers)
            foreach (var board in boards.Where(static b => !b.IsBingo()))
            {
                board.Mark(number);
                if (board.IsBingo() && ++wonBoards >= boards.Length)
                    return board.GetUnmarkedValues().Sum(static v => v) * number;
            }
        throw new();
    }
}
