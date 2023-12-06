namespace AdventOfCode.Core.Extensions;

public static class NumericExtensions
{
    public static int MaxBetween(int min, int max) => min > max ? min : max;
    public static long MaxBetween(long min, long max) => min > max ? min : max;
    public static int LimitBy(this int value, int max) => value <= max ? value : max;
    public static long LimitBy(this long value, long max) => value <= max ? value : max;
    public static int AtLeast(this int value, int min) => value < min ? min : value;
    public static long AtLeast(this long value, long min) => value < min ? min : value;
}
