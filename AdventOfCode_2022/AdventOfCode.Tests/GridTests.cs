using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Tests.Shared;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class GridTests
{
    private static readonly List<string> _list1234 = new() { "1", "2", "3", "4" };
    private static readonly List<string> _list5678 = new() { "5", "6", "7", "8" };

    [Theory]
    [InlineData(GridType.Indexed, AsymmetrySide.None)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Indexed, AsymmetrySide.MoreColumns)]
    [InlineData(GridType.Cartesian, AsymmetrySide.None)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns)]
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
    [InlineData(GridType.Cartesian, AsymmetrySide.None)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreRows)]
    [InlineData(GridType.Cartesian, AsymmetrySide.MoreColumns)]
    public void Should_reverse_traverse_indexed_grid(GridType gridType, AsymmetrySide asymmetrySide)
    {
        var grid = GridBuilder.GetGrid(gridType, asymmetrySide);
        var coord = grid.GetCoordinates();
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
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_correcetly_get_grid_corners(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.None);

        var coord = grid.GetCoordinates(GridCorner.TopLeft);
        grid[coord] = new Point("A");

        coord = grid.GetCoordinates(GridCorner.TopRight);
        grid[coord] = new Point("B");

        coord = grid.GetCoordinates(GridCorner.BottomLeft);
        grid[coord] = new Point("C");

        coord = grid.GetCoordinates(GridCorner.BottomRight);
        grid[coord] = new Point("D");

        grid.Rows.First().First().Text.Should().Be("A");
        grid.Rows.First().Last().Text.Should().Be("B");
        grid.Rows.Last().First().Text.Should().Be("C");
        grid.Rows.Last().Last().Text.Should().Be("D");

        coord = grid.GetCoordinates(GridCorner.Start);
        grid[coord] = new Point("S");

        coord = grid.GetCoordinates(GridCorner.End);
        grid[coord] = new Point("E");

        if (gridType is GridType.Cartesian)
        {
            grid.Rows.Last().First().Text.Should().Be("S");
            grid.Rows.First().Last().Text.Should().Be("E");
        }
        else if (gridType is GridType.Indexed)
        {
            grid.Rows.First().First().Text.Should().Be("S");
            grid.Rows.Last().Last().Text.Should().Be("E");
        }
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_index_operator_work(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows);

        var value = gridType is GridType.Cartesian ? "2" : "6";
        grid[1, 1].Text.Should().Be(value);
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_index_operator_by_coordinates_work(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows);
        var coord = grid.GetCoordinates(1, 1);

        var value = gridType is GridType.Cartesian ? "2" : "6";
        grid[coord].Text.Should().Be(value);
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_row_indexing_work(GridType gridType)
    {
        var index = 1;
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows);

        var rowEqual = gridType is GridType.Cartesian ? _list1234 : _list5678;
        grid.Row(index).Select(x => x.Text).SequenceEqual(rowEqual).Should().BeTrue();

        grid.Rows[index].Select(x => x.Text).SequenceEqual(_list5678).Should().BeTrue();
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_row_indexing_by_coordinates_work(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows);
        var coord = grid.GetCoordinates(1, 1);

        var actualRowEqual = gridType is GridType.Cartesian ? _list1234 : _list5678;
        grid.Row(coord).Select(x => x.Text).SequenceEqual(actualRowEqual).Should().BeTrue();
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_insert_row_correctly(GridType gridType)
    {
        var y = 2;
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows);

        grid.InsertRows(y, repeat: 2);

        grid.Row(y).Select(x => x.Text).SequenceEqual(Dots(4)).Should().BeTrue();
        grid.Row(y + 1).Select(x => x.Text).SequenceEqual(Dots(4)).Should().BeTrue();

        var allItems = grid.GetAllPoints();
        allItems.Count(x => x.Text == ".").Should().Be(8);
        allItems.DistinctBy(x => x.Id).Count().Should().Be(36);
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_add_row_correctly(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.None);
        grid.AddRows(2);

        grid.Row(grid.Height - 1).Select(x => x.Text).SequenceEqual(Dots(3)).Should().BeTrue();
        grid.Row(grid.Height - 2).Select(x => x.Text).SequenceEqual(Dots(3)).Should().BeTrue();

        var allItems = grid.GetAllPoints();
        allItems.Count(x => x.Text == ".").Should().Be(6);
        allItems.DistinctBy(x => x.Id).Count().Should().Be(15);
    }

    [Theory]
    [InlineData(GridType.Indexed)]
    [InlineData(GridType.Cartesian)]
    public void Should_remove_row_correctly(GridType gridType)
    {
        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows, rowsSameNumber: true);

        grid.RemoveRows(y: 2, repeat: 2);

        var allItems = grid.GetAllPoints();
        allItems.Count.Should().Be(20);

        if (gridType is GridType.Indexed)
        {
            allItems.Count(x => x.Text == "3").Should().Be(0);
            allItems.Count(x => x.Text == "4").Should().Be(0);
        }
        else
        {
            allItems.Count(x => x.Text == "5").Should().Be(0);
            allItems.Count(x => x.Text == "4").Should().Be(0);
        }
    }

    [Theory]
    [InlineData(GridType.Indexed, false)]
    [InlineData(GridType.Indexed, true)]
    [InlineData(GridType.Cartesian, false)]
    [InlineData(GridType.Cartesian, true)]
    public void Should_correctly_get_row_slice_left(GridType gridType, bool includePosition)
    {
        var x = 3;
        var y = 1;

        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyColumns, columnsSameNumber: true);

        var slice = new List<string> { "1", "2", "3" };

        if (includePosition)
        {
            slice.Add("4");
        }

        var sliceLeft = grid.RowSliceLeft(x, y, includePosition);
        sliceLeft.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();

        var coord = grid.GetCoordinates(x, y);
        sliceLeft = grid.RowSliceLeft(coord, includePosition);

        sliceLeft.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();
    }

    [Theory]
    [InlineData(GridType.Indexed, false)]
    [InlineData(GridType.Indexed, true)]
    [InlineData(GridType.Cartesian, false)]
    [InlineData(GridType.Cartesian, true)]
    public void Should_correctly_get_row_slice_right(GridType gridType, bool includePosition)
    {
        var x = 3;
        var y = 1;

        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyColumns, columnsSameNumber: true);

        var slice = new List<string> { "5", "6", "7" };

        if (includePosition)
        {
            slice.Insert(0, "4");
        }

        var sliceRight = grid.RowSliceRight(x, y, includePosition);
        sliceRight.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();

        var coord = grid.GetCoordinates(x, y);
        sliceRight = grid.RowSliceRight(coord, includePosition);

        sliceRight.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();
    }

    [Theory]
    [InlineData(GridType.Indexed, false)]
    [InlineData(GridType.Indexed, true)]
    [InlineData(GridType.Cartesian, false)]
    [InlineData(GridType.Cartesian, true)]
    public void Should_correctly_get_column_slice_up(GridType gridType, bool includePosition)
    {
        var x = 1;
        var y = 3;

        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows, rowsSameNumber: true);

        var slice = new List<string> { "1", "2", "3" };

        if (includePosition)
        {
            slice.Add("4");
        }

        var sliceUp = grid.ColumnSliceUp(x, y, includePosition);
        sliceUp.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();

        var coord = grid.GetCoordinates(x, y);
        sliceUp = grid.ColumnSliceUp(coord, includePosition);

        sliceUp.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();
    }

    [Theory]
    [InlineData(GridType.Indexed, false)]
    [InlineData(GridType.Indexed, true)]
    [InlineData(GridType.Cartesian, false)]
    [InlineData(GridType.Cartesian, true)]
    public void Should_correctly_get_column_slice_down(GridType gridType, bool includePosition)
    {
        var x = 1;
        var y = 3;

        var grid = GridBuilder.GetGrid(gridType, AsymmetrySide.ManyRows, rowsSameNumber: true);

        var slice = new List<string> { "5", "6", "7" };

        if (includePosition)
        {
            slice.Insert(0, "4");
        }

        var sliceDown = grid.ColumnSliceDown(x, y, includePosition);
        sliceDown.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();

        var coord = grid.GetCoordinates(x, y);
        sliceDown = grid.ColumnSliceDown(coord, includePosition);

        sliceDown.Select(x => x.Text).SequenceEqual(slice).Should().BeTrue();
    }

    private static List<string> Dots(int count) => new string('.', count).Select(x => x.ToString()).ToList();
}
