using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models.Bases;

public abstract class AoCBaseDay<TPart1, TPart2, TArgs> : IDay
{
    public abstract AoCSolution<TPart1, TPart2> Solve(AoCResourceType resourceType);

    protected abstract TPart1 Part1(TArgs args);

    protected abstract TPart2 Part2(TArgs args);

    protected AoCSolution<TPart1, TPart2> Solution(TArgs args)
    {
        return new AoCSolution<TPart1, TPart2>(Part1(args), Part2(args));
    }

    protected AoCSolution<TPart1, TPart2> ReturnResult(TArgs argsPart1, TArgs argsPart2)
    {
        return new AoCSolution<TPart1, TPart2>(Part1(argsPart1), Part2(argsPart2));
    }

    protected FileDescription FileDescription(IDay day, AoCResourceType resourceType = AoCResourceType.Solution) => new(day, resourceType);
}
