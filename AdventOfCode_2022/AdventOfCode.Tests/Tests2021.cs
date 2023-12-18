using AdventOfCode.Core.Models;
using AdventOfCode.Tests.Shared;
using AdventOfCode.Version2021;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2021 : AoCTestBase
{
    private readonly string _year = nameof(Tests2023).Replace("Test", "");

    [Fact]
    public void Day01()
    {
        var day = new Day_01();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(1553);
        solution.Part2.Should().Be(1597);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day02()
    {
        var day = new Day_02();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(1654760);
        solution.Part2.Should().Be(1956047400);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day03()
    {
        var day = new Day_03();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(2967914);
        solution.Part2.Should().Be(7041258);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day04()
    {
        var day = new Day_04();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(35670);
        solution.Part2.Should().Be(22704);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day05()
    {
        var day = new Day_05();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(7380);
        solution.Part2.Should().Be(21373);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day06()
    {
        var day = new Day_06();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(349549);
        solution.Part2.Should().Be(1589590444365);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day07()
    {
        var day = new Day_07();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(343605);
        solution.Part2.Should().Be(96744904);

        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day08()
    {
        var day = new Day_08();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(440);
        solution.Part2.Should().Be(1046281);

        PrintRuntime(solution, day.Description);
    }
}