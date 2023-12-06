using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_6 : AoCBaseDay<int, int, string>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var input = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Replace("Time: ", "")
            .Replace("Distance: ", "");

        return Solution(input);
    }

    protected override int Part1(string input)
    {
        var races = input
            .Split("\n")
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y.ToString())).ToList())
            .RotateRowsColumns()
            .Select(x => (x[0], x[1]))
            .ToList();

        var waysToWin = Enumerable.Range(0, races.Count).Select(x => 0).ToList();

        for (var i = 0; i < races.Count; i++)
        {
            var (duration, record) = races[i];

            for (var holdSeconds = 1; holdSeconds < duration; holdSeconds++)
            {
                waysToWin[i] += (duration - holdSeconds) * holdSeconds > record ? 1 : 0;
            }
        }

        return waysToWin.Multiply();
    }

    protected override int Part2(string input)
    {
        (var duration, var recordDistance) = input
            .Replace(" ", "")
            .Split("\n")
            .Select(x => long.Parse(x.ToString()))
            .ToList()
            is List<long> list ? (list[0], list[1]) : default;

        var waysToWin = 0;

        for (var holdSeconds = 1; holdSeconds < duration; holdSeconds++)
        {
            waysToWin += (duration - holdSeconds) * holdSeconds > recordDistance ? 1 : 0;
        }

        return waysToWin;
    }
}