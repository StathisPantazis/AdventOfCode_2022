using AdventOfCode_2022.Extensions;

namespace AdventOfCode_2022.Utils;

internal interface IGrid {
    int Height { get; }
    int Width { get; }
}

internal class Grid<T> : IGrid {
    public T this[int row, int col] {
        get => Rows[row][col];
        set {
            Rows[row][col] = value;
            Columns[col][row] = value;
        }
    }

    public T this[Coordinates pos] {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;

    }

    public Grid(IEnumerable<string> source, string separator = " ", bool singleCharacters = false, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries, bool makeSquare = false, string emptyOverrideForSquare = "") {
        if (typeof(T) == typeof(string)) {
            InitializeString(source, separator, stringSplitOptions, singleCharacters);
        }
        else if (typeof(T) == typeof(char)) {
            InitializeChar(source);
        }
        else if (typeof(T) == typeof(int)) {
            InitializeInt(source, separator, stringSplitOptions, singleCharacters);
        }
        else if (typeof(T) == typeof(double)) {
            InitializeDouble(source, separator, stringSplitOptions);
        }
        else if (typeof(T).BaseType == typeof(Gridable)) {
            InitializeGridable(source, separator, stringSplitOptions, typeof(T));
        }

        if (makeSquare) {
            for (int i = 0; i < Height; i++) {
                if (Rows[i].Count < Width) {
                    for (int j = Rows[i].Count; j < Width; j++) {
                        Rows[i].Add(GetEmpty(emptyOverrideForSquare));
                    }
                }
            }
        }

        RebuildColumns(makeSquare, emptyOverrideForSquare);
    }

    public List<List<T>> Rows { get; private set; }

    public List<List<T>> Columns { get; private set; }

    public List<T> Row(int row) => Rows[row];

    public List<T> Column(int col) => Columns[col];

    public List<T> Row(Coordinates coord) => Rows[coord.X];

    public List<T> Column(Coordinates coord) => Columns[coord.Y];

    public Coordinates TopLeft => new(this, 0, 0);
    public Coordinates TopRight => new(this, 0, Width - 1);
    public Coordinates BottomLeft => new(this, Height - 1, 0);
    public Coordinates BottomRight => new(this, Height - 1, Width - 1);

    public int Height => Rows.Count;

    public int Width => Rows.Max(x => x.Count);

    public void RebuildColumns(bool makeSquare = false, string emptyOverrideForSquare = "") {
        Columns = makeSquare || Height == Width
            ? Enumerable.Range(0, Height).Select(row => Enumerable.Range(0, Width).Select(col => makeSquare && col >= Height ? GetEmpty(emptyOverrideForSquare) : Rows[col][row]).ToList()).ToList()
            : Enumerable.Range(0, Width).Select(col => Enumerable.Range(0, Height).Select(row => Rows[row][col]).ToList()).ToList();
    }

    public string ToString(string rowSeparator = "") => string.Join("\n", ListExtensions.ForNTimesFill(Height, (int row) => string.Join(rowSeparator, ListExtensions.ForNTimesFill<string>(Row(row).Count, (int col) => this[row, col].ToString()))));

    public override string ToString() => ToString();

    public void InsertRow(int index, T emptyValue, int repeat = 1, bool rebuildColumns = true) {
        ListExtensions.ForNTimesDo(repeat, () => {
            Rows.Insert(index, ListExtensions.ForNTimesFill(Width, () => emptyValue));
            if (rebuildColumns) {
                RebuildColumns();
            }
        });
    }

    public void AddRow(T emptyValue, int repeat = 1, bool rebuildColumns = true) {
        ListExtensions.ForNTimesDo(repeat, () => {
            Rows.Add(ListExtensions.ForNTimesFill(Width, () => emptyValue));
            if (rebuildColumns) {
                RebuildColumns();
            }
        });
    }

    public void InsertColumn(int index, T emptyValue, int repeat = 1, bool rebuildColumns = true) {
        ListExtensions.ForNTimesDo(repeat, () => {
            Rows.ForEach(row => row.Insert(index, emptyValue));
            if (rebuildColumns) {
                RebuildColumns();
            }
        });
    }

    public void AddColumn(T emptyValue, int repeat = 1, bool rebuildColumns = true) {
        ListExtensions.ForNTimesDo(repeat, () => {
            Rows.ForEach(row => row.Add(emptyValue));
            if (rebuildColumns) {
                RebuildColumns();
            }
        });
    }

    public List<T> RowSliceLeft(Coordinates pos, bool reverse = true, bool includePosition = false) => RowSliceLeft(pos.X, pos.Y, reverse, includePosition);

    public List<T> RowSliceLeft(int row, int column, bool reverse = true, bool includePosition = false) {
        return reverse
            ? Rows[row].Take(column + (includePosition ? 1 : 0)).Reverse().ToList()
            : Rows[row].Take(column + (includePosition ? 1 : 0)).ToList();
    }

    public List<T> RowSliceRight(Coordinates pos, bool includePosition = false) => RowSliceRight(pos.X, pos.Y, includePosition);

    public List<T> RowSliceRight(int row, int column, bool includePosition = false) {
        return Rows[row].Skip(column + (includePosition ? 0 : 1)).ToList();
    }

    public List<T> ColumnSliceUp(Coordinates pos, bool reverse = true, bool includePosition = false) => ColumnSliceUp(pos.Y, pos.X, reverse, includePosition);

    public List<T> ColumnSliceUp(int column, int row, bool reverse = true, bool includePosition = false) {
        return reverse
            ? Columns[column].Take(row + (includePosition ? 1 : 0)).Reverse().ToList()
            : Columns[column].Take(row + (includePosition ? 1 : 0)).ToList();
    }

    public List<T> ColumnSliceDown(Coordinates pos, bool includePosition = false) => ColumnSliceDown(pos.Y, pos.X, includePosition);

    public List<T> ColumnSliceDown(int column, int row, bool includePosition = false) {
        return Columns[column].Skip(row + (includePosition ? 0 : 1)).ToList();
    }

    public static Grid<string> CreateGrid(int rows, int columns, char points = '.') {
        return new Grid<string>(Enumerable.Range(0, rows).Select(i => new string(points, columns)), singleCharacters: true);
    }

    private void InitializeString(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, bool singleCharacters) {
        if (singleCharacters) {
            Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)(y.ToString())).ToList()).ToList();
        }
        else {
            Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)y).ToList()).ToList();
        }
    }

    private void InitializeChar(IEnumerable<string> source) {
        Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)y).ToList()).ToList();
    }

    private void InitializeInt(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, bool singleCharacters) {
        if (singleCharacters) {
            Rows = source.Select(x => x.ToCharArray().Select(y => (T)(object)int.Parse(y.ToString())).ToList()).ToList();
        }
        else {
            Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)int.Parse(y)).ToList()).ToList();
        }
    }

    private void InitializeDouble(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions) {
        Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)double.Parse(y)).ToList()).ToList();
    }

    private void InitializeGridable(IEnumerable<string> source, string separator, StringSplitOptions stringSplitOptions, Type type) {
        Rows = source.Select(x => x.Split(separator, stringSplitOptions).Select(y => (T)(object)(Gridable)Activator.CreateInstance(type, y)).ToList()).ToList();
    }

    private static T GetEmpty(string emptyOverride) {
        if (typeof(T) == typeof(string)) {
            return (T)(object)(!string.IsNullOrEmpty(emptyOverride) ? emptyOverride : "_");
        }
        else if (typeof(T) == typeof(char)) {
            return (T)(object)(!string.IsNullOrEmpty(emptyOverride) ? emptyOverride : '_');
        }
        else if (typeof(T) == typeof(int)) {
            return (T)(object)(!string.IsNullOrEmpty(emptyOverride) ? emptyOverride : 0);
        }
        else if (typeof(T) == typeof(double)) {
            return (T)(object)(!string.IsNullOrEmpty(emptyOverride) ? emptyOverride : 0);
        }
        return default;
    }
}
