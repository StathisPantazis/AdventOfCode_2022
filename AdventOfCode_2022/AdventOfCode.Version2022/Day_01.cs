using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_01 : AoCBaseDay<int, int, int[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var cals = Helpers.FileReadText(FileDescription(this, resourceType))
            .Split("\n\r\n")
            .Select(x => x.Replace("\r", ""))
            .Select(x => x.Split('\n')
            .Select(y => int.Parse(y))
            .Sum())
            .ToArray();

        return Solution(cals);
    }

    protected override int Part1(int[] args)
    {
        return args.OrderByDescending(x => x).First();
    }

    protected override int Part2(int[] args)
    {
        var calcTotals = args.OrderByDescending(x => x).ToArray();
        return (calcTotals[0] + calcTotals[1] + calcTotals[2]);
    }
}
