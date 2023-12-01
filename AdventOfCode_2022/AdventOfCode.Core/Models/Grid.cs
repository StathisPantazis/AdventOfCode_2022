using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Models;

public interface IGrid
{
    int Height { get; }
    int Width { get; }
}

public class Grid<T> : IGrid
{
    private readonly T _emptyValue;

    public string Id { get; } = Guid.NewGuid().ToString();

    public T this[int x, int y]
    {
        get => Rows[y][x];
        set
        {
            Rows[y][x] = value;
            Columns[x][y] = value;
        }
    }

    public T this[Coordinates pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;

    }

    private Grid()
    {
        Rows = new();
    }

    public Grid(IEnumerable<string> source, string separator = " ", bool singleCharacters = false, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries, T emptyValue = default)
    {
        _emptyValue = emptyValue;
        if (typeof(T) == typeof(string))
        {
            InitializeString(source, separator, stringSplitOptions, singleCharacters);
        }
        else if (typeof(T) == typeof(char))
        {
            InitializeChar(source);
        }
        else if (typeof(T) == typeof(int))
        {
            InitializeInt(source, separator, stringSplitOptions, singleCharacters);
        }
        else if (typeof(T) == typeof(double))
        {
            InitializeDouble(source, separator, stringSplitOptions);
        }
        else if (typeof(T).BaseType == typeof(GridBase))
        {
            InitializeGridable(source, separator, stringSplitOptions, typeof(T));
        }

        RebuildColumns();
    }

    public Grid(IEnumerable<IEnumerable<T>> source)
    {
        Rows = source.Select(line => line.Select(elem => (T)(object)elem).ToList()).ToList();
        RebuildColumns();
    }

    public List<List<T>> Rows { get; private set; }

    public List<List<T>> Columns { get; private set; }

    public List<T> Row(int y) => Rows[y];

    public List<T> Column(int x) => Columns[x];

    public List<T> Row(Coordinates coord) => Rows[coord.Y];

    public List<T> Column(Coordinates coord) => Columns[coord.X];

    public Coordinates TopLeft => new(this, 0, 0);
    public Coordinates TopRight => new(this, Width - 1, 0);
    public Coordinates BottomLeft => new(this, 0, Height - 1);
    public Coordinates BottomRight => new(this, Width - 1, Height - 1);

    public int Height => Rows.Count;

    public int Width => Rows.Max(x => x.Count);

    public void RebuildColumns()
    {
        Columns = Enumerable.Range(0, Width).Select(x => Enumerable.Range(0, Height).Select(y => x < Height ? Rows[y][x] : _emptyValue).ToList()).ToList();
    }

    public void CropBottomOfGrid(int remainingRows, bool rebuildColumns = false)
    {
        Rows = Rows.Take(remainingRows).ToList();

        if (rebuildColumns)
        {
            RebuildColumns();
        }
    }

    public void CropTopOfGrid(int remainingRows, bool rebuildColumns = false)
    {
        Rows = Rows.Skip(Height - remainingRows).ToList();

        if (rebuildColumns)
        {
            RebuildColumns();
        }
    }

    public string ToString(string rowSeparator = "") => string.Join("\n", ListExtensions.ForNTimesFill(Height, (y) => string.Join(rowSeparator, ListExtensions.ForNTimesFill(Row(y).Count, (x) => this[x, y]!.ToString()!))));

    public override string ToString() => ToString();

    public void InsertRow(int index, T emptyValue, int repeat = 1, bool rebuildColumns = true)
    {
        ListExtensions.ForNTimesDo(repeat, () =>
        {
            Rows.Insert(index, ListExtensions.ForNTimesFill(Width, () => emptyValue));
            if (rebuildColumns)
            {
                RebuildColumns();
            }
        });
    }

    public void AddRow(T emptyValue, int repeat = 1, bool rebuildColumns = true)
    {
        ListExtensions.ForNTimesDo(repeat, () =>
        {
            Rows.Add(ListExtensions.ForNTimesFill(Width, () => emptyValue));
            if (rebuildColumns)
            {
                RebuildColumns();
            }
        });
    }

    public void RemoveRow(int index)
    {
        Rows.RemoveAt(index);
    }

    public void InsertColumn(int index, T emptyValue, int repeat = 1, bool rebuildColumns = true)
    {
        ListExtensions.ForNTimesDo(repeat, () =>
        {
            Rows.ForEach(row => row.Insert(index, emptyValue));
            if (rebuildColumns)
            {
                RebuildColumns();
            }
        });
    }

    public void AddColumn(T emptyValue, int repeat = 1, bool rebuildColumns = true)
    {
        ListExtensions.ForNTimesDo(repeat, () =>
        {
            Rows.ForEach(row => row.Add(emptyValue));
            if (rebuildColumns)
            {
                RebuildColumns();
            }
        });
    }

    public List<T> RowSliceLeft(Coordinates pos, bool reverse = true, bool includePosition = false) => RowSliceLeft(pos.X, pos.Y, reverse, includePosition);

    public List<T> RowSliceLeft(int x, int y, bool reverse = true, bool includePosition = false)
    {
        return reverse
            ? Rows[y].Take(x + (includePosition ? 1 : 0)).Reverse().ToList()
            : Rows[y].Take(x + (includePosition ? 1 : 0)).ToList();
    }

    public List<T> RowSliceRight(Coordinates pos, bool includePosition = false) => RowSliceRight(pos.X, pos.Y, includePosition);

    public List<T> RowSliceRight(int x, int y, bool includePosition = false)
    {
        return Rows[y].Skip(x + (includePosition ? 0 : 1)).ToList();
    }

    public List<T> ColumnSliceUp(Coordinates pos, bool reverse = true, bool includePosition = false) => ColumnSliceUp(pos.X, pos.Y, reverse, includePosition);

    public List<T> ColumnSliceUp(int x, int y, bool reverse = true, bool includePosition = false)
    {
        return reverse
            ? Columns[x].Take(y + (includePosition ? 1 : 0)).Reverse().ToList()
            : Columns[x].Take(y + (includePosition ? 1 : 0)).ToList();
    }

    public List<T> ColumnSliceDown(Coordinates pos, bool includePosition = false) => ColumnSliceDown(pos.X, pos.Y, includePosition);

    public List<T> ColumnSliceDown(int x, int y, bool includePosition = false)
    {
        return Columns[x].Skip(y + (includePosition ? 0 : 1)).ToList();
    }

    public static Grid<string> CreateEmptyGrid() => new();

    public static Grid<string> CreateGrid(int rows, int columns, char points = '.')
    {
        return new Grid<string>(Enumerable.Range(0, rows).Select(i => new string(points, columns)), singleCharacters: true);
    }

    private void InitializeString(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, bool singleCharacters)
    {
        if (singleCharacters)
        {
            Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)y.ToString()).ToList()).ToList();
        }
        else
        {
            Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)y).ToList()).ToList();
        }
    }

    private void InitializeChar(IEnumerable<string> source)
    {
        Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)y).ToList()).ToList();
    }

    private void InitializeInt(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, bool singleCharacters)
    {
        if (singleCharacters)
        {
            Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)int.Parse(y.ToString())).ToList()).ToList();
        }
        else
        {
            Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)int.Parse(y)).ToList()).ToList();
        }
    }

    private void InitializeDouble(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions)
    {
        Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)double.Parse(y)).ToList()).ToList();
    }

    private void InitializeGridable(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, Type type)
    {
        Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)(GridBase)Activator.CreateInstance(type, y)!).ToList()).ToList();
    }
}
