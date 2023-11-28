using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_1 : AoCBaseDay<string, string, int[]>
{
    public override AoCSolution<string, string> Solve(AoCResourceType resourceType)
    {
        var cals = Helpers.File_ReadText(1, 2022, resourceType)
            .Split("\n\r\n")
            .Select(x => x.Replace("\r", ""))
            .Select(x => x.Split('\n')
            .Select(y => int.Parse(y))
            .Sum())
            .ToArray();

        return new AoCSolution<string, string>(Part1(cals), Part2(cals));
    }

    protected override string Part1(int[] args)
    {
        return args.OrderByDescending(x => x).First().ToString();
    }

    protected override string Part2(int[] args)
    {
        var calcTotals = args.OrderByDescending(x => x).ToArray();
        return (calcTotals[0] + calcTotals[1] + calcTotals[2]).ToString();
    }
}
