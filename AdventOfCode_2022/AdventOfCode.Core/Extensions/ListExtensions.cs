using System.Collections;
using System.Text;

namespace AdventOfCode.Core.Extensions;

public static class ListExtensions
{
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

    public static string DictionaryToString<TKey, TValue>(this IDictionary<TKey, TValue> dict, string separator = "\n")
        => string.Join(separator, dict.Select(pair => $"{pair.Key}  |  {(typeof(IEnumerable).IsAssignableFrom(typeof(TValue)) ? ((IEnumerable)pair.Value).ListToString(",") : pair.Value)}"));
}
