using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2021;

public class Day_7 : AoCBaseDay<int, int, Dictionary<int, int>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var positions = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Split(',')
            .Select(int.Parse)
            .ToList();

        var allPositionsMinMax = ListBuilder.IntRange(positions.Min(), positions.Max() - positions.Min() + 1);
        var positionCrabsDict = allPositionsMinMax.ToDictionary(x => x, x => positions.Count(c => c == x));

        return Solution(positionCrabsDict);
    }

    protected override int Part1(Dictionary<int, int> dict)
    {
        var shortestFuel = int.MaxValue;

        foreach (var position in dict.Keys)
        {
            var fuel = 0;

            foreach (var key in dict.Keys)
            {
                fuel += Math.Abs(key - position) * dict[key];
            }

            if (fuel < shortestFuel)
            {
                shortestFuel = fuel;
            }
        }

        return shortestFuel;
    }

    protected override int Part2(Dictionary<int, int> positionCrabsDict)
    {
        var moveCostDict = positionCrabsDict.Keys.ToDictionary(x => x, x => 0);

        foreach (var moves in moveCostDict.Keys)
        {
            var distance = moves;

            var cost = 0;
            var steps = 0;

            while (distance > 0)
            {
                steps++;

                cost += steps;
                distance--;
            }

            moveCostDict[moves] = cost;
        }

        var shortestFuel = int.MaxValue;

        foreach (var position in positionCrabsDict.Keys)
        {
            var fuel = 0;

            foreach (var crabPosition in positionCrabsDict.Keys)
            {
                fuel += moveCostDict[Math.Abs(crabPosition - position)] * positionCrabsDict[crabPosition];
            }

            if (fuel < shortestFuel)
            {
                shortestFuel = fuel;
            }
        }

        return shortestFuel;
    }
}