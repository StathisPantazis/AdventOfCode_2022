namespace AdventOfCode.Core.Models.Bases;

public abstract class AoCBaseDay<TPart1, TPart2, TArgs>
{
    public abstract AoCSolution<TPart1, TPart2> Solve(AoCResourceType resourceType);

    protected abstract TPart1 Part1(TArgs args);

    protected abstract TPart2 Part2(TArgs args);
}
