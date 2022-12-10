namespace AdventOfCode.Y2021.Day02;

#pragma warning disable CS0612 // 'StringExtensions.SplitLine(string)' is Obsolete
[ProblemName("Dive!")]
public class Solution : Solver
{
    private enum Direction
    {
        Forward,
        Up,
        Down,
    }

    private static IEnumerable<(Direction direction, int value)> Parse(string input)
        => input.SplitLine()
            .Select(static line => line.Split(' '))
            .Select(static array => (Enum.Parse<Direction>(array[0], ignoreCase: true), int.Parse(array[1])));

    public object PartOne(string input)
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value. For example, the pattern ‘...’ is not covered.
        => Parse(input).Aggregate((position: 0, depth: 0), static (infos, instruction) => instruction switch
#pragma warning restore
            {
                (Direction.Forward, var value) => infos with {
                    position = infos.position + value
                },
                (Direction.Up, var value) => infos with {
                    depth = infos.depth - value
                },
                (Direction.Down, var value) => infos with {
                    depth = infos.depth + value
                },
            }, static infos => infos.depth * infos.position);

    public object PartTwo(string input)
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value. For example, the pattern ‘...’ is not covered.
        => Parse(input).Aggregate((position: 0, depth: 0, aim: 0), static (infos, instruction) => instruction switch
#pragma warning restore
            {
                (Direction.Forward, var value) => infos with
                    {
                        position = infos.position + value,
                        depth = infos.depth + value * infos.aim
                    },
                (Direction.Up, var value) => infos with
                    {
                        aim = infos.aim - value
                    },
                (Direction.Down, var value) => infos with
                    {
                        aim = infos.aim + value
                    },
            }, static infos => infos.position * infos.depth);
}
