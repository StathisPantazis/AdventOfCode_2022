using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core.Models.Bases;

public interface IGrid
{
    int Height { get; }
    int Width { get; }
    string ToString(string itemSeparator = "", string rowSeparator = "\n");
}

public abstract class Grid<T> : IGrid
{
    private readonly bool _isCoordinatedGrid;
    protected readonly T _emptyValue;

    public string Id { get; } = Guid.NewGuid().ToString();

    public abstract T this[int x, int y] { get; set; }

    public T this[Coordinates pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    protected Grid()
    {
        _emptyValue = GetDefault();
        Rows = [];
    }

    protected Grid(int rows, int columns, T emptyValue) : this(GetEmptyGridLines(rows, columns, emptyValue), emptyValue)
    {
    }

    protected Grid(IEnumerable<IEnumerable<T>> source)
    {
        _isCoordinatedGrid = typeof(ICoordinated).IsAssignableFrom(typeof(T));
        _emptyValue = GetDefault();
        Rows = source.Select(line => line.Select(elem => (T)(object)elem).ToList()).ToList();

        Rebuild();
    }

    protected Grid(IEnumerable<IEnumerable<T>> source, T emptyValue) : this(source)
    {
        _emptyValue = emptyValue;
    }

    public List<List<T>> Rows { get; private set; }

    public List<List<T>> Columns { get; private set; }

    public abstract List<T> Row(int y);

    public List<T> Column(int x) => Columns[x];

    public abstract List<T> Row(Coordinates coord);

    public List<T> Column(Coordinates coord) => Columns[coord.X];

    public abstract Coordinates GetCoordinates(bool startFromNegative = false);

    public abstract Coordinates GetCoordinates(int startX, int startY);

    public abstract Coordinates GetCoordinates(GridCorner gridCorner);

    public int Height { get; protected set; }

    public int Width { get; protected set; }

    public string ToString(string itemSeparator = "", string rowSeparator = "\n") => string.Join(rowSeparator, Rows.Select(x => x.ListToString(itemSeparator)));

    public override string ToString() => ToString();

    public abstract void InsertRows(int y, int repeat = 1, bool rebuild = true);

    public abstract void InsertRow(int y, List<T> newRow, bool rebuild = true);

    public abstract void AddRows(int repeat, bool rebuild = true);

    public abstract void RemoveRows(int y, int repeat = 1, bool rebuild = true);

    public void InsertColumn(int index, T emptyValue, int repeat = 1, bool rebuild = true)
    {
        for (var i = 0; i < repeat; i++)
        {
            var total = Rows.Count;

            for (var r = 0; r < total; r++)
            {
                Rows[r].Insert(index, emptyValue);
            }
        }

        Width += repeat;

        if (rebuild)
        {
            Rebuild();
        }
    }

    public void AddColumn(T emptyValue, int repeat = 1, bool rebuild = true)
    {
        ListExtensions.ForNTimesDo(repeat, () => Rows.ForEach(row => row.Add(emptyValue)));
        Width += repeat;

        if (rebuild)
        {
            Rebuild();
        }
    }

    public void RemoveColumns(int x, int repeat = 1, bool rebuild = true)
    {
        for (var i = 0; i < repeat; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                Rows[j].RemoveAt(x);
            }
            Width--;
        }

        if (rebuild)
        {
            Rebuild();
        }
    }

    public List<T> RowSliceLeft(Coordinates pos, bool includePosition = false) => pos != null ? RowSliceLeft(pos.X, pos.Y, includePosition) : [];

    public abstract List<T> RowSliceLeft(int x, int y, bool includePosition = false);

    public List<T> RowSliceRight(Coordinates pos, bool includePosition = false) => pos != null ? RowSliceRight(pos.X, pos.Y, includePosition) : [];

    public abstract List<T> RowSliceRight(int x, int y, bool includePosition = false);

    public List<T> ColumnSliceUp(Coordinates pos, bool includePosition = false) => pos != null ? ColumnSliceUp(pos.X, pos.Y, includePosition) : [];

    public abstract List<T> ColumnSliceUp(int x, int y, bool includePosition = false);

    public List<T> ColumnSliceDown(Coordinates pos, bool includePosition = false) => pos != null ? ColumnSliceDown(pos.X, pos.Y, includePosition) : [];

    public abstract List<T> ColumnSliceDown(int x, int y, bool includePosition = false);

    public List<T> GetAllPoints(Func<T, bool> whereClause = null)
    {
        return whereClause is null
            ? Rows.SelectMany(x => x.Select(y => y)).ToList()
            : Rows.SelectMany(x => x.Select(y => y)).Where(x => whereClause(x)).ToList();
    }

    public T GetPoint(Func<T, bool> whereClause = null)
    {
        return whereClause is null
            ? Rows.SelectMany(x => x.Select(y => y)).FirstOrDefault()
            : Rows.SelectMany(x => x.Select(y => y)).FirstOrDefault(x => whereClause(x));
    }

    public int FirstIndexOfColumn(Func<T, bool> locatorCondition)
    {
        for (var i = 0; i < Columns.Count; i++)
        {
            if (i > 100)
            {
                var la = Column(i);
            }

            if (Column(i).Any(x => locatorCondition(x)))
            {
                return i;
            }
        }

        return -1;
    }

    public int LastIndexOfColumn(Func<T, bool> locatorCondition)
    {
        for (var i = Columns.Count - 1; i > -1; i--)
        {
            if (Column(i).Any(x => locatorCondition(x)))
            {
                return i + 1;
            }
        }

        return -1;
    }

    public int FirstIndexOfRow(Func<T, bool> locatorCondition)
    {
        for (var i = 0; i < Rows.Count; i++)
        {
            if (Row(i).Any(x => locatorCondition(x)))
            {
                return i;
            }
        }

        return -1;
    }

    public int LastIndexOfRow(Func<T, bool> locatorCondition)
    {
        for (var i = Rows.Count - 1; i > -1; i--)
        {
            if (Row(i).Any(x => locatorCondition(x)))
            {
                return i + 1;
            }
        }

        return -1;
    }

    public bool RowIndexIsOnBorder(int index) => index == 0 || index == Height - 1;

    public bool AnyRowIndexesAreOnBorder(params int[] indexes) => indexes.Any(index => index == 0 || index == Height - 1);

    public bool ColumnIndexIsOnBorder(int index) => index == 0 || index == Width - 1;

    public bool AnyIndexesAreOnBorder(GridSide gridSide, params int[] indexes) => gridSide is GridSide.Row ? AnyRowIndexesAreOnBorder(indexes) : AnyColumnIndexesAreOnBorder(indexes);

    public bool AnyColumnIndexesAreOnBorder(params int[] indexes) => indexes.Any(index => index == 0 || index == Width - 1);

    public void ReplaceRow(int y, List<T> newRow)
    {
        var existingRow = Row(y);

        for (var i = 0; i < existingRow.Count; i++)
        {
            existingRow[i] = newRow[i];
        }
    }

    public void ReplaceColumn(int x, List<T> newColumn)
    {
        var existingColumn = Column(x);

        for (var i = 0; i < existingColumn.Count; i++)
        {
            existingColumn[i] = newColumn[i];
        }
    }

    public List<T> RowOrColumn(GridSide gridSide, int index) => gridSide is GridSide.Row ? Row(index) : Column(index);

    //public void RotateRightBecomesLeft????()
    //{
    //    var newRows = new List<List<T>>();

    //    for (var x = Width - 1; x > -1; x--)
    //    {
    //        newRows.Add(Columns[x]);
    //    }

    //    Rows = newRows;

    //    RebuildColumns();
    //    RebuildCoordinates();
    //}

    public void Rotate()
    {
        var newRows = new List<List<T>>(Width);

        for (var x = 0; x < Width; x++)
        {
            newRows.Add(Columns[x].ReverseList());
        }

        Rows = newRows;
        Rebuild();
    }

    public void RebuildCoordinates()
    {
        if (_isCoordinatedGrid)
        {
            for (var y = 0; y < Rows.Count; y++)
            {
                for (var x = 0; x < Columns.Count; x++)
                {
                    if (this[x, y] is ICoordinated element)
                    {
                        element.Position = GetCoordinates(x, y);
                    }
                }
            }
        }
    }

    public void FloodFill(Coordinates position, Action<T> fill, Func<T, bool> shouldStopFill)
    {
        if (position is null || shouldStopFill(this[position]))
        {
            return;
        }

        var stack = new Stack<Coordinates>();
        stack.Push(position);

        while (stack.Count > 0)
        {
            var pos = stack.Pop();

            if (pos is null || shouldStopFill(this[pos]))
            {
                continue;
            }

            fill(this[pos]);

            stack.Push(pos.R);
            stack.Push(pos.L);
            stack.Push(pos.D);
            stack.Push(pos.U);
        }

        if (position is null || position.IsOutsideOfBorder || shouldStopFill(this[position]))
        {
            return;
        }
    }

    protected void Rebuild()
    {
        Height = Rows.Count;
        Width = Rows.Max(x => x.Count);
        Columns = Enumerable.Range(0, Width).Select(x => Enumerable.Range(0, Height).Select(y => x < Width ? Rows[y][x] : GenericBuilder.GetDefault(_emptyValue)).ToList()).ToList();
        RebuildCoordinates();
    }

    private static List<List<TType>> GetEmptyGridLines<TType>(int rows, int columns, TType emptyValue = default)
    {
        return Enumerable.Range(0, rows)
            .Select(i => ListBuilder.Repeat(columns, emptyValue))
            .ToList();
    }

    private static T GetDefault() => typeof(T) == typeof(string) ? (T)(object)"." : default;
}