using AdventOfCode_2022.Extensions;

namespace AdventOfCode_2022.Utils;

internal interface IGrid {
    int MaxRows { get; }
    int MaxColumns { get; }
}

internal class Grid<T> : IGrid {
    public T this[int row, int col] {
        get => Rows[row][col];
        set => Rows[row][col] = value;
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
            for (int i = 0; i < MaxRows; i++) {
                if (Rows[i].Count < MaxColumns) {
                    for (int j = Rows[i].Count; j < MaxColumns; j++) {
                        Rows[i].Add(GetEmpty(emptyOverrideForSquare));
                    }
                }
            }
        }

        Columns = makeSquare || MaxRows == MaxColumns
            ? Enumerable.Range(0, MaxRows).Select(row => Enumerable.Range(0, MaxColumns).Select(col => makeSquare && col >= MaxRows ? GetEmpty(emptyOverrideForSquare) : Rows[col][row]).ToList()).ToList()
            : Enumerable.Range(0, MaxRows).Select(row => Enumerable.Range(0, MaxColumns).Select(col => Rows[row][col]).ToList()).ToList();
    }

    public List<List<T>> Rows { get; private set; }

    public List<List<T>> Columns { get; init; }

    public List<T> Row(int row) => Rows[row];

    public List<T> Column(int col) => Columns[col];

    public int Count => MaxRows;

    public int MaxRows => Rows.Count;

    public int MaxColumns => Rows.Max(x => x.Count);

    public string ToString(string rowSeparator = "") => string.Join("\n", ListExtensions.ForNTimesFill(MaxRows, (int row) => string.Join(rowSeparator, ListExtensions.ForNTimesFill<string>(Row(row).Count, (int col) => this[row, col].ToString()))));

    public override string ToString() => ToString();

    public (List<T> row, List<T>) GetCurrentRowColumn(Coordinates pos) {
        return (Rows[pos.X], Columns[pos.Y]);
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
