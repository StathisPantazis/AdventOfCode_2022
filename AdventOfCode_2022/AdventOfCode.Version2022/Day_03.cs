using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_03 : AoCBaseDay<int, int, (string[] comps, List<char> types)>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var comps = Helpers.FileCleanReadLines(FileDescription(this, resourceType));
        var types = Enumerable.Range('a', 26).ToList().Union(Enumerable.Range('A', 26)).Select(x => (char)x).ToList();

        return Solution((comps, types));
    }

    protected override int Part1((string[] comps, List<char> types) args)
    {
        var sum = 0;

        foreach (var comp in args.comps)
        {
            var pt1 = comp[..(comp.Length / 2)];
            var pt2 = comp[(comp.Length / 2)..];
            var dif = pt1.FirstOrDefault(pt2.Contains);
            sum += args.types.IndexOf(dif) + 1;
        }

        return sum;
    }

    protected override int Part2((string[] comps, List<char> types) args)
    {
        var sum = 0;

        for (var i = 0; i < args.comps.Length; i += 3)
        {
            var pt1 = args.comps[i].Select(x => x).ToList();
            var pt2 = args.comps[i + 1].Select(x => x).ToList();
            var pt3 = args.comps[i + 2].Select(x => x).ToList();

            var same = pt1.Intersect(pt2).Intersect(pt3).First();
            sum += args.types.IndexOf(same) + 1;
        }

        return sum;
    }
}
