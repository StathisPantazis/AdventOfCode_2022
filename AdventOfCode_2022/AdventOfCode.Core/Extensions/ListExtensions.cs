using System.Collections;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Core.Extensions;

public static class ListExtensions
{
    public static List<T> ReverseList<T>(this List<T> list)
    {
        list.Reverse();
        return list;
    }

    public static List<T> RemoveFirst<T>(this List<T> list, bool condition) => condition ? RemoveFirst(list) : list;
    public static List<T> RemoveFirst<T>(this List<T> list, Func<T, bool> condition = null)
    {
        if (list.Count > 0 && (condition == null || condition(list[0])))
        {
            list.RemoveAt(0);
        }

        return list;
    }

    public static List<T> RemoveLast<T>(this List<T> list, bool condition) => condition ? RemoveLast(list) : list;
    public static List<T> RemoveLast<T>(this List<T> list, Func<T, bool> condition = null)
    {
        if (list.Count > 0 && (condition == null || condition(list[^1])))
        {
            list.RemoveAt(list.Count - 1);
        }

        return list;
    }

    public static List<T> AddIf<T>(this List<T> list, T newElement, bool condition)
    {
        if (condition)
        {
            list.Add(newElement);
        }

        return list;
    }

    public static HashSet<T> AddIf<T>(this HashSet<T> list, T newElement, bool condition)
    {
        if (condition)
        {
            list.Add(newElement);
        }

        return list;
    }

    public static int CountMany<T>(this List<T> list, Func<T, IEnumerable<T>> selector) => list.SelectMany(x => selector(x)).Count();

    public static int NMax<T>(this IEnumerable<T> collection, int nth, Func<T, int> selector)
    {
        return collection.Select(selector).OrderByDescending(x => x).Skip(nth - 1).First();
    }

    public static int NMin<T>(this IEnumerable<T> collection, int nth, Func<T, int> selector)
    {
        return collection.Select(selector).OrderBy(x => x).Skip(nth - 1).First();
    }

    public static void ForEachDo<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static void ForEachDoBreak<T>(this IEnumerable<T> collection, Action<T> action, Func<T, bool> breakClause)
    {
        foreach (var item in collection)
        {
            action(item);

            if (breakClause(item))
            {
                break;
            }
        }
    }

    public static void ForNTimesDo(int iterations, Action<int> action)
    {
        Enumerable.Range(0, iterations).ToList().ForEach(x => action(x));
    }

    public static void ForNTimesDo(int iterations, Action action)
    {
        Enumerable.Range(0, iterations).ToList().ForEach(x => action());
    }

    public static List<T> ForNTimesFill<T>(int iterations, Func<int, T> func)
    {
        List<T> list = new();
        ForNTimesDo(iterations, (i) => list.Add(func(i)));
        return list;
    }

    public static List<T> ForNTimesFill<T>(int iterations, Func<T> func)
    {
        List<T> list = new();
        ForNTimesDo(iterations, () => list.Add(func()));
        return list;
    }

    public static T Pop<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new Exception("Cannot perform function.");
        }

        var currentFirst = list[0];
        list.RemoveAt(0);
        return currentFirst;
    }

    public static T PopChange<T>(this List<T> list, T change)
    {
        if (list == null || list.Count == 0)
        {
            throw new Exception("Cannot perform function.");
        }

        list.RemoveAt(0);
        return change;
    }

    public static bool None<T>(this List<T> source, Func<T, bool> predicate)
    {
        foreach (var element in source)
        {
            if (predicate(element))
            {
                return false;
            }
        }

        return true;
    }

    public static void AddRangeDistinct<T>(this List<T> list, List<T> newList)
    {
        var distinctItems = new HashSet<T>(list);

        foreach (var item in newList)
        {
            distinctItems.Add(item);
        }

        list.Clear();
        list.AddRange(distinctItems);
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T exceptedElement)
    {
        return list.Where(x => !x.Equals(exceptedElement)).ToList();
    }

    /// <summary>
    /// Turns { 1, 2, 3 }, { 1, 2, 3 } to { 1, 1 }, { 2, 2 }, { 3, 3 }
    /// </summary>
    public static List<List<T>> RotateRowsColumns<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var list = source.Select(x => x.ToList()).ToList();
        var size = list.First().Count;

        if (list.Any(x => x.Count != size))
        {
            throw new ArgumentException("Every list should have the same amount of elements.");
        }

        var rowCount = list.Count;
        var newLists = new List<List<T>>();

        for (var column = 0; column < size; column++)
        {
            var newList = new List<T>();
            newLists.Add(newList);

            for (var row = 0; row < rowCount; row++)
            {
                newList.Add(list[row][column]);
            }
        }

        return newLists;
    }

    public static string DictionaryToString<TKey, TValue>(this IDictionary<TKey, TValue> dict, string separator = "\n")
        => string.Join(separator, dict.Select(pair => $"{pair.Key}  |  {(typeof(IEnumerable).IsAssignableFrom(typeof(TValue)) ? ((IEnumerable)pair.Value).ListToString(",") : pair.Value)}"));

    public static string ListToString<T>(this IEnumerable<T> list, string separator = "\n") => string.Join(separator, list.Select(x => x.ToString()));

    public static string ListToString(this IEnumerable list, string separator = "\n")
    {
        var stringBuilder = new StringBuilder();

        foreach (var item in list)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(separator);
            }

            stringBuilder.Append(item.ToString() ?? string.Empty);
        }

        return stringBuilder.ToString();
    }

    public static string AsString(this IEnumerable<char> letters) => string.Join(string.Empty, letters);

    public static int Multiply<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) => Multiply(source, selector);
    public static long Multiply<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) => Multiply(source, selector);
    public static float Multiply<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector) => Multiply(source, selector);
    public static double Multiply<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector) => Multiply(source, selector);
    public static decimal Multiply<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector) => Multiply(source, selector);
    public static TResult Multiply<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        where TResult : struct, INumber<TResult>
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        using var iterator = source.GetEnumerator();
        if (!iterator.MoveNext())
        {
            return default;
        }

        var total = selector(iterator.Current);
        while (iterator.MoveNext())
        {
            var currentValue = selector(iterator.Current);
            total *= currentValue;
        }

        return total;
    }
    public static int Multiply(this IEnumerable<int> source) => Multiply<int, int>(source);
    public static long Multiply(this IEnumerable<long> source) => Multiply<long, long>(source);
    public static float Multiply(this IEnumerable<float> source) => (float)Multiply<float, double>(source);
    public static double Multiply(this IEnumerable<double> source) => Multiply<double, double>(source);
    public static decimal Multiply(this IEnumerable<decimal> source) => Multiply<decimal, decimal>(source);
    private static TResult Multiply<TSource, TResult>(this IEnumerable<TSource> source)
        where TSource : struct, INumber<TSource>
        where TResult : struct, INumber<TResult>
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var product = TResult.One;
        foreach (var item in source)
        {
            product *= TResult.CreateChecked(item);
        }

        return product;
    }
}
