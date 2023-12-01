using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models.Bases;

public abstract class AoCBaseDay<TPart1, TPart2, TArgs> : IDay
{
    public abstract AoCSolution<TPart1, TPart2> Solve(AoCResourceType resourceType);

    protected abstract TPart1 Part1(TArgs args);

    protected abstract TPart2 Part2(TArgs args);

    protected FileDescription FileDescription(IDay day, AoCResourceType resourceType = AoCResourceType.Solution) => new(day, resourceType);
}
