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

    [Fact]
    public void Day2()
    {
        var day = new Day_2();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(2265);
        solution.Part2.Should().Be(64097);
    }

    [Fact]
    public void Day3()
    {
        var day = new Day_3();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(539433);
        solution.Part2.Should().Be(75847567);
    }

    [Fact]
    public void Day4()
    {
        var day = new Day_4();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(26443);
        solution.Part2.Should().Be(6284877);
    }

    [Fact]
    public void Day6()
    {
        var day = new Day_6();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(303600);
        solution.Part2.Should().Be(23654842);
    }

    [Fact]
    public void Day7()
    {
        var day = new Day_7();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(250254244);
        solution.Part2.Should().Be(250087440);
    }

    [Fact]
    public void Day8()
    {
        var day = new Day_8();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(19631);
        solution.Part2.Should().Be(21003205388413);
    }

    [Fact]
    public void Day9()
    {
        var day = new Day_9();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(2101499000);
        solution.Part2.Should().Be(1089);
    }
}