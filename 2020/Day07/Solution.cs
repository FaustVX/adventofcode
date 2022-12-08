using RegExtract;

namespace AdventOfCode.Y2020.Day07;

class Bag
{
    private static readonly Regex _parser2 = new(@"(\w*) (\w*) bags?");
    private static readonly Regex _parserQty = new(@"(\d*) (.*)");
    public Bag(string adjective, string color)
    {
        Adjective = adjective;
        Color = color;
        _bags[FullAdjective] = this;
        _containedIn[this] = new();
    }

    public static Bag GetBag(string fullAdjective)
        => _bags[fullAdjective];

    public static IReadOnlyList<Bag> GetBags(Bag bag)
        => _containedIn[bag];

    private static readonly Dictionary<string, Bag> _bags = new(33 * 18 + 50); // ~33 Adjectives * ~18 Colors
    private static readonly Dictionary<Bag, List<Bag>> _containedIn = new();
    public string FullAdjective => $"{Adjective} {Color}";

    public string Adjective { get; }
    public string Color { get; }
    public (int qty, Bag bag)[] Bags { get; private set; }

    public void SetBag((int qty, Bag bag)[] bags)
    {
        Bags = bags;
        foreach (var bag in bags)
        {
            _containedIn[bag.bag].Add(this);
        }
    }

    public static Bag Parse(string input)
    {
        var (adj, col) = input.Extract<(string, string)>(_parser2);
        if (_bags.TryGetValue($"{adj} {col}", out var bag))
            return bag;
        return new(adj, col);
    }

    public static (int, Bag) ParseWithQty(string input)
        => input.Extract<(int, Bag)>(_parserQty);

    public static void Reset()
    {
        _bags.Clear();
        _containedIn.Clear();
    }
}

[ProblemName("Handy Haversacks")]
public class Solution : Solver
{
    public object PartOne(string input)
    {
        Bag.Reset();
        PrepareBags(input);
        return Solve1("shiny gold");
    }

    public object PartTwo(string input)
    {
        Bag.Reset();
        PrepareBags(input);
        return Solve2("shiny gold");
    }

    static int Solve1(string findBag)
    {
        var bag = Bag.GetBag(findBag);
        return GetParents(bag).Skip(1).Select(bag => bag.FullAdjective).ToHashSet().Count;
    }

    static int Solve2(string findBag)
    {
        var bag = Bag.GetBag(findBag);
        return GetContent(bag);
    }

    static void PrepareBags(string input)
    {
        _ = GetBags(input).ToArray();

        static IEnumerable<Bag> GetBags(string input)
        {
            foreach (var rule in input.SplitLine())
            {
                var splitted = rule[..^1].Split(" contain ");
                var bag = Bag.Parse(splitted[0]);
                if (splitted[1] is not "no other bags")
                {
                    var contain = splitted[1].Split(", ").Select(Bag.ParseWithQty).ToArray();
                    bag.SetBag(contain);
                }
                yield return bag;
            }
        }
    }

    static IEnumerable<Bag> GetParents(Bag bag)
    {
        yield return bag;

        foreach (var item in Bag.GetBags(bag).SelectMany(GetParents))
            yield return item;
    }

    static int GetContent(Bag bag)
        => bag.Bags?.Aggregate(0, (o, c) => o += GetContent(c.bag) * c.qty + c.qty) ?? 0;
}
