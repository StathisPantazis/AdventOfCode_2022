using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_8 : AoCBaseDay<long, long, (Dictionary<string, string[]> maps, List<int> moves)>
{
    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Replace(" = (", ",").Replace(", ", ",").Replace(")", "");

        var lines = Helpers.Text_CleanReadLines(text);

        var moves = lines[0].Replace("L", "0").Replace("R", "1").Select(x => int.Parse(x.ToString())).ToList();
        var maps = lines
            .Skip(2)
            .Select(x => x.Split(','))
            .ToDictionary(x => x[0], x => new string[] { x[1], x[2] });

        return Solution((maps, moves));
    }

    protected override long Part1((Dictionary<string, string[]> maps, List<int> moves) args)
    {
        var maps = args.maps;
        var moves = args.moves;

        var moveIndex = 0;
        var steps = 0;
        var dest = "AAA";

        while (dest != "ZZZ")
        {
            steps++;
            var move = moves[moveIndex];
            dest = maps[dest][move];
            moveIndex = moveIndex == moves.Count - 1 ? 0 : moveIndex + 1;
        }

        return steps;
    }

    protected override long Part2((Dictionary<string, string[]> maps, List<int> moves) args)
    {
        var maps = args.maps;
        var moves = args.moves;

        var moveIndex = 0;
        var steps = 0;
        var destinations = maps.Keys.Where(x => x.EndsWith("A")).ToList();
        var start = destinations.ToList();
        var z_repeats = destinations.ToDictionary(x => x, x => (long)0);

        while (z_repeats.Any(x => x.Value == 0))
        {
            steps++;
            var move = moves[moveIndex];
            destinations = destinations.Select(dest => maps[dest][move]).ToList();

            moveIndex = moveIndex == moves.Count - 1 ? 0 : moveIndex + 1;

            if (destinations.Any(x => x.EndsWith("Z")))
            {
                foreach (var dest in destinations.Where(x => x.EndsWith("Z")).ToList())
                {
                    var key = start[destinations.IndexOf(dest)];
                    z_repeats[key] = z_repeats[key] == 0 ? steps : z_repeats[key];
                }
            }
        }

        return z_repeats.Values.LeastCommonMultiple();
    }
}
