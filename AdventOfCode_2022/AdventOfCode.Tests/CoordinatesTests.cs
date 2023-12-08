using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Tests.Shared;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class CoordinatesTests
{
    [Theory]
    [InlineData(GridType.Indexed, AsymmetrySide.None)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Indexed, AsymmetrySide.BigSquare)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Cartesian, AsymmetrySide.BigSquare)]
    public void Should_traverse_indexed_grid(GridType gridType, AsymmetrySide asymmetrySide)
    {
        var grid = GridBuilder.GetGrid(gridType, asymmetrySide);
        var coord = grid.GetCoordinates(true);
        var steps = 0;

        while (coord.TraverseGrid())
        {
            steps++;
            grid[coord] = new Point() { Text = "X" };
        }

        var allPoints = grid.GetAllPoints();
        allPoints.Select(x => x.Text).Distinct().Count().Should().Be(1);
        allPoints.Select(x => x.Text).Distinct().First().Should().Be("X");
        steps.Should().Be(allPoints.Count);
    }

    [Theory]
    [InlineData(GridType.Indexed, AsymmetrySide.None)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Indexed, AsymmetrySide.BigSquare)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Cartesian, AsymmetrySide.BigSquare)]
    public void Should_reverse_traverse_indexed_grid(GridType gridType, AsymmetrySide asymmetrySide)
    {
        var grid = GridBuilder.GetGrid(gridType, asymmetrySide);
        var coord = grid.GetCoordinates(1, 1);
        coord.GoToEnd(true);
        var steps = 0;

        while (coord.ReverseTraverseGrid())
        {
            steps++;
            grid[coord] = new Point() { Text = "X" };
        }

        var allPoints = grid.GetAllPoints();
        allPoints.Select(x => x.Text).Distinct().Count().Should().Be(1);
        allPoints.Select(x => x.Text).Distinct().First().Should().Be("X");
        steps.Should().Be(allPoints.Count);
    }

    [Theory]
    [InlineData(GridType.Indexed, AsymmetrySide.None, false)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows, false)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns, false)]
    [InlineData(GridType.Indexed, AsymmetrySide.BigSquare, false)]
    [InlineData(GridType.Indexed, AsymmetrySide.None, true)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows, true)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns, true)]
    [InlineData(GridType.Indexed, AsymmetrySide.BigSquare, true)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None, false)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows, false)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns, false)]
    [InlineData(GridType.Cartesian, AsymmetrySide.BigSquare, false)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None, true)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows, true)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns, true)]
    [InlineData(GridType.Cartesian, AsymmetrySide.BigSquare, true)]
    public void Should_move_correctly_towards(GridType gridType, AsymmetrySide asymmetrySide, bool allowOverlap)
    {
        var grid = GridBuilder.GetGrid(gridType, asymmetrySide);
        var start = grid.GetCoordinates(0, 0);
        var end = start.Copy();
        end.GoToEnd();

        while (start.MoveTowards(end, allowOverlap))
        {
            grid[start] = new Point(".");
        }

        if (allowOverlap)
        {
            grid[end].Text.Should().Be(".");
        }

        var allPoints = grid.GetAllPoints();
        var isSquare = grid.Rows.Count == grid.Columns.Count;
        var dots = NumericExtensions.MaxBetween(grid.Rows.Count, grid.Columns.Count) - Math.Abs(grid.Rows.Count - grid.Columns.Count) + (isSquare ? -1 : 0) + (allowOverlap ? 1 : 0) - 1;
        allPoints.Count(x => x.Text == ".").Should().Be(dots);
    }

    [Theory]
    [InlineData(GridType.Indexed, AsymmetrySide.None)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Indexed, AsymmetrySide.BigSquare)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Cartesian, AsymmetrySide.BigSquare)]
    public void Should_move_correctly_opposite(GridType gridType, AsymmetrySide asymmetrySide)
    {
        var grid = GridBuilder.GetGrid(gridType, asymmetrySide);
        var start = grid.GetCoordinates(1, 1);
        var awayFrom = grid.GetCoordinates(0, 0);

        while (start.MoveOpposite(awayFrom))
        {
            grid[start] = new Point(".");
        }

        var allPoints = grid.GetAllPoints();
        var isSquare = grid.Rows.Count == grid.Columns.Count;
        var dots = NumericExtensions.MaxBetween(grid.Rows.Count, grid.Columns.Count) - Math.Abs(grid.Rows.Count - grid.Columns.Count) + (isSquare ? -1 : 0) - 1;
        allPoints.Count(x => x.Text == ".").Should().Be(dots);
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_up_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var up = coord.U;
        grid[up] = new Point(".");

        var row = grid.Rows[2];
        row[3].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_down_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var down = coord.D;
        grid[down] = new Point(".");

        var row = grid.Rows[4];
        row[3].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_left_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var left = coord.L;
        grid[left] = new Point(".");

        var column = grid.Columns[2];
        column[3].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_right_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var right = coord.R;
        grid[right] = new Point(".");

        var column = grid.Columns[4];
        column[3].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_up_right_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var up = coord.UR;
        grid[up] = new Point(".");

        var row = grid.Rows[2];
        row[4].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_down_right_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var down = coord.DR;
        grid[down] = new Point(".");

        var row = grid.Rows[4];
        row[4].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_up_left_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var left = coord.UL;
        grid[left] = new Point(".");

        var row = grid.Rows[2];
        row[2].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correctly_get_down_left_coordinates(GridType gridType)
    {
        var (grid, coord) = CenterOfSquare(gridType);
        var right = coord.DL;
        grid[right] = new Point(".");

        var row = grid.Rows[4];
        row[2].Text.Should().Be(".");
    }

    private static (Grid<Point> grid, Coordinates coord) CenterOfSquare(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.BigSquare);
        var coord = grid.GetCoordinates(3, 3);

        return (grid, coord);
    }
}
