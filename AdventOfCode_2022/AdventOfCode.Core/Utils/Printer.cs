using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;

namespace AdventOfCode.Core.Utils;

public static class Printer
{
    public static void Print<T>(this IEnumerable<T> list)
    {
        Console.WriteLine(list.ListToString());
    }

    public static void Print<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        Console.WriteLine(dict.DictionaryToString());
    }

    public static void Print(this IGrid grid)
    {
        Console.WriteLine(grid.ToString());
    }

    public static void Print(this Coordinates coord)
    {
        Console.WriteLine(coord.ToString());
    }
}
