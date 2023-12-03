namespace AdventOfCode.Core.Extensions;

public static class ListBuilder
{
    public static List<int> IntRange(int start, int stop, int step = 1)
    {
        var list = new List<int>();

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
