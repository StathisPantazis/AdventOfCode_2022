using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core.Tests;

public class FastListGrid<T> : IGrid
{
    private readonly T _defaultValue;

    public T this[int x, int y]
    {
        get => Rows[y][x];
        set => Rows[y][x] = value;
    }

    public T this[FastCoordinates pos]
    {
        get => this[pos.X, pos.Y];
        set => this[pos.X, pos.Y] = value;
    }

    public FastListGrid(IEnumerable<IEnumerable<T>> source, T defaultValue)
    {
        _defaultValue = defaultValue;

        Rows = source.Select(line => line.Select(elem => (T)(object)elem).ToList()).ToList();
        Rebuild();
    }

    public List<List<T>> Rows { get; private set; }

    public List<List<T>> Columns { get; private set; }

    public int Height { get; private set; }

    public int Width { get; private set; }

    // METHODS
    public List<T> Row(int y) => Rows[y];

    public List<T> Column(int x) => Columns[x];

    public void AddRow(List<T> newRow)
    {
        Rows.Add(newRow);
        Rebuild();
    }

    public string ToString(string itemSeparator = "", string rowSeparator = "\n") => string.Join(rowSeparator, Rows.Select(x => x.ListToString(itemSeparator)));

    private void Rebuild()
    {
        Height = Rows.Count;
        Width = Rows.Max(x => x.Count);

        Columns = ListBuilder.ForI(Width)
            .Select(x => ListBuilder.ForI(Height)
                .Select(y => x < Width ? Rows[y][x] : _defaultValue)
                .ToList())
            .ToList();
    }
}