namespace AdventOfCode.Y2021.Day03;

[ProblemName("Binary Diagnostic")]
public class Solution : Solver
{
    private static byte[][] Parse(string input)
        => input.ParseToArrayOfT(static l => l.Select(static s => (byte)(s - '0')).ToArray());

    public object PartOne(string input)
    {
        var datas = Parse(input);
        var half = datas.Length / 2;

        var gamma = datas.Aggregate(Enumerable.Repeat(0, datas[0].Length),
            static (acc, data) => data
                .Zip(acc)
                .Select(static v => v.Second + v.First))
        .Aggregate(0, (acc, sum) => (acc << 1) + (sum > half ? 1 : 0));
        return gamma * (~gamma & Convert.ToInt32(new string('1', datas[0].Length), 2));
    }

    public object PartTwo(string input)
    {
        var datas = Parse(input);
        var (oxygen, co2) = (Find(datas, keepMostCommonBit: true), Find(datas, keepMostCommonBit: false));
        return oxygen * co2;

        static int Find(byte[][] datas, bool keepMostCommonBit, int index = 0)
        {
            var grouped = datas
                .GroupBy(d => d[index])
                .ToDictionary(g => (int)g.Key, g => g.ToArray());
            var isSame = AllAreSame(grouped, g => g.Value.Length);
            var mostCommonBit = grouped.MaxBy(kvp => kvp.Value.Length).Key;

            datas = (isSame, keepMostCommonBit) switch
            {
                (true, true) => grouped[1],
                (true, false) => grouped[0],
                (false, true) => grouped[mostCommonBit],
                (false, false) => grouped[1 - mostCommonBit],
            };
            if (datas.Length is 1)
                return datas[0].Aggregate(0, (value, bit) => (value << 1) + bit);
            return Find(datas, keepMostCommonBit, index + 1);

            static bool AllAreSame<TSource, TValue>(IEnumerable<TSource> source, Func<TSource, TValue> selector)
                where TValue : IEquatable<TValue>
            {
                var first = selector(source.First());
                foreach (var item in source.Skip(1).Select(selector))
                    if (!item.Equals(first))
                        return false;
                return true;
            }
        }
    }
}
