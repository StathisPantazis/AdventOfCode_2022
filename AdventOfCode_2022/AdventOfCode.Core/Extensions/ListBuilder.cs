namespace AdventOfCode.Core.Extensions;

public static class ListBuilder
{
    public static List<long> RangeFromTo(long start, long stop, long step = 1)
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

    public static List<int> RangeFromTo(int start, int stop, int step = 1) => RangeFromTo((long)start, stop, step).Select(x => (int)x).ToList();

    public static List<long> RangeFromNTimes(long start, long times, long step = 1)
    {
        var list = new List<long>();
        var counter = 0;

        for (var i = start; counter < times; i += step)
        {
            counter++;
            list.Add(i);
        }

        return list;
    }

    public static List<int> RangeFromNTimes(int start, int times, int step = 1) => RangeFromNTimes(start, times, step).Select(x => (int)x).ToList();

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
}
