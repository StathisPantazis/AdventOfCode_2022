using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_11 : AoCBaseDay<long, long, IndexedGrid<string>>
{
    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var grid = new IndexedGrid<string>(Helpers.File_CleanReadLines(FileDescription(this, resourceType))
          .Select(x => x.Select(x => x.ToString()).ToList().ToList()));

        return Solution(grid);
    }

    protected override long Part1(IndexedGrid<string> grid)
    {
        return SharedSolution(grid, 1);
    }

    protected override long Part2(IndexedGrid<string> grid)
    {
        return SharedSolution(grid, 1000000);
    }

    private static long SharedSolution(IndexedGrid<string> grid, int multiplier)
    {
        // Expand
        var rowIndexesToExpand = grid.Rows.Select((x, i) => x.All(y => y == ".") ? i : -1).Where(x => x != -1).ToList();
        var columnIndexesToExpand = grid.Columns.Select((x, i) => x.All(y => y == ".") ? i : -1).Where(x => x != -1).ToList();

        // Find galaxies
        var galaxies = new List<Coordinates>();
        var traversal = grid.GetCoordinates(true);

        while (traversal.TraverseGrid())
        {
            if (grid[traversal] == "#")
            {
                galaxies.Add(traversal.Copy());
            }
        }

        // Increase galaxies' x and y
        foreach (var galaxy in galaxies)
        {
            var extraColumnsLeft = columnIndexesToExpand.Count(x => x < galaxy.X) * (multiplier - 1).AtLeast(1);
            galaxy.X += extraColumnsLeft;

            var extraRowsUp = rowIndexesToExpand.Count(x => x < galaxy.Y) * (multiplier - 1).AtLeast(1);
            galaxy.Y += extraRowsUp;
        }

        // Calculate steps
        long totalSteps = 0;

        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                totalSteps += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
            }
        }

        return totalSteps;
    }
}