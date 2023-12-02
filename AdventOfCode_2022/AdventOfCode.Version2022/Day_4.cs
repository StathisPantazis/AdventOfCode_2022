using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_4 : AoCBaseDay<int, int, List<int[]>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Replace(",", "-");

        var pairs = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split('-').Select(y => int.Parse(y.ToString())).ToArray())
            .ToList();

        return Solution(pairs);
    }

    protected override int Part1(List<int[]> args)
    {
        return args.Count(x => (x[0] <= x[2] && x[1] >= x[3]) || (x[2] <= x[0] && x[3] >= x[1]));
    }

    protected override int Part2(List<int[]> args)
    {
        return args.Count(x => (x[0] <= x[2] && x[1] >= x[2]) || (x[2] <= x[0] && x[3] >= x[0]));
    }
}
