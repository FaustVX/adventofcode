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
    namespace {{nameof(AdventOfCode)}}.Y{{problem.Year}}.Day{{problem.Day:00}};

    [{{nameof(ProblemInfo)}}("{{problem.Title}}")]
    public class Solution : {{nameof(ISolver)}} //, {{nameof(IDisplay)}}
    {
        public object {{nameof(ISolver.PartOne)}}({{nameof(ReadOnlyMemory<char>)}}<char> input)
        {
            return 0;
        }

        public object {{nameof(ISolver.PartTwo)}}({{nameof(ReadOnlyMemory<char>)}}<char> input)
        {
            return 0;
        }
    }

    """;
}
