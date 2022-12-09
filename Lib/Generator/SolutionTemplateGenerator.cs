using AdventOfCode.Model;

namespace AdventOfCode.Generator;

static class SolutionTemplateGenerator {
    public static string Generate(Problem problem)
        => $$"""
#nullable enable
namespace AdventOfCode.Y{{problem.Year}}.Day{{problem.Day:00}};

[ProblemName("{{problem.Title}}")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    {
        return 0;
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}

""";
}
