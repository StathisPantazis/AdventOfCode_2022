using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Utils;

public static class Printer
{
    public static void Print<T>(this IEnumerable<T> list, bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);
        Console.WriteLine(list.ListToString());
    }

    public static void Print<T>(this List<IndexedGrid<T>> grids, string itemSeparator = "", string rowSeparator = "\n", bool clearConsole = true, bool padTop = true)
        => PrintGridList(grids.Select(x => (Grid<T>)x).ToList(), itemSeparator, rowSeparator, clearConsole, padTop);

    public static void Print<T>(this List<CartesianGrid<T>> grids, string itemSeparator = "", string rowSeparator = "\n", bool clearConsole = true, bool padTop = true)
        => PrintGridList(grids.Select(x => (Grid<T>)x).ToList(), itemSeparator, rowSeparator, clearConsole, padTop);

    public static void Print<T>(this List<List<T>> list, string rowSeparator = "", bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);

        foreach (var line in list)
        {
            Console.WriteLine(line.ListToString(rowSeparator));
        }
    }

    public static void Print<TKey, TValue>(this IDictionary<TKey, TValue> dict, bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);
        Console.WriteLine(dict.DictionaryToString());
    }

    public static void Print(this IGrid grid, string itemSeparator = "", string rowSeparator = "\n", bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);
        Console.WriteLine(grid.ToString(itemSeparator, rowSeparator));
    }

    public static void Print(this IGrid grid, bool clearConsole, bool padTop = true) => grid.Print("", "\n", clearConsole: clearConsole, padTop: padTop);

    public static void Print(this Coordinates coord, bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);
        Console.WriteLine(coord.ToString());
    }

    private static void PrintGridList<T>(List<Grid<T>> grids, string itemSeparator = "", string rowSeparator = "\n", bool clearConsole = true, bool padTop = true)
    {
        ClearConsole(clearConsole, padTop);

        foreach (var grid in grids)
        {
            grid.Print(itemSeparator, rowSeparator, clearConsole: false, padTop: false);
            Console.WriteLine();
        }
    }

    private static void ClearConsole(bool clearConsole, bool padTop)
    {
        try
        {
            if (clearConsole)
            {
                Console.Clear();
            }
            else if (padTop)
            {
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        catch (IOException)
        {
        }
    }
}
