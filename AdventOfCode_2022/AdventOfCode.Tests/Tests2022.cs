using AdventOfCode.Core.Models;
using AdventOfCode.Version2022;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2022
{
    [Fact]
    public void Day1()
    {
        var day = new Day_1();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be("69177");
        solution.Part2.Should().Be("207456");
    }

    [Fact]
    public void Day2()
    {
        var day = new Day_2();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(14297);
        solution.Part2.Should().Be(10498);
    }
}