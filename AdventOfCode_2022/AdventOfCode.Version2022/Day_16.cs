using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_16 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_ReadText(FileDescription(this, resourceType))
            .Replace("valves", "valve")
            .Replace("leads", "lead")
            .Replace("tunnels", "tunnel")
            .Replace(" has flow rate=", ",")
            .Replace("; tunnel lead to valve ", ",")
            .Replace("Valve ", "").Replace(" ", "");

        return default;
    }

    protected override int Part1(string[] args)
    {
        return 0;
    }

    protected override int Part2(string[] args)
    {
        return 0;
    }
}