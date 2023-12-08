using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Utils;

public static class Printer
{
    public static void Print<T>(this IEnumerable<T> list, bool clearConsole = true)
    {
        ClearConsole(clearConsole);
        Console.WriteLine(list.ListToString());
    }

    public static void Print<T>(this List<List<T>> list, string rowSeparator = "", bool clearConsole = true)
    {
        ClearConsole(clearConsole);

        foreach (var line in list)
        {
            Console.WriteLine(line.ListToString(rowSeparator));
        }
    }

    public static void Print<TKey, TValue>(this IDictionary<TKey, TValue> dict, bool clearConsole = true)
    {
        ClearConsole(clearConsole);
        Console.WriteLine(dict.DictionaryToString());
    }

    public static void Print(this IGrid grid, string itemSeparator = "", string rowSeparator = "\n", bool clearConsole = true)
    {
        ClearConsole(clearConsole);
        Console.WriteLine(grid.ToString());
    }

    public static void Print(this IGrid grid, bool clearConsole) => grid.Print("", "\n", clearConsole: clearConsole);

    public static void Print(this Coordinates coord, bool clearConsole = true)
    {
        ClearConsole(clearConsole);
        Console.WriteLine(coord.ToString());
    }

    private static void ClearConsole(bool clearConsole)
    {
        try
        {
            if (clearConsole)
            {
                Console.Clear();
            }
            else
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
