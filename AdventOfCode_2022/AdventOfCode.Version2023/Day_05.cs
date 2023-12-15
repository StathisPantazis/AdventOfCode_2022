using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using System.Data;

namespace AdventOfCode.Version2023;

public class Day_05 : AoCBaseDay<long, long, List<long>>
{
    private List<Map> _maps = new();

    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        _maps = Helpers.FileCleanReadText(FileDescription(this, resourceType))
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

        var seeds = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .First().Split(": ")[1].Split(' ').Select(long.Parse).ToList();

        return Solution(seeds);
    }

    protected override long Part1(List<long> seeds)
    {
        return GetBestLocation(seeds).Location;
    }

    protected override long Part2(List<long> seeds)
    {
        var seedRanges = ListBuilder.ForI(seeds.Count, 2)
            .Select(i => (seeds[i], seeds[i] + seeds[i + 1]))
            .ToList();

        var bestResult = new SearchResult(long.MaxValue, long.MaxValue);

        // Get best range
        for (var i = 0; i < seedRanges.Count; i++)
        {
            var range = seedRanges[i];
            var searchRange = ListBuilder.FromXtoN(range.Item1, range.Item2, 10000);
            var result = GetBestLocation(searchRange);

            result.SearchIndex = i;
            bestResult = result.Location < bestResult.Location ? result : bestResult;
        }

        // Pad best result
        var bestRange = ListBuilder.FromXtoN((bestResult.Seed - 10000).AtLeast(seedRanges[bestResult.SearchIndex].Item1), (bestResult.Seed + 10000).AtMost(seedRanges[bestResult.SearchIndex].Item2), 1);
        var result2 = GetBestLocation(bestRange);

        return result2.Location;
    }

    private SearchResult GetBestLocation(List<long> seeds)
    {
        var results = new List<SearchResult>();
        var startingMap = "seed";
        var total = seeds.Count;

        for (var i = 0; i < seeds.Count; i++)
        {
            var seed = seeds[i];
            var map = _maps.First(x => x.SourceName == startingMap);
            var location = seed;

            while (true)
            {
                for (var j = 0; j < map.Sources.Count; j++)
                {
                    var mapLength = map.Lengths[j];
                    var min = map.Sources[j];

                    if (location >= min && location < min + mapLength)
                    {
                        location += map.Destinations[j] - min;
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

    public class Map
    {
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public List<long> Sources { get; set; }
        public List<long> Destinations { get; set; }
        public List<long> Lengths { get; set; }

        public override string ToString() => $"{SourceName} to {DestinationName}";
    }

    private record SearchResult(long Seed, long Location)
    {
        public int SearchIndex { get; set; } = -1;
    }
}