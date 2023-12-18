using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models;

public record struct AoCSolution<T1, T2>(T1 Part1, T2 Part2) : IAoCSolution
{
    public long Part1Runtime { get; set; }
    public long Part2Runtime { get; set; }
    public readonly long TotalRuntime => Part1Runtime + Part2Runtime;

    public readonly void Print()
    {
        Console.WriteLine($"Part 1 ({Part1Runtime}ms): {Part1}");
        Console.WriteLine($"Part 2 ({Part2Runtime}ms): {Part2}");
        Console.WriteLine($"Total Runtime: {TotalRuntime}ms");
    }
}
