using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using System.Data;
using static AdventOfCode.Version2023.Day_5;

namespace AdventOfCode.Version2023;

public class Day_5 : AoCBaseDay<long, long, (List<Map> maps, List<long> seeds)>
{
    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var maps = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Split("\n\n")
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

        foreach (var map in maps)
        {
            for (var i = 0; i < map.Sources.Count; i++)
            {
                var source = map.Sources[i];
                var dest = map.Destinations[i];

                map.Distances.Add(dest - source);
                map.SourceGreaterThanDest.Add(dest > source);
            }
        }

        var lines = Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .First().Split(": ")[1].Split(' ').Select(long.Parse).ToList();

        //var part1 = Part1((maps, lines));
        var part2 = Part2((maps, lines));

        //Console.WriteLine($"Part 1: {part1}\n");
        //Console.WriteLine($"Part 2: {part2}");

        return default;
    }

    protected override long Part1((List<Map> maps, List<long> seeds) args)
    {
        return GetBestLocation(args.seeds, args.maps).location;
    }

    protected override long Part2((List<Map> maps, List<long> seeds) args)
    {
        var seedRanges = ListBuilder.RangeFromTo(0, args.seeds.Count, 2)
            .Select(i => ListBuilder.RangeFromNTimes(args.seeds[i], args.seeds[i + 1]))
            .ToList();

        seedRanges.Print();

        //var ranges = new List<SeedPair>();

        //for (var i = 0; i < seedPairs.Count; i++)
        //{
        //    var pair = seedPairs[i];

        //    if (ranges.FirstOrDefault(x => pair.Start <= x.Start && x.Start < pair.End) is SeedPair startIncuded)
        //    {
        //        startIncuded.Start = pair.Start;
        //    }

        //    if (ranges.FirstOrDefault(x => pair.Start <= x.End && x.End < pair.End) is SeedPair endIncuded)
        //    {
        //        endIncuded.End = pair.End;
        //    }

        //    if (ranges.None(x => x.Start <= pair.Start && pair.End <= x.End))
        //    {
        //        ranges.Add(new SeedPair(pair.Start, pair.End));
        //    }
        //}

        //seeds = ListBuilder.NumRange(result.seed - 30000, result.seed + 30000);
        //result = GetBestLocation(seeds, args.maps);

        //var minus1 = SingleSolve(result.seed - 1, args.maps);
        //var plus1 = SingleSolve(result.seed - 1, args.maps);

        //Console.WriteLine($"-2: {SingleSolve(result.seed - 2, args.maps)}");
        //Console.WriteLine($"-1: {minus1}");
        //Console.WriteLine($"0: {result.location}");
        //Console.WriteLine($"+1: {plus1}");
        //Console.WriteLine($"+2: {SingleSolve(result.seed + 2, args.maps)}");
        //Console.WriteLine($"+3: {SingleSolve(result.seed + 3, args.maps)}");
        //Console.WriteLine($"+4: {SingleSolve(result.seed + 4, args.maps)}");
        //Console.WriteLine($"+5: {SingleSolve(result.seed + 5, args.maps)}");
        //Console.WriteLine($"+6: {SingleSolve(result.seed + 6, args.maps)}");

        return default;
    }

    private static long SingleSolve(long seed, List<Map> maps)
    {
        var map = maps.First(x => x.SourceName == "seed");
        var location = seed;

        while (true)
        {
            for (var i = 0; i < map.Sources.Count; i++)
            {
                var sourceGreaterThanDest = map.SourceGreaterThanDest[i];
                var mapLength = map.Lengths[i];
                var min = map.Sources[i];

                if (location >= min && location <= min + mapLength)
                {
                    location += map.Distances[i];
                    break;
                }
            }

            if (map.DestinationName == "location")
            {
                break;
            }

            map = maps.First(x => x.SourceName == map.DestinationName);
        }

        return location;
    }

    private static (long location, long seed) GetBestLocation(List<long> seeds, List<Map> maps)
    {
        var locations_seed = new List<(long location, long seed)>();
        var startingMap = "seed";

        foreach (var seed in seeds)
        {
            var map = maps.First(x => x.SourceName == startingMap);
            var source = seed;

            while (true)
            {
                for (var i = 0; i < map.Sources.Count; i++)
                {
                    var sourceGreaterThanDest = map.SourceGreaterThanDest[i];
                    var mapLength = map.Lengths[i];
                    var min = map.Sources[i];

                    if (source >= min && source <= min + mapLength)
                    {
                        source += map.Distances[i];
                        break;
                    }
                }

                if (map.DestinationName == "location")
                {
                    break;
                }

                map = maps.First(x => x.SourceName == map.DestinationName);
            }

            locations_seed.Add(new(source, seed));
        }

        return locations_seed.OrderBy(x => x.location).First();
    }

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
}