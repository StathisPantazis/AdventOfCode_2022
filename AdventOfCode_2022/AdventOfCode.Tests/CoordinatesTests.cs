using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
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
        var start = grid.GetCoordinates(GridCorner.Start);
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
    [InlineData(GridType.Indexed, GridCorner.TopLeft, true)]
    [InlineData(GridType.Indexed, GridCorner.TopLeft, false)]
    [InlineData(GridType.Cartesian, GridCorner.TopLeft, true)]
    [InlineData(GridType.Cartesian, GridCorner.TopLeft, false)]
    [InlineData(GridType.Indexed, GridCorner.TopRight, true)]
    [InlineData(GridType.Indexed, GridCorner.TopRight, false)]
    [InlineData(GridType.Cartesian, GridCorner.TopRight, true)]
    [InlineData(GridType.Cartesian, GridCorner.TopRight, false)]
    [InlineData(GridType.Indexed, GridCorner.BottomLeft, true)]
    [InlineData(GridType.Indexed, GridCorner.BottomLeft, false)]
    [InlineData(GridType.Cartesian, GridCorner.BottomLeft, true)]
    [InlineData(GridType.Cartesian, GridCorner.BottomLeft, false)]
    [InlineData(GridType.Indexed, GridCorner.BottomRight, true)]
    [InlineData(GridType.Indexed, GridCorner.BottomRight, false)]
    [InlineData(GridType.Cartesian, GridCorner.BottomRight, true)]
    [InlineData(GridType.Cartesian, GridCorner.BottomRight, false)]
    public void Should_move_correctly_opposite_for_diagonal(GridType gridType, GridCorner gridCorner, bool touching)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.HugeSquare);
        var awayFrom = grid.GetCoordinates(gridCorner);

        var direction = gridCorner switch
        {
            GridCorner.TopLeft => Direction.DR,
            GridCorner.TopRight => Direction.DL,
            GridCorner.BottomLeft => Direction.UR,
            GridCorner.BottomRight => Direction.UL,
        };

        var start = awayFrom.Copy();
        start.Move(direction);

        if (!touching)
        {
            start.Move(direction);
        }

        while (start.MoveOpposite(awayFrom))
        {
            grid[start] = new Point(".");
        }

        var allPoints = grid.GetAllPoints();
        var dots = grid.Rows.Count - 2 - (touching ? 0 : 1);
        allPoints.Count(x => x.Text == ".").Should().Be(dots);

        grid[7, 7].Text.Should().Be(".");
    }

    [Theory]
    [InlineData(GridType.Indexed, Direction.L, true)]
    [InlineData(GridType.Indexed, Direction.L, false)]
    [InlineData(GridType.Cartesian, Direction.L, true)]
    [InlineData(GridType.Cartesian, Direction.L, false)]
    [InlineData(GridType.Indexed, Direction.R, true)]
    [InlineData(GridType.Indexed, Direction.R, false)]
    [InlineData(GridType.Cartesian, Direction.R, true)]
    [InlineData(GridType.Cartesian, Direction.R, false)]
    [InlineData(GridType.Indexed, Direction.U, true)]
    [InlineData(GridType.Indexed, Direction.U, false)]
    [InlineData(GridType.Cartesian, Direction.U, true)]
    [InlineData(GridType.Cartesian, Direction.U, false)]
    [InlineData(GridType.Indexed, Direction.D, true)]
    [InlineData(GridType.Indexed, Direction.D, false)]
    [InlineData(GridType.Cartesian, Direction.D, true)]
    [InlineData(GridType.Cartesian, Direction.D, false)]
    public void Should_move_correctly_opposite_for_straight(GridType gridType, Direction direction, bool touching)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.HugeSquare);

        var gridCorner = direction switch
        {
            Direction.L => GridCorner.TopRight,
            Direction.R => GridCorner.TopLeft,
            Direction.U => GridCorner.BottomLeft,
            Direction.D => GridCorner.TopLeft,
        };

        var awayFrom = grid.GetCoordinates(gridCorner);

        var start = awayFrom.Copy();
        start.Move(direction);

        if (!touching)
        {
            start.Move(direction);
        }

        while (start.MoveOpposite(awayFrom))
        {
            grid[start] = new Point(".");
        }

        var allPoints = grid.GetAllPoints();
        var dots = grid.Rows.Count - 2 - (touching ? 0 : 1);
        allPoints.Count(x => x.Text == ".").Should().Be(dots);

        var dotsToCheck = direction switch
        {
            Direction.L or Direction.R => grid.Rows[0],
            Direction.U or Direction.D => grid.Columns[0],
        };

        dotsToCheck.Count(x => x.Text == ".").Should().Be(dots);
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
