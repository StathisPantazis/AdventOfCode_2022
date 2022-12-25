using System.Xml.Serialization;

namespace AdventOfCode_2022.Extensions;

internal static class ListExtensions {
    public static int NMax<T>(this IEnumerable<T> collection, int nth, Func<T, int> selector) {
        return collection.Select(selector).OrderByDescending(x => x).Skip(nth - 1).First();
    }

    public static int NMin<T>(this IEnumerable<T> collection, int nth, Func<T, int> selector) {
        return collection.Select(selector).OrderBy(x => x).Skip(nth - 1).First();
    }

    public static void ForEachDo<T>(this IEnumerable<T> collection, Action<T> action) {
        foreach (T item in collection) {
            action(item);
        }
    }

    public static void ForEachDoBreak<T>(this IEnumerable<T> collection, Action<T> action, Func<T, bool> breakClause) {
        foreach (T item in collection) {
            action(item);

            if (breakClause(item)) {
                break;
            }
        }
    }

    public static void ForNTimesDo(int iterations, Action<int> action) {
        Enumerable.Range(0, iterations).ToList().ForEach(x => action(x));
    }

    public static void ForNTimesDo(int iterations, Action action) {
        Enumerable.Range(0, iterations).ToList().ForEach(x => action());
    }

    public static List<T> ForNTimesFill<T>(int iterations, Func<int, T> func) {
        List<T> list = new();
        ForNTimesDo(iterations, (int i) => list.Add(func(i)));
        return list;
    }

    public static List<T> ForNTimesFill<T>(int iterations, Func<T> func) {
        List<T> list = new();
        ForNTimesDo(iterations, () => list.Add(func()));
        return list;
    }

    public static T Pop<T>(this List<T> list) {
        if (list == null || list.Count == 0) {
            throw new Exception("Cannot perform function.");
        }

        T currentFirst = list[0];
        list.RemoveAt(0);
        return currentFirst;
    }

    public static T PopChange<T>(this List<T> list, T change) {
        if (list == null || list.Count == 0) {
            throw new Exception("Cannot perform function.");
        }

        list.RemoveAt(0);
        return change;
    }

    public static bool None<T>(this List<T> source, Func<T, bool> predicate) {
        foreach (T element in source) {
            if (predicate(element)) {
                return false;
            }
        }

        return true;
    }
}
