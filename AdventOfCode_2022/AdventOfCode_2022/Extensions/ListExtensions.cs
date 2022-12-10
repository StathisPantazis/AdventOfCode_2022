namespace AdventOfCode_2022.Extensions;

internal static class ListExtensions {
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
}
