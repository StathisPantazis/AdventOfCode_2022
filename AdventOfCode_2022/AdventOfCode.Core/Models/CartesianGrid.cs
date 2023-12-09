using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core.Models;

public class CartesianGrid<T> : Grid<T>
{
    public CartesianGrid() : base() { }

    public CartesianGrid(int rows, int columns, T emptyValue = default) : base(rows, columns, emptyValue) { }

    public CartesianGrid(IEnumerable<IEnumerable<T>> source) : base(source) { }

    public CartesianGrid(IEnumerable<IEnumerable<T>> source, T emptyValue) : base(source, emptyValue) { }

    public override T this[int x, int y]
    {
        get => Rows[CartesianY(y)][x];
        set
        {
            Rows[CartesianY(y)][x] = value;
            Columns[x][CartesianY(y)] = value;
        }
    }

    public override List<T> Row(int y) => Rows[CartesianY(y)];

    public override List<T> Row(Coordinates coord) => Rows[CartesianY(coord.Y)];

    public override Coordinates GetCoordinates(bool startFromNegative = false) => new CartesianCoordinates(this, startFromNegative);

    public override Coordinates GetCoordinates(int startX, int startY) => new CartesianCoordinates(this, startX, startY);

    public override Coordinates GetCoordinates(GridCorner gridCorner)
    {
        return gridCorner switch
        {
            GridCorner.Start or GridCorner.BottomLeft => new CartesianCoordinates(this, 0, 0),
            GridCorner.End or GridCorner.TopRight => new CartesianCoordinates(this, Columns.Count - 1, Rows.Count - 1),
            GridCorner.BottomRight => new CartesianCoordinates(this, Columns.Count - 1, 0),
            GridCorner.TopLeft => new CartesianCoordinates(this, 0, Rows.Count - 1),
            _ => throw new NotImplementedException()
        };
    }

    public override void InsertRows(int y, int repeat = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var newRows = ListBuilder.Repeat(Width, _emptyValue);
            Rows.Insert(CartesianY(y) + 1, newRows);
        }

        RebuildColumns();
    }

    public override void AddRows(int repeat)
    {
        for (var i = 0; i < repeat; i++)
        {
            var newRows = ListBuilder.Repeat(Width, _emptyValue);
            Rows.Insert(0, newRows);
        }

        RebuildColumns();
    }

    public override void RemoveRows(int y, int repeat = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            Rows.RemoveAt(CartesianY(y));
        }
    }

    public override List<T> RowSliceLeft(int x, int y, bool includePosition = false)
        => Rows[CartesianY(y)].Take(x + (includePosition ? 1 : 0)).ToList();

    public override List<T> RowSliceRight(int x, int y, bool includePosition = false)
        => Rows[CartesianY(y)].Skip(x + (includePosition ? 0 : 1)).ToList();

    public override List<T> ColumnSliceUp(int x, int y, bool includePosition = false)
        => Columns[x].Take(CartesianY(y) + (includePosition ? 1 : 0)).ToList();

    public override List<T> ColumnSliceDown(int x, int y, bool includePosition = false)
        => Columns[x].Skip(CartesianY(y) + (includePosition ? 0 : 1)).ToList();

    private int CartesianY(int y) => Height - y - 1;
}
