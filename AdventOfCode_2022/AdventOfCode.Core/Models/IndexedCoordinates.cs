using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Models;

/// <summary>
/// x = 0, y = 0 is at the grid's top left corner.
/// </summary>
public class IndexedCoordinates : Coordinates
{
    internal IndexedCoordinates(IGrid grid, bool startFromNegative = false) : base(grid, startFromNegative)
    {
        Y = 0;
    }

    internal IndexedCoordinates(IGrid grid, int startX, int startY) : base(grid, startX, startY)
    {
    }

    protected IndexedCoordinates(int gridWidth, int gridHeight, bool startFromNegative = false) : base(gridWidth, gridHeight, startFromNegative)
    {
        Y = 0;
    }

    protected IndexedCoordinates(int gridWidth, int gridHeight, int startX, int startY) : base(gridWidth, gridHeight, startX, startY)
    {
    }

    protected override bool BottomestRow => Y == Y_Border;
    protected override bool TopestRow => Y == 0;

    public override Coordinates U => Y - 1 >= 0 ? Copy(X, Y - 1) : null;
    public override Coordinates D => Y + 1 <= Y_Border ? Copy(X, Y + 1) : null;
    public override Coordinates UR => X + 1 <= X_Border && Y - 1 >= 0 ? Copy(X + 1, Y - 1) : null;
    public override Coordinates DR => X + 1 <= X_Border && Y + 1 <= Y_Border ? Copy(X + 1, Y + 1) : null;
    public override Coordinates UL => X - 1 >= 0 && Y - 1 >= 0 ? Copy(X - 1, Y - 1) : null;
    public override Coordinates DL => X - 1 >= 0 && Y + 1 <= Y_Border ? Copy(X - 1, Y + 1) : null;

    public override Coordinates Copy() => Copy(X, Y);
    public override Coordinates Copy(int newX, int newY) => new IndexedCoordinates(X_Border + 1, Y_Border + 1, newX, newY);

    public override bool TraverseGrid()
    {
        var lastCol = RightestCol;

        if (X == -1) // start from outside case
        {
            X = 0;
            return true;
        }
        else if (BottomestRow && lastCol)
        {
            return false;
        }

        X += lastCol ? -X_Border : 1;
        Y += lastCol ? 1 : 0;
        return true;
    }

    public override bool ReverseTraverseGrid()
    {
        var lastCol = LeftestCol;

        if (X == X_Border + 1) // start from outside case
        {
            X = X_Border;
            return true;
        }
        else if (TopestRow && lastCol)
        {
            return false;
        }

        X -= lastCol ? -X_Border : 1;
        Y -= lastCol ? 1 : 0;
        return true;
    }

    public override Coordinates GetFromDirectionWithDistance(Direction direction, int distance)
    {
        var newCoord = direction switch
        {
            Direction.R => Copy(X + distance, Y),
            Direction.L => Copy(X - distance, Y),
            Direction.U => Copy(X, Y - distance),
            Direction.D => Copy(X, Y + distance),
            Direction.UR => Copy(X + distance, Y - distance),
            Direction.DR => Copy(X + distance, Y + distance),
            Direction.UL => Copy(X - distance, Y - distance),
            Direction.DL => Copy(X - distance, Y + distance),
            _ => throw new NotImplementedException(),
        };

        return newCoord.IsInsideOfBorder ? newCoord : null;
    }

    protected override int GetYmove(Direction direction)
    {
        return direction switch
        {
            Direction.U or Direction.UR or Direction.UL => -1,
            Direction.D or Direction.DR or Direction.DL => 1,
            _ => 0
        };
    }

    protected override Direction GetDirectionTowards(Coordinates dest)
    {
        return dest switch
        {
            _ when dest.X < X && dest.Y < Y => Direction.UL,
            _ when dest.X < X && dest.Y > Y => Direction.DL,
            _ when dest.X < X => Direction.L,

            _ when dest.X > X && dest.Y < Y => Direction.UR,
            _ when dest.X > X && dest.Y > Y => Direction.DR,
            _ when dest.X > X => Direction.R,

            _ when dest.Y < Y => Direction.U,
            _ when dest.Y > Y => Direction.D,

            _ => throw new ArgumentException()
        };
    }

    protected override Direction GetDirectionOpposite(Coordinates awayFrom)
    {
        return awayFrom switch
        {
            _ when awayFrom.X < X && awayFrom.Y < Y => Direction.DR,
            _ when awayFrom.X < X && awayFrom.Y > Y => Direction.UR,
            _ when awayFrom.X < X => Direction.R,

            _ when awayFrom.X > X && awayFrom.Y < Y => Direction.DL,
            _ when awayFrom.X > X && awayFrom.Y > Y => Direction.UL,
            _ when awayFrom.X > X => Direction.L,

            _ when awayFrom.Y < Y => Direction.D,
            _ when awayFrom.Y > Y => Direction.U,

            _ => throw new ArgumentException()
        };
    }
}
