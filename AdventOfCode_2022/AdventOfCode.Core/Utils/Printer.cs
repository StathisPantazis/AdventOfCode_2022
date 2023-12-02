using AdventOfCode.Core.Models;

namespace AdventOfCode.Core.Utils;

public static class Printer
{
    public static void Print<T>(this IEnumerable<T> list)
    {
        var print = string.Join("\n", list.Select(x => x.ToString()));
        Console.WriteLine(print);
    }

    public static void Print<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        var print = string.Join("\n", dict.Select(pair => $"{pair.Key}  |  {pair.Value}"));
        Console.WriteLine(print);
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
