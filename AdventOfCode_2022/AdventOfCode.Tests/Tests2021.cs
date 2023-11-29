using AdventOfCode.Core.Models;
using AdventOfCode.Version2021;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2021
{
    [Fact]
    public void Day1()
    {
        var day = new Day_1();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(1553);
        solution.Part2.Should().Be(1597);
    }

    [Fact]
    public void Day2()
    {
        var day = new Day_2();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(1654760);
        solution.Part2.Should().Be(1956047400);
    }
}