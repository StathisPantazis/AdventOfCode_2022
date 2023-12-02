using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2021;

public class Day_5 : AoCBaseDay<int, int, string>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(FileDescription(this, resourceType));
        text = text.Replace(" -> ", ",");

        return Solution(text);
    }

    protected override int Part1(string text)
    {
        return SharedSolution(text, true);
    }

    protected override int Part2(string text)
    {
        return SharedSolution(text, false);
    }

    protected int SharedSolution(string text, bool isPart1)
    {
        var coordinates = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split(",") is string[] nums ? (int.Parse(nums[0]), int.Parse(nums[1]), int.Parse(nums[2]), int.Parse(nums[3])) : default)
            .ToArray();

        var maxX = coordinates.Select(x => NumericExtensions.MaxBetween(x.Item1, x.Item3)).Max();
        var maxY = coordinates.Select(x => NumericExtensions.MaxBetween(x.Item2, x.Item4)).Max();

        var grid = Grid<Vent>.CreateGrid<Vent>(maxY + 1, maxX + 1);

        foreach (var coordinate in coordinates)
        {
            var coordStart = new Coordinates(grid, coordinate.Item1, coordinate.Item2);
            var coordEnd = new Coordinates(grid, coordinate.Item3, coordinate.Item4);

            if (isPart1 && coordStart.X != coordEnd.X && coordStart.Y != coordEnd.Y)
            {
                continue;
            }

            grid[coordStart].Overlaps++;

            while (coordStart.MoveTowards(coordEnd, true))
            {
                grid[coordStart].Overlaps++;
            }
        }

        return grid.Rows.Sum(x => x.Count(y => y.Overlaps > 1));
    }

    private class Vent
    {
        public int Overlaps { get; set; }

        public override string ToString() => Overlaps == 0 ? "." : Overlaps.ToString();
    }
}