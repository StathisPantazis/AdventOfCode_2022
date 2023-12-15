using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_12 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        Console.Clear();

        var rows = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Split(' ') is string[] arr ? new Row(arr[0], arr[1].Split(',').Select(y => int.Parse(y)).ToList()) : new())
            .ToList();

        rows = new List<Row>() { rows[1] };
        //rows = new List<Row>() { rows.First(x => x.Arrangement.StartsWith("?#?#")) };
        //rows = new List<Row>() { rows.First(x => x.Arrangement == ".???..??...?##.") };

        foreach (var row in rows)
        {
            var init = row.ToString();

            Console.WriteLine();
            Console.WriteLine();

            var choppedTexts = new List<string>();

            while (row.Text.Length > 0 && row.Groups.Count > 0)
            {
                Console.WriteLine("\nLoop entry...");
                Console.WriteLine(row);
                var startingLength = row.Text.Length;

                Console.WriteLine("Cleaning...");
                row.Text = CompleteClean(row.Text, row.Groups);

                Console.WriteLine(row);

                var nextMixed = row.Text.CropUntil('.');
                var nextMixedLength = nextMixed.Length;

                var nextGroups = new List<int>();

                foreach (var group in row.Groups)
                {
                    if (group <= nextMixedLength)
                    {
                        nextMixedLength -= group;
                        nextGroups.Add(group);
                    }
                }

                var combinations = nextMixed
                    .GetAllCombinations('#', nextGroups.Sum(), includeBrokenPairs: true)
                    .Where(x => x.Split('?', StringSplitOptions.RemoveEmptyEntries).Length == nextGroups.Count)
                    .ToList();

                row.Result += combinations.Count;
                row.Text = row.Text.RemoveOnce(nextMixed);
                nextGroups.ForEachDo(x => row.Groups.Remove(x));
            }

            Console.WriteLine("\n-----RESULT------");
            Console.WriteLine(row);
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        //Console.WriteLine(rows[0].Result == 1);
        //Console.WriteLine(rows[1].Result == 4);
        //Console.WriteLine(rows[2].Result == 1);
        //Console.WriteLine(rows[3].Result == 1);
        //Console.WriteLine(rows[4].Result == 4);
        //Console.WriteLine(rows[5].Result == 10);

        return default;
    }

    private string CompleteClean(string text, List<int> groups)
    {
        // Clean back first
        text = text.ReverseString();
        groups.Reverse();

        text = text.TrimStart('.');
        text = CleanNextHashtag(text, groups);
        text = text.TrimStart('.');

        // Then clean front
        text = text.ReverseString();
        groups.Reverse();

        text = text.TrimStart('.');
        text = CleanNextHashtag(text, groups);
        text = text.TrimStart('.');

        return text;
    }

    private string CleanNextHashtag(string text, List<int> groups)
    {
        text = text.CropFrom(text.CountImmediate('#'), out var stringChanged, out var charsCropped);

        if (stringChanged && charsCropped == groups[0])
        {
            groups.RemoveAt(0);
        }

        return text;
    }

    protected override int Part1(string[] args)
    {
        return 0;
    }

    protected override int Part2(string[] args)
    {
        return 0;
    }

    private record Possibility(string Text, List<int> Ways)
    {
        public override string ToString() => $"{Text} - {string.Join(",", Ways)}";
    }

    private class Row
    {
        public Row() { }

        public Row(string arrangement, List<int> groups)
        {
            InitText = arrangement;
            Text = arrangement;
            InitGroups = [.. groups];
            Groups = groups;
        }

        public string InitText { get; set; }
        public List<int> InitGroups { get; }
        public string Text { get; set; }
        public List<int> Groups { get; }
        public bool Done { get; set; }
        public int Result { get; set; }

        public override string ToString() => $"{InitText} {string.Join(",", InitGroups)} [{Result}]  - ({Text} - {Text.Length} - [{string.Join(",", Groups)}])";
    }
}