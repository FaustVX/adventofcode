using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using AngleSharp.Text;

namespace AdventOfCode.Y2021.Day12;

[ProblemName("Passage Pathing")]
class Solution : Solver
{
    record class Cave(string Name)
    {
        public bool IsBig { get; } = Name[0].IsUppercaseAscii();
        public int Visited { get; set; }
        public int MaxVisit { get; set; } = 1;
        public List<Cave> Related { get; } = new();

        public IEnumerable<Cave> NavigateCaves()
            => Related.Where(static cave => cave.IsBig || cave.Visited < cave.MaxVisit);

        public void AddRelated(Cave cave)
        {
            Related.Add(cave);
            cave.Related.Add(this);
        }

        public override string ToString()
            => $"{Name} - {{{string.Join(", ", Related.Select(static cave => cave.Name))}}}";
    }

    private static (Cave start, Cave end, Dictionary<string, Cave> caves) Parse(string input)
    {
        var start = new Cave("start");
        var caves = new Dictionary<string, Cave>()
        {
            [start.Name] = start,
        };
        var relations = new List<(string, string)>(input.SplitLine()
            .Select(static line => line.Split('-'))
            .Select(static line => (line[0], line[1])));
        AddRelative(start, relations, caves);
        return (start, caves["end"], caves);

        static void AddRelative(Cave cave, List<(string, string)> list, Dictionary<string, Cave> caves)
        {
            for (int i = list.Count - 1; i >= 0 ; i--)
            {
                if (!TryGetName(cave, list, i, out var name))
                    continue;

                var related = caves[name] = caves.ContainsKey(name) ? caves[name] : new(name);
                cave.AddRelated(related);
                list.RemoveAt(i);
                AddRelative(related, list, caves);
                i = Math.Min(i, list.Count);
            }

            static bool TryGetName(Cave cave, List<(string, string)> list, int i, out string name)
            {
                var (a, b) = list[i];
                if (a == cave.Name)
                {
                    name = b;
                    return true;
                }
                if (b == cave.Name)
                {
                    name = a;
                    return true;
                }
                name = default;
                return false;
            }
        }
    }

    public object PartOne(string input)
    {
        var (start, end, _) = Parse(input);

        var paths = new List<Cave[]>();
        AllDepthFirstSearch(start, end, new(), paths);
        return paths.Count;
    }

    private static void AllDepthFirstSearch(Cave entrance, Cave end, LinkedList<Cave> path, List<Cave[]> paths)
    {
        if (entrance.Name == end.Name)
        {
            var array = path.ToArray();
            if (!paths.Any(p => p.SequenceEqual(array)))
                paths.Add(array);
            return;
        }
        entrance.Visited++;
        foreach (var cave in entrance.NavigateCaves())
        {
            path.AddLast(cave);
            AllDepthFirstSearch(cave, end, path, paths);
            path.RemoveLast();
        }
        entrance.Visited--;
    }

    public object PartTwo(string input)
    {
        var (start, end, caves) = Parse(input);

        var paths = new List<Cave[]>();
        foreach (var cave in caves.Values.Where(static cave => !cave.IsBig))
        {
            if (cave.Name == start.Name || cave.Name == end.Name)
                continue;
            cave.MaxVisit++;
            AllDepthFirstSearch(start, end, new(), paths);
            cave.MaxVisit--;
        }
        return paths.Count;
    }
}
