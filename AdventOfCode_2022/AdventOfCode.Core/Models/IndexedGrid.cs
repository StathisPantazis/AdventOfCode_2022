using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core.Models;

public class IndexedGrid<T> : Grid<T>
{
    public IndexedGrid() : base() { }

    public IndexedGrid(int rows, int columns, T emptyValue = default) : base(rows, columns, emptyValue) { }

    public IndexedGrid(IEnumerable<IEnumerable<T>> source) : base(source) { }

    public IndexedGrid(IEnumerable<IEnumerable<T>> source, T emptyValue) : base(source, emptyValue) { }

    public override T this[int x, int y]
    {
        get => Rows[y][x];
        set
        {
            Rows[y][x] = value;
            Columns[x][y] = value;
        }
    }

    public override List<T> Row(int y) => Rows[y];

    public override List<T> Row(Coordinates coord) => Rows[coord.Y];

    public override Coordinates GetCoordinates(bool startFromNegative = false) => new IndexedCoordinates(this, startFromNegative);

    public override Coordinates GetCoordinates(int startX, int startY) => new IndexedCoordinates(this, startX, startY);

    public override void InsertRows(int y, int repeat = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            var newRows = ListBuilder.Repeat(Width, _emptyValue);
            Rows.Insert(y, newRows);
        }

        RebuildColumns();
    }

    public override void AddRows(int repeat)
    {
        for (var i = 0; i < repeat; i++)
        {
            var newRows = ListBuilder.Repeat(Width, _emptyValue);
            Rows.Add(newRows);
        }

        RebuildColumns();
    }

    public override void RemoveRows(int y, int repeat = 1)
    {
        for (var i = 0; i < repeat; i++)
        {
            Rows.RemoveAt(y);
        }
    }

    public override List<T> RowSliceLeft(int x, int y, bool includePosition = false)
        => Row(y).Take(x + (includePosition ? 1 : 0)).ToList();

    public override List<T> RowSliceRight(int x, int y, bool includePosition = false)
        => Row(y).Skip(x + (includePosition ? 0 : 1)).ToList();

    public override List<T> ColumnSliceUp(int x, int y, bool includePosition = false)
        => Columns[x].Take(y + (includePosition ? 1 : 0)).ToList();

    public override List<T> ColumnSliceDown(int x, int y, bool includePosition = false)
        => Columns[x].Skip(y + (includePosition ? 0 : 1)).ToList();
}
