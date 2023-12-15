using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2021.Day_09;

namespace AdventOfCode.Version2021;

public class Day_09 : AoCBaseDay<int, int, IndexedGrid<Point>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Select(y => new Point(int.Parse(y.ToString()))))
            .ToList();

        var grid = new IndexedGrid<Point>(lines);

        Part1(grid);

        return default;
    }

    protected override int Part1(IndexedGrid<Point> grid)
    {
        var coord = grid.GetCoordinates(true);
        var sum = 0;

        while (coord.TraverseGrid())
        {
            var point = grid[coord];

            var neighbours = coord.GetAllNeighbours();
            var lowNeighbours = neighbours.Where(x => point.Value < grid[x].Value).ToList();

            if (neighbours.Count == lowNeighbours.Count)
            {
                point.BelongsToBasin = true;
                point.IsLowPoint = true;
                sum += point.Value + 1;
            }
        }

        return sum;
    }

    protected override int Part2(IndexedGrid<Point> grid)
    {
        return 0;
    }

    public class Point(int value)
    {
        public int Value { get; init; } = value;
        public CartesianCoordinates Coords { get; set; }
        public bool IsLowPoint { get; set; }
        public bool BelongsToBasin { get; set; }
    }
}