using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2021.Day_02;

namespace AdventOfCode.Version2021;

public class Day_02 : AoCBaseDay<int, int, Tuple<Dir, int>[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var movements = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Split(' ').ToArray() is string[] arr ? Tuple.Create(Enum.Parse<Dir>(arr[0]), int.Parse(arr[1])) : null)
            .ToArray();

        return Solution(movements);
    }

    protected override int Part1(Tuple<Dir, int>[] movements)
    {
        var horizontal = movements
            .Where(x => x.Item1 is Dir.forward)
            .Sum(x => x.Item2);

        var depth = movements
            .Where(x => x.Item1 is not Dir.forward)
            .Sum(x => x.Item1 is Dir.down ? x.Item2 : -x.Item2);

        return horizontal * depth;
    }

    protected override int Part2(Tuple<Dir, int>[] movements)
    {
        var aim = 0;
        var horizontal = 0;
        var depth = 0;

        foreach (var movement in movements)
        {
            if (movement.Item1 is Dir.down or Dir.up)
            {
                aim += movement.Item1 is Dir.down ? movement.Item2 : -movement.Item2;
            }
            else
            {
                horizontal += movement.Item2;
                depth += movement.Item2 * aim;
            }
        }

        return horizontal * depth;
    }

    public enum Dir
    {
        forward,
        up,
        down,
    }
}

