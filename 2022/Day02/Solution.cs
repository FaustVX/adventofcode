namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
public class Solution : Solver, IDisplay
{
    public object PartOne(string input)
    {
        var sum = 0;
        foreach (var rule in input.SplitLine())
        {
            // A X : Rock
            // B Y : Paper
            // C Z : Scissors

            var me = rule[2] - 'X' + 1;
            sum += me + rule switch
            {
                ['A', _, 'X'] or ['B', _, 'Y'] or ['C', _, 'Z'] => 3,
                ['A', _, 'Y'] or ['B', _, 'Z'] or ['C', _, 'X'] => 6,
                ['A', _, 'Z'] or ['B', _, 'X'] or ['C', _, 'Y'] => 0,
            };
        }
        return sum;
    }

    public object PartTwo(string input)
    {
        var sum = 0;
        foreach (var rule in input.SplitLine())
        {
            // A : Rock
            // B : Paper
            // C : Scissors

            var loose = rule[0] switch
            {
                'A' => 3,
                'B' => 1,
                'C' => 2,
            };

            var draw = rule[0] - 'A' + 1;

            var win = rule[0] switch
            {
                'A' => 2,
                'B' => 3,
                'C' => 1,
            };
            sum += rule[2] switch
            {
                'X' => 0 + loose,
                'Y' => 3 + draw,
                'Z' => 6 + win,
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
        foreach (var rule in input.SplitLine())
        {
            var sb = new StringBuilder();
            sb.Append(rule[0] switch
            {
                'A' => "   Rock ",
                'B' => "  Paper ",
                'C' => "Scisors ",
            });
            sb.Append(" vs. ");
            sb.Append(rule[2] switch
            {
                'X' => "Rock    ",
                'Y' => "Paper   ",
                'Z' => "Scisors ",
            });
            sb.Append(".");
            sb.Append(".");
            sb.Append(".");
            sb.Append(rule switch
            {
                ['A', _, 'X'] or ['B', _, 'Y'] or ['C', _, 'Z'] => " Draw     ",
                ['A', _, 'Y'] or ['B', _, 'Z'] or ['C', _, 'X'] => " You win  ",
                ['A', _, 'Z'] or ['B', _, 'X'] or ['C', _, 'Y'] => " You Lose ",
            });

            var me = rule[2] - 'X' + 1;
            var outcome = rule switch
            {
                ['A', _, 'X'] or ['B', _, 'Y'] or ['C', _, 'Z'] => 3,
                ['A', _, 'Y'] or ['B', _, 'Z'] or ['C', _, 'X'] => 6,
                ['A', _, 'Z'] or ['B', _, 'X'] or ['C', _, 'Y'] => 0,
            };

            sb.Append($"[{me} + {outcome} => {sum += me + outcome}]\n");
            sb.TypeString(TimeSpan.FromSeconds(1));
        }
    }

    static void DisplayPartTwo(string input)
    {
        var sum = 0;
        foreach (var rule in input.SplitLine())
        {
            var sb = new StringBuilder();
            sb.Append(rule[0] switch
            {
                'A' => "   Rock ",
                'B' => "  Paper ",
                'C' => "Scisors ",
            });
            sb.Append(" vs. ");
            sb.Append(rule[2] switch
            {
                'X' => "Rock    ",
                'Y' => "Paper   ",
                'Z' => "Scisors ",
            });
            sb.Append(".");
            sb.Append(".");
            sb.Append(".");

            var loose = rule[0] switch
            {
                'A' => 3,
                'B' => 1,
                'C' => 2,
            };

            var draw = rule[0] - 'A' + 1;

            var win = rule[0] switch
            {
                'A' => 2,
                'B' => 3,
                'C' => 1,
            };
            var outcome = rule[2] switch
            {
                'X' => 0 + loose,
                'Y' => 3 + draw,
                'Z' => 6 + win,
            };

            sb.Append($"[{draw} + {outcome} => {sum += outcome}]\n");
            sb.TypeString(TimeSpan.FromSeconds(1));
        }
    }
}
