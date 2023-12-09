namespace AdventOfCode.Core.Utils;

public static class ListBuilder
{
    public static List<long> FromXtoN(long start, long stop, long step = 1)
    {
        var list = new List<long>();

        if (start < stop && step > 0)
        {
            for (var i = start; i < stop; i += step)
            {
                list.Add(i);
            }
        }
        else if (start > stop && step < 0)
        {
            for (var i = start; i > stop; i += step)
            {
                list.Add(i);
            }
        }

        return list;
    }

    public static List<int> FromXtoN(int start, int stop, int step = 1) => FromXtoN((long)start, stop, step).Select(x => (int)x).ToList();

    public static List<long> ForI(long start, long times, long step = 1)
    {
        var list = new List<long>();

        for (var i = start; i < start + times; i += step)
        {
            list.Add(i);
        }

        return list;
    }

    public static List<int> ForI(long times, long step = 1) => ForI(0, times, step).Select(x => (int)x).ToList();
    public static List<int> ForI(int times, int step = 1) => ForI((long)0, times, step).Select(x => (int)x).ToList();
    public static List<int> ForI(int start, int times, int step = 1) => ForI((long)start, times, step).Select(x => (int)x).ToList();

    public static List<char> CharRange(char start, char stop, int step = 1)
    {
        var list = new List<char>();

        if (char.IsUpper(start) != char.IsUpper(stop))
        {
            throw new ArgumentException("Start and stop characters must both be uppercase or lowercase.");
        }

        var startInt = (int)start;
        var stopInt = (int)stop;

        if (startInt < stopInt && step > 0)
        {
            for (var i = startInt; i <= stopInt; i += step)
            {
                list.Add((char)i);
            }
        }
        else if (startInt > stopInt && step < 0)
        {
            for (var i = startInt; i >= stopInt; i += step)
            {
                list.Add((char)i);
            }
        }

        return list;
    }

    public static List<string> StringRange(string start, string stop, int step = 1) => CharRange(char.Parse(start), char.Parse(stop), step).Select(x => x.ToString()).ToList();

    public static List<T> Repeat<T>(int count, T emptyValue = default)
    {
        var list = new List<T>();

        for (var i = 0; i < count; i++)
        {
            var value = emptyValue ?? GenericBuilder.GetDefault<T>();
            list.Add(value);
        }

        return list;
    }
}
