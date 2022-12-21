#nullable enable
namespace AdventOfCode.Y2022.Day21;

[ProblemName("Monkey Math")]
public class Solution : Solver //, IDisplay
{
    public object PartOne(string input)
    => IMonkey.ParseMonkeys(input.AsMemory().SplitLine())["root"].Value;

    public object PartTwo(string input)
    {
        IMonkey.i++;
        var monkeys = IMonkey.ParseMonkeys(input.AsMemory().SplitLine());
        var humn = monkeys["humn"] = new Human();
        var root = new RootMonkey((OperationMonkey)monkeys["root"]);
        monkeys["root"] = root;
        root.GetHumanValue();
        if (root.LeftMonkey.Value == root.RightMonkey.Value)
            return humn.Value;
        else
            throw new UnreachableException($"{root.LeftMonkey.Value} != {root.RightMonkey.Value}");
    }
}
