using System.Numerics;

namespace AdventOfCode.Core.Extensions;

public static class NumericExtensions
{
    public static bool IsEven(this int number) => number % 2 == 0;
    public static bool IsOdd(this int number) => !IsEven(number);
    public static int MaxBetween(int min, int max) => min > max ? min : max;
    public static long MaxBetween(long min, long max) => min > max ? min : max;
    public static int LimitBy(this int value, int max) => value <= max ? value : max;
    public static long LimitBy(this long value, long max) => value <= max ? value : max;
    public static int AtLeast(this int value, int min) => value < min ? min : value;
    public static long AtLeast(this long value, long min) => value < min ? min : value;
    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }
    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T> => a / GreatestCommonDivisor(a, b) * b;
    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T> => values.Aggregate(LeastCommonMultiple);
}
