namespace AdventOfCode.Core.Models.Interfaces;

public interface IAoCSolution
{
    long Part1Runtime { get; }
    long Part2Runtime { get; }
    long TotalRuntime { get; }
}
