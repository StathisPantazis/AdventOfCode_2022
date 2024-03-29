using AdventOfCode.Core.Models;
using AdventOfCode.Tests.Shared;
using AdventOfCode.Version2022;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class Tests2022 : AoCTestBase
{
    [Fact]
    public void Day01()
    {
        var day = new Day_01();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(69177);
        solution.Part2.Should().Be(207456);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day02()
    {
        var day = new Day_02();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(14297);
        solution.Part2.Should().Be(10498);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day03()
    {
        var day = new Day_03();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(7446);
        solution.Part2.Should().Be(2646);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day11()
    {
        var day = new Day_11();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(113232);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day12()
    {
        var day = new Day_12();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(330);
        solution.Part2.Should().Be(321);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day14()
    {
        var day = new Day_14();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(1330);
        solution.Part2.Should().Be(26139);


        PrintRuntime(solution, day.Description);
    }

    //[Fact]
    public void Day17()
    {
        var day = new Day_17();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(3181);
        solution.Part2.Should().Be(1570434782634);


        PrintRuntime(solution, day.Description);
    }

    //[Fact]
    public void Day19()
    {
        var day = new Day_19();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(988);
        solution.Part2.Should().Be(8580);


        PrintRuntime(solution, day.Description);
    }

    [Fact]
    public void Day21()
    {
        var day = new Day_21();
        var solution = day.Solve(AoCResourceType.Solution);

        solution.Part1.Should().Be(41857219607906);
        solution.Part2.Should().Be(3916936880448);


        PrintRuntime(solution, day.Description);
    }
}