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
        public object PartOne(ReadOnlyMemory<char> input)
        {
            return 0;
        }

        public object PartTwo(ReadOnlyMemory<char> input)
        {
            return 0;
        }
    }

    """;
}
