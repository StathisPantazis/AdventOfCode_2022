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

        var rows = Helpers.FileCleanReadText(FileDescription(this, resourceType))
            .Replace("......", ".").Replace(".....", ".").Replace("....", ".").Replace("...", ".").Replace("..", ".")
            .Split('\n')
            .Select(x => x.Split(' ') is string[] arr ? new Row(arr[0].Trim('.'), arr[1].Split(',').Select(y => int.Parse(y)).ToList()) : new())
            .ToList();

        rows = [rows[2]];
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

                var lala = '?'.Repeat(nextMixed.Length)
                    .GetAllCombinations('#', nextGroups.Sum(), includeBrokenPairs: true);

                var combinations = lala
                    .Where(x => x.Split('?').Length == nextGroups.Count)
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

        var results = new Dictionary<string, int>()
        {
            {"???.### 1,1,3", 1 },
            {".??..??...?##. 1,1,3", 4 },
            {"?#?#?#?#?#?#?#? 1,3,1,6", 1 },
            {"????.#...#... 4,1,1", 1 },
            {"????.######..#####. 1,6,5", 4 },
            { "?###???????? 3,2,1", 10 },
        };

        foreach (var row in rows)
        {
            if (!results.ContainsKey(row.Key))
            {
                Console.WriteLine($"Don't know about [{row}]");
            }
            else
            {
                Console.WriteLine($"{row.Key}  :  {(row.Result == results[row.Key] ? "Correct" : $"Wrong ({row.Result} instead of {results[row.Key]})")}");
            }
        }

        return default;
    }

    private static string CompleteClean(string text, List<int> groups)
    {
        while (true)
        {
            var initLength = text.Length;

            text = text.TrimStartExactlyNTimes('#', groups.FirstOrDefault(), out var trimmed);
            groups.RemoveFirstIf(trimmed);

            text = text.TrimEndExactlyNTimes('#', groups.LastOrDefault(), out trimmed);
            groups.RemoveLastIf(trimmed);

            for (var i = 0; i < 2; i++)
            {
                if (groups.Count == 0)
                {
                    break;
                }

                text = text.ReverseString();
                groups = groups.ReverseList();

                var nextMixed = text.CropUntil('.');
                var nextHashtags = nextMixed.CountFirstOccurence('#');

                if (nextHashtags == groups[0])
                {
                    text = text.CropFrom('#'.Repeat(nextHashtags)).Trim('.');
                    groups.RemoveFirstIf(true);
                }
            }

            if (initLength == text.Length || groups.Count == 0)
            {
                break;
            }
        }

        return text;
    }

    private static string CleanNextHashtag(string text, List<int> groups)
    {
        text = text.CropFrom(text.CountImmediate('#'), out var stringChanged, out var charsCropped);
        groups.RemoveFirstIf(first => stringChanged && charsCropped == first);

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
            Key = $"{InitText} {string.Join(",", InitGroups)}";
        }

        public string InitText { get; set; }
        public List<int> InitGroups { get; }
        public string Text { get; set; }
        public List<int> Groups { get; }
        public bool Done { get; set; }
        public int Result { get; set; }
        public string Key { get; set; }

        public override string ToString() => $"{InitText} ({InitText.Length}) {string.Join(",", InitGroups)} [{Result}]  - ({Text} ({Text.Length}) - [{string.Join(",", Groups)}])";
    }
}