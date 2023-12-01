namespace AdventOfCode.Core.Extensions;

public static class StringExtensions
{
    public static int AoCDay(this string str) => int.Parse(str.Split('_')[1]);

    public static int FromBinaryToInt(this string binary) => Convert.ToInt32(binary, 2);

    public static string AsString(this IEnumerable<char> letters) => string.Join("", letters);
}
