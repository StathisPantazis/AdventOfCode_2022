using AdventOfCode.Core.Models;
using AdventOfCode.Version2023;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2023
{
    [Fact]
    public void Day1()
    {
        var day = new Day_1();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(53386);
        solution.Part2.Should().Be(53312);
    }
}