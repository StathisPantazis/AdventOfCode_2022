using AdventOfCode.Core.Models;

namespace AdventOfCode.Core.Extensions;

public static class EnumExtensions
{
    public static Direction GetOpposite(this Direction direction)
    {
        return direction switch
        {
            Direction.D => Direction.U,
            Direction.U => Direction.D,
            Direction.L => Direction.R,
            Direction.R => Direction.L,
            Direction.UL => Direction.DR,
            Direction.UR => Direction.DL,
            Direction.DL => Direction.UR,
            Direction.DR => Direction.DL,
        };
    }

    public static bool IsOpposite(this Direction direction, Direction other)
    {
        return direction switch
        {
            Direction.D => other is Direction.U,
            Direction.U => other is Direction.D,
            Direction.L => other is Direction.R,
            Direction.R => other is Direction.L,
            Direction.UL => other is Direction.DR,
            Direction.UR => other is Direction.DL,
            Direction.DL => other is Direction.UR,
            Direction.DR => other is Direction.DL,
        };
    }
}
