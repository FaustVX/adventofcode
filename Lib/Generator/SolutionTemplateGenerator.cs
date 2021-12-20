using AdventOfCode.Model;

namespace AdventOfCode.Generator;

class SolutionTemplateGenerator {
    public string Generate(Problem problem) {
        return $@"namespace AdventOfCode.Y{problem.Year}.Day{problem.Day:00};
             |
             |[ProblemName(""{problem.Title}"")]
             |class Solution : Solver
             |{{
             |
             |    public object PartOne(string input)
             |    {{
             |        return 0;
             |    }}
             |
             |    public object PartTwo(string input)
             |    {{
             |        return 0;
             |    }}
             |}}
             |".StripMargin();
    }
}
