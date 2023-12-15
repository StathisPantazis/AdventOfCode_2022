using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core.Tests;

public class FastArrArrGrid<T> : IGrid
{
    private readonly T _defaultValue;

    public T this[int x, int y]
    {
        get => Rows[y][x];
        set => Rows[y][x] = value;
    }

    public T this[Coordinates pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    public FastArrArrGrid(IEnumerable<IEnumerable<T>> source, T defaultValue)
    {
        _defaultValue = defaultValue;

        Rows = source.Select(line => line.Select(elem => (T)(object)elem).ToArray()).ToArray();
        Rebuild();
    }

    public T[][] Rows { get; private set; }
    public T[][] Columns { get; private set; }

    public int Height { get; private set; }

    public int Width { get; private set; }

    // METHODS
    public T[] Row(int y) => Rows[y];

    public T[] Column(int x) => Columns[x];

    public void AddRow(T[] newRow)
    {
        var newArray = new T[Height + 1][];

        for (var i = 0; i < Height; i++)
        {
            newArray[i] = Rows[i];
        }

        newArray[Height] = newRow;
        Rows = newArray;
        Rebuild();
    }

    public string ToString(string itemSeparator = "", string rowSeparator = "\n") => string.Join(rowSeparator, Rows.Select(x => x.ListToString(itemSeparator)));

    private void Rebuild()
    {
        Height = Rows.Length;
        Width = Rows.Max(x => x.Length);

        Columns = ListBuilder.ForI(Width)
            .Select(x => ListBuilder.ForI(Height)
                .Select(y => x < Width ? Rows[y][x] : _defaultValue)
                .ToArray())
            .ToArray();
    }
}
