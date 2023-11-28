using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_6 : AoCBaseDay<int, int, string>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(6, 2022, resourceType);

        return new AoCSolution<int, int>(Part1(text), Part2(text));
    }

    protected override int Part1(string args)
    {
        return SharedSolution(args, 4);
    }

    protected override int Part2(string args)
    {
        return SharedSolution(args, 14);
    }

    private static int SharedSolution(string text, int partCount)
    {
        var marker = text[..partCount];
        var meter = 0;

        while (marker.ToCharArray().Distinct().Count() < partCount)
        {
            meter++;
            marker = text.Substring(meter, partCount);
        }

        return text.IndexOf(marker) + partCount;
    }
}
