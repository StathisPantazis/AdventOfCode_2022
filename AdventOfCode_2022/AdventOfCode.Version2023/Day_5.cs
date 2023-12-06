using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using System.Data;

namespace AdventOfCode.Version2023;

public class Day_5 : AoCBaseDay<long, long, List<long>>
{
    private List<Map> _maps = new();

    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        _maps = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(x => x.Split('\n').ToList() is List<string> descriptions ? new Map()
            {
                SourceName = descriptions[0].Split('-') is string[] s ? s[0] : string.Empty,
                DestinationName = descriptions[0].Split('-') is string[] d ? d[2].Split(' ')[0] : string.Empty,
                Destinations = descriptions.Skip(1).Select(y => long.Parse(y.Split(' ')[0])).ToList(),
                Sources = descriptions.Skip(1).Select(y => long.Parse(y.Split(' ')[1])).ToList(),
                Lengths = descriptions.Skip(1).Select(y => long.Parse(y.Split(' ')[2])).ToList(),
            } : new())
            .ToList();

        foreach (var map in _maps)
        {
            for (var i = 0; i < map.Sources.Count; i++)
            {
                var source = map.Sources[i];
                var dest = map.Destinations[i];

                map.Distances.Add(dest - source);
                map.SourceGreaterThanDest.Add(dest > source);
            }
        }

        var seeds = Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .First().Split(": ")[1].Split(' ').Select(long.Parse).ToList();

        Console.WriteLine(Part1(seeds));

        return Solution(seeds);
    }

    protected override long Part1(List<long> seeds)
    {
        return GetBestLocation(seeds).Location;
    }

    protected override long Part2(List<long> seeds)
    {
        var seedRanges = ListBuilder.RangeFromTo(0, seeds.Count, 2)
            .Select(i => (seeds[i], seeds[i] + seeds[i + 1]))
            .ToList();

        var bestResult = new SearchResult(long.MaxValue, long.MaxValue);

        Console.WriteLine("Getting best range...");
        // Get best range
        for (var i = 0; i < seedRanges.Count; i++)
        {
            var range = seedRanges[i];
            var searchRange = ListBuilder.RangeFromTo(range.Item1, range.Item2, StepConsideringExample(range.Item1, 10000));
            var result = GetBestLocation(searchRange);
            result.SearchIndex = i;
            Console.WriteLine(result);

            if (result.Location < bestResult.Location)
            {
                bestResult = result;
            }
        }
        Console.WriteLine("\nSelecting best result...");
        Console.WriteLine(bestResult);

        // Filter best range
        Console.WriteLine("\nFiltering best range...");
        var range1 = seedRanges[bestResult.SearchIndex];
        var searchRange1 = ListBuilder.RangeFromTo(range1.Item1, range1.Item2, StepConsideringExample(range1.Item1, 9999));
        var result1 = GetBestLocation(searchRange1);
        Console.WriteLine($"From: {result1}");

        if (result1.Location < bestResult.Location)
        {
            bestResult = result1;
        }
        Console.WriteLine($"To: {bestResult}");

        // Pad best result
        Console.WriteLine("\nPadding result...");
        var searchRange2 = ListBuilder.RangeFromTo((bestResult.Seed - 10000).AtLeast(range1.Item1), (bestResult.Seed + 10000).LimitBy(range1.Item2), 1);
        var result2 = GetBestLocation(searchRange2);

        Console.WriteLine("\nBest Solution:");
        Console.WriteLine(result2);

        return default;
    }

    private SearchResult GetBestLocation(List<long> seeds)
    {
        var results = new List<SearchResult>();
        var startingMap = "seed";

        for (var i = 0; i < seeds.Count; i++)
        {
            var seed = seeds[i];
            var map = _maps.First(x => x.SourceName == startingMap);
            var location = seed;

            while (true)
            {
                for (var j = 0; j < map.Sources.Count; j++)
                {
                    var sourceGreaterThanDest = map.SourceGreaterThanDest[j];
                    var mapLength = map.Lengths[j];
                    var min = map.Sources[j];

                    if (location >= min && location <= min + mapLength)
                    {
                        location += map.Distances[j];
                        break;
                    }
                }

                if (map.DestinationName == "location")
                {
                    break;
                }

                map = _maps.First(x => x.SourceName == map.DestinationName);
            }

            results.Add(new(seed, location));
        }

        return results.OrderBy(x => x.Location).First();
    }

    private static int StepConsideringExample(long start, int step) => start < 100 ? 1 : step;

    public class Map
    {
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public List<long> Sources { get; set; }
        public List<long> Destinations { get; set; }
        public List<long> Lengths { get; set; }
        public List<long> Distances { get; set; } = new();
        public List<bool> SourceGreaterThanDest { get; set; } = new();

        public override string ToString() => $"{SourceName} to {DestinationName}";
    }

    private record SearchResult(long Seed, long Location)
    {
        public int SearchIndex { get; set; } = -1;
    }
}