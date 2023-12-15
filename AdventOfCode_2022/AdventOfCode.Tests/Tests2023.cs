using AdventOfCode.Core.Models;
using AdventOfCode.Version2023;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2023
{
    [Fact]
    public void Day01()
    {
        var day = new Day_01();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(53386);
        solution.Part2.Should().Be(53312);
    }

    [Fact]
    public void Day02()
    {
        var day = new Day_02();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(2265);
        solution.Part2.Should().Be(64097);
    }

    [Fact]
    public void Day03()
    {
        var day = new Day_03();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(539433);
        solution.Part2.Should().Be(75847567);
    }

    [Fact]
    public void Day04()
    {
        var day = new Day_04();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(26443);
        solution.Part2.Should().Be(6284877);
    }

    [Fact]
    public void Day05()
    {
        var day = new Day_05();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(323142486);
        solution.Part2.Should().Be(79874951);
    }

    [Fact]
    public void Day06()
    {
        var day = new Day_06();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(303600);
        solution.Part2.Should().Be(23654842);
    }

    [Fact]
    public void Day07()
    {
        var day = new Day_07();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(250254244);
        solution.Part2.Should().Be(250087440);
    }

    [Fact]
    public void Day08()
    {
        var day = new Day_08();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(19631);
        solution.Part2.Should().Be(21003205388413);
    }

    [Fact]
    public void Day09()
    {
        var day = new Day_09();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(2101499000);
        solution.Part2.Should().Be(1089);
    }

    [Fact]
    public void Day10()
    {
        var day = new Day_10();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(6864);
        solution.Part2.Should().Be(349);
    }

    [Fact]
    public void Day11()
    {
        var day = new Day_11();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(9312968);
        solution.Part2.Should().Be(597714117556);
    }

    [Fact]
    public void Day13()
    {
        var day = new Day_13();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(31956);
        solution.Part2.Should().Be(37617);
    }

    [Fact]
    public void Day14()
    {
        var day = new Day_14();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(109098);
        solution.Part2.Should().Be(100064);
    }

    [Fact]
    public void Day15()
    {
        var day = new Day_15();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(516469);
        solution.Part2.Should().Be(221627);
    }
}