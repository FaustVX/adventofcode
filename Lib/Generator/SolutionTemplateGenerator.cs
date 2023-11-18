using AdventOfCode.Model;

namespace AdventOfCode.Generator;

#if !LIBRARY
[DebuggerStepThrough]
#endif
internal static class SolutionTemplateGenerator
{
    public static string Generate(Problem problem)
    => $$"""
    #nullable enable
    namespace AdventOfCode.Y{{problem.Year}}.Day{{problem.Day:00}};

    [ProblemName("{{problem.Title}}")]
    public class Solution : {{nameof(ISolver)}} //, {{nameof(IDisplay)}}
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
