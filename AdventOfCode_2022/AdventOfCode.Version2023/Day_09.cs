using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_09 : AoCBaseDay<long, long, List<List<List<long>>>>
{
    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var difs = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x.ToString())).ToList())
            .Select(x => new List<List<long>> { x, GetDifferences(x) })
            .ToList();

        foreach (var dif in difs)
        {
            while (!dif.Last().All(x => x == 0))
            {
                dif.Add(GetDifferences(dif.Last()));
            }

            for (var j = dif.Count - 2; j > -1; j--)
            {
                var lastNum = dif[j].Last() + dif[j + 1].Last();
                dif[j].Add(lastNum);

                var firstNum = dif[j].First() - dif[j + 1].First();
                dif[j].Insert(0, firstNum);
            }
        }

        return Solution(difs);
    }

    protected override long Part1(List<List<List<long>>> difs)
    {
        return difs.Sum(x => x.First().Last());
    }

    protected override long Part2(List<List<List<long>>> difs)
    {
        return difs.Sum(x => x.First().First());
    }

    private static List<long> GetDifferences(List<long> numbers)
    {
        return ListBuilder.ForI(numbers.Count - 1)
            .Select(i => numbers[i + 1] - numbers[i])
            .ToList();
    }
}
