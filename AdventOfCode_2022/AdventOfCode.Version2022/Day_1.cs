using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_1 : AoCBaseDay<string, string, string[]>
{
    public override AoCSolution<string, string> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_ReadText(1, 2022, resourceType);

        var cals = text.Split("\n\r\n").Select(x => x.Replace("\r", ""));
        var calsTotals = cals.Select(x => x.Split('\n').Select(y => int.Parse(y)).Sum());
        var calsTotals2 = calsTotals.OrderByDescending(x => x).ToArray();

        Console.WriteLine(calsTotals2[0] + calsTotals2[1] + calsTotals2[2]);

        var calss = Helpers.File_ReadText(1, 2022, resourceType)
            .Split("\n\r\n")
            .Select(x => x.Replace("\r", ""))
            .ToArray();

        return new AoCSolution<string, string>(Part1(calss), Part2(calss));
    }

    protected override string Part1(string[] args)
    {
        return args.Select(x => x.Split('\n').Select(y => int.Parse(y)).Sum()).First().ToString();
    }

    protected override string Part2(string[] args)
    {
        var calcTotals = args.OrderByDescending(x => x).ToArray();
        return calcTotals[0] + calcTotals[1] + calcTotals[2];
    }
}
