using AdventOfCode.Core.Models.Interfaces;
using System.Diagnostics;

namespace AdventOfCode.Core.Models.Bases;

public abstract class AoCBaseDay<TPart1, TPart2, TArgs> : IDay
{
    public abstract AoCSolution<TPart1, TPart2> Solve(AoCResourceType resourceType);

    public FileDescription Description { get; private set; }

    protected abstract TPart1 Part1(TArgs args);
    protected abstract TPart2 Part2(TArgs args);

    protected AoCSolution<TPart1, TPart2> Solution(TArgs args) => Solution(args, args);

    protected AoCSolution<TPart1, TPart2> Solution(TArgs argsPart1, TArgs argsPart2)
    {
        var sw = new Stopwatch();
        sw.Start();

        sw.Restart();
        var part1 = Part1(argsPart1);
        var part1Runtime = sw.ElapsedMilliseconds;

        sw.Restart();
        var part2 = Part2(argsPart2);
        var part2Runtime = sw.ElapsedMilliseconds;

        return new AoCSolution<TPart1, TPart2>(part1, part2)
        {
            Part1Runtime = part1Runtime,
            Part2Runtime = part2Runtime,
        };
    }

    protected FileDescription FileDescription(IDay day, AoCResourceType resourceType = AoCResourceType.Solution)
    {
        Description = new FileDescription(day, resourceType);
        return Description;
    }
}
