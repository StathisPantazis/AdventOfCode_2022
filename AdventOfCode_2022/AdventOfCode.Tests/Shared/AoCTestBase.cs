using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.Tests.Shared;

public abstract class AoCTestBase
{
    protected static void PrintRuntime(IAoCSolution solution, FileDescription description)
    {
        Debug.WriteLine($"{description.AoCYear} - Day {description.AoCDay}");
        Debug.WriteLine($"Part 1: {solution.Part1Runtime}ms");
        Debug.WriteLine($"Part 2: {solution.Part2Runtime}ms");
        Debug.WriteLine($"Total: {solution.TotalRuntime}ms\n");
    }
}
