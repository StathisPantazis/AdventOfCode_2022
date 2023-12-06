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
        var lines = input
            .Split("\n")
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y.ToString())).ToList())
            .ToList();

        var races = new List<(int duration, int record)>();

        for (var i = 0; i < lines[0].Count; i++)
        {
            races.Add(new(lines[0][i], lines[1][i]));
        }

        var waysToWin = Enumerable.Range(0, races.Count).Select(x => 0).ToList();

        for (var i = 0; i < races.Count; i++)
        {
            var (duration, record) = races[i];

            for (var holdSeconds = 1; holdSeconds < duration; holdSeconds++)
            {
                if ((duration - holdSeconds) * holdSeconds > record)
                {
                    waysToWin[i] += 1;
                }
            }
        }

        return waysToWin.Multiply();
    }

    protected override int Part2(string input)
    {
        var lines = input
            .Replace(" ", "")
            .Split("\n")
            .Select(x => long.Parse(x.ToString()))
            .ToList();

        var duration = lines[0];
        var recordDistance = lines[1];
        var waysToWin = 0;

        for (var holdSeconds = 1; holdSeconds < duration; holdSeconds++)
        {
            if ((duration - holdSeconds) * holdSeconds > recordDistance)
            {
                waysToWin++;
            }
        }

        return waysToWin;
    }
}