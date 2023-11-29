using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2021;

public class Day_1 : AoCBaseDay<int, int, int[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var measurements = Helpers.File_CleanReadLines(1, 2021, resourceType)
            .Select(int.Parse)
            .ToArray();

        return new AoCSolution<int, int>(Part1(measurements), Part2(measurements));
    }

    protected override int Part1(int[] measurements)
    {
        var count = 0;

        for (var i = 1; i < measurements.Length; i++)
        {
            if (measurements[i] > measurements[i - 1])
            {
                count++;
            }
        }

        return count;
    }

    protected override int Part2(int[] measurements)
    {
        var triplets = new List<int>();

        for (var i = 2; i < measurements.Length; i++)
        {
            triplets.Add(measurements[i - 2] + measurements[i - 1] + measurements[i]);
        }

        var count = 0;

        for (var i = 1; i < triplets.Count; i++)
        {
            if (triplets[i] > triplets[i - 1])
            {
                count++;
            }
        }

        return count;
    }
}
