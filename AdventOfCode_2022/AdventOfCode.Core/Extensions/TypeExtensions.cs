namespace AdventOfCode.Core.Extensions;

public static class TypeExtensions
{
    public static int AoCYear(this Type type) => int.Parse(type.Namespace!.TakeLast(4).AsString());
}
