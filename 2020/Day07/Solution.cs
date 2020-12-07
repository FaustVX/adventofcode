using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day07
{
    class Bag
    {
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
        public (int qty, Bag bag)[]? Bags { get; private set; }

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
            var matches = Regex.Match(input, @"(\w*) (\w*) bags?");
            if(_bags.TryGetValue($"{matches.Groups[1].Value} {matches.Groups[2].Value}", out var bag))
                return bag;
            return new(matches.Groups[1].Value, matches.Groups[2].Value);
        }

        public static (int, Bag) ParseWithQty(string input)
        {
            var matches = Regex.Match(input, @"(\d*) (.*)");
            return (int.Parse(matches.Groups[1].Value), Parse(matches.Groups[2].Value));
        }

        public static void Reset()
        {
            _bags.Clear();
            _containedIn.Clear();
        }
    }

    class Solution : Solver
    {
        public string Name => "Handy Haversacks";

        public IEnumerable<object> Solve(string input)
        {
            Bag.Reset();
            PrepareBags(input);
            yield return PartOne(input, "shiny gold");
            yield return PartTwo(input, "shiny gold");
        }

        int PartOne(string input, string findBag)
        {
            var bag = Bag.GetBag(findBag);
            return GetParents(bag).Skip(1).Select(bag => bag.FullAdjective).ToHashSet().Count();
        }

        int PartTwo(string input, string findBag)
        {
            var bag = Bag.GetBag(findBag);
            return GetContent(bag);
        }

        static void PrepareBags(string input)
        {
            GetBags(input).ToArray();

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
}