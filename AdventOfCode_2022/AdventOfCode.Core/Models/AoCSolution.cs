namespace AdventOfCode.Core.Models;

public readonly record struct AoCSolution<T1, T2>(T1 Part1, T2 Part2)
{
    public void Print()
    {
        Console.WriteLine($"Part 1: {Part1}");
        Console.WriteLine($"Part 2: {Part2}");
    }
}
