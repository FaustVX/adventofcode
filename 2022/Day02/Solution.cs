namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    {
        var sum = 0;
        foreach (var rule in input.AsMemory().SplitLine().Span)
        {
            // A X : Rock
            // B Y : Paper
            // C Z : Scissors

            var me = rule.Span[2] - 'X' + 1;
            sum += me + rule.Span switch
            {
                "A X" or "B Y" or "C Z" => 3,
                "A Y" or "B Z" or "C X" => 6,
                "A Z" or "B X" or "C Y" => 0,
                _ => throw new UnreachableException(),
            };
        }
        return sum;
    }

    public object PartTwo(string input)
    {
        var sum = 0;
        foreach (var rule in input.AsMemory().SplitLine().Span)
        {
            // A : Rock
            // B : Paper
            // C : Scissors

            var loose = rule.Span[0] switch
            {
                'A' => 3,
                'B' => 1,
                'C' => 2,
                _ => throw new UnreachableException(),
            };

            var draw = rule.Span[0] - 'A' + 1;

            var win = rule.Span[0] switch
            {
                'A' => 2,
                'B' => 3,
                'C' => 1,
                _ => throw new UnreachableException(),
            };
            sum += rule.Span[2] switch
            {
                'X' => 0 + loose,
                'Y' => 3 + draw,
                'Z' => 6 + win,
                _ => throw new UnreachableException(),
            };
        }
        return sum;
    }

    public IEnumerable<(string name, Action<string> action)> GetDisplays()
    {
        yield return ("Part1", DisplayPartOne);
        yield return ("Part2", DisplayPartTwo);
    }

    static void DisplayPartOne(string input)
    {
        var sum = 0;
        foreach (var rule in input.AsMemory().SplitLine().Span)
        {
            var sb = new StringBuilder();
            sb.Append(rule.Span[0] switch
            {
                'A' => "   Rock ",
                'B' => "  Paper ",
                'C' => "Scisors ",
                _ => throw new UnreachableException(),
            });
            sb.Append(" vs. ");
            sb.Append(rule.Span[2] switch
            {
                'X' => "Rock    ",
                'Y' => "Paper   ",
                'Z' => "Scisors ",
                _ => throw new UnreachableException(),
            });
            sb.Append(".");
            sb.Append(".");
            sb.Append(".");
            sb.Append(rule.Span switch
            {
                "A X" or "B Y" or "C Z" => " Draw     ",
                "A Y" or "B Z" or "C X" => " You win  ",
                "A Z" or "B X" or "C Y" => " You Lose ",
                _ => throw new UnreachableException(),
            });

            var me = rule.Span[2] - 'X' + 1;
            var outcome = rule.Span switch
            {
                "A X" or "B Y" or "C Z" => 3,
                "A Y" or "B Z" or "C X" => 6,
                "A Z" or "B X" or "C Y" => 0,
                _ => throw new UnreachableException(),
            };

            sb.Append($"[{me} + {outcome} => {sum += me + outcome}]\n");
            sb.TypeString(TimeSpan.FromSeconds(1));
        }
    }

    static void DisplayPartTwo(string input)
    {
        var sum = 0;
        foreach (var rule in input.AsMemory().SplitLine().Span)
        {
            var sb = new StringBuilder();
            sb.Append(rule.Span[0] switch
            {
                'A' => "   Rock ",
                'B' => "  Paper ",
                'C' => "Scisors ",
                _ => throw new UnreachableException(),
            });
            sb.Append(" vs. ");
            sb.Append(rule.Span[2] switch
            {
                'X' => "Rock    ",
                'Y' => "Paper   ",
                'Z' => "Scisors ",
                _ => throw new UnreachableException(),
            });
            sb.Append(".");
            sb.Append(".");
            sb.Append(".");

            var loose = rule.Span[0] switch
            {
                'A' => 3,
                'B' => 1,
                'C' => 2,
                _ => throw new UnreachableException(),
            };

            var draw = rule.Span[0] - 'A' + 1;

            var win = rule.Span[0] switch
            {
                'A' => 2,
                'B' => 3,
                'C' => 1,
                _ => throw new UnreachableException(),
            };
            var outcome = rule.Span[2] switch
            {
                'X' => 0 + loose,
                'Y' => 3 + draw,
                'Z' => 6 + win,
                _ => throw new UnreachableException(),
            };

            sb.Append($"[{draw} + {outcome} => {sum += outcome}]\n");
            sb.TypeString(TimeSpan.FromSeconds(1));
        }
    }
}
