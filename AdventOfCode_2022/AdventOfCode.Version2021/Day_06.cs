using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2021;

public class Day_06 : AoCBaseDay<double, double, string[]>
{
    public override AoCSolution<double, double> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.FileCleanReadText(FileDescription(this, resourceType)).Split(',');
        return Solution(lines);
    }

    protected override double Part1(string[] lines)
    {
        var fishes = lines
          .Select(x => new Lanternfish(int.Parse(x)))
          .ToList();

        var days = 80;

        for (var i = 0; i < days; i++)
        {
            var newFishes = new List<Lanternfish>();

            foreach (var fish in fishes)
            {
                if (fish.DayPassed() is Lanternfish newFish)
                {
                    newFishes.Add(newFish);
                }
            }

            fishes.AddRange(newFishes);
        }

        return fishes.Count;
    }

    protected override double Part2(string[] lines)
    {
        var fishes = lines.Select(int.Parse).ToList();
        var dict = Enumerable.Range(0, 9).ToDictionary(x => x, x => (ulong)fishes.Count(y => y == x));

        var days = 256;

        for (var i = 0; i < days; i++)
        {
            var zeroes = dict[0];

            foreach (var key in dict.Keys)
            {
                if (key == 8)
                {
                    continue;
                }

                dict[key] = dict[key + 1];
            }

            dict[6] += zeroes;
            dict[8] = zeroes;
        }

        return dict.Sum(x => (double)1 * x.Value);
    }

    private class Lanternfish(int daysLeft)
    {
        public int DaysLeft { get; private set; } = daysLeft;

        public Lanternfish DayPassed()
        {
            if (DaysLeft == 0)
            {
                DaysLeft = 6;
                return new Lanternfish(8);
            }

            DaysLeft--;
            return null;
        }
    }
}