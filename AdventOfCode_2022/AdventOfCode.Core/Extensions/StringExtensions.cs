namespace AdventOfCode.Core.Extensions;

public static class StringExtensions
{
    public static int FromBinaryToInt(this string binary) => Convert.ToInt32(binary, 2);
}
