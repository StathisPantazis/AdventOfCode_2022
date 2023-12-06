using System.Collections;
using System.Numerics;
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
