using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Models;

/// <summary>
/// x = 0, y = 0 is at the grid's bottom left corner.
/// </summary>
public class CartesianCoordinates : Coordinates
{
    internal CartesianCoordinates(IGrid grid, bool startFromNegative = false) : base(grid, startFromNegative)
    {
        X_Border = grid.Width - 1;
        Y_Border = grid.Height - 1;
        X = startFromNegative ? -1 : 0;
    }

    internal CartesianCoordinates(IGrid grid, int startX, int startY) : this(grid)
    {
        X = startX;
        Y = startY;
    }

    protected CartesianCoordinates(int gridWidth, int gridHeight, bool startFromNegative = false) : base(gridWidth, gridHeight, startFromNegative)
    {
        X_Border = gridWidth - 1;
        Y_Border = gridHeight - 1;
        X = startFromNegative ? -1 : 0;
    }

    protected CartesianCoordinates(int gridWidth, int gridHeight, int startX, int startY) : this(gridWidth, gridHeight)
    {
        X = startX;
        Y = startY;
    }

    public override Coordinates Copy() => new CartesianCoordinates(X_Border + 1, Y_Border + 1, X, Y);

    protected override bool BottomestRow => Y == 0;
    protected override bool TopestRow => Y == Y_Border;

    public override Coordinates U => Copy(X, Y + 1);
    public override Coordinates D => Copy(X, Y - 1);
    public override Coordinates UR => Copy(X + 1, Y + 1);
    public override Coordinates DR => Copy(X + 1, Y - 1);
    public override Coordinates UL => Copy(X - 1, Y + 1);
    public override Coordinates DL => Copy(X - 1, Y - 1);

    public override Coordinates Copy(int newX, int newY) => new CartesianCoordinates(X_Border + 1, Y_Border + 1, newX, newY);

    public override bool TraverseGrid()
    {
        var lastCol = RightestCol;

        if (X == -1) // start from outside case
        {
            X = 0;
            return true;
        }
        else if (TopestRow && lastCol)
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
        else if (BottomestRow && lastCol)
        {
            return false;
        }

        X -= lastCol ? -X_Border : 1;
        Y -= lastCol ? 1 : 0;
        return true;
    }

    protected override int GetYmove(Direction direction)
    {
        return direction switch
        {
            Direction.U or Direction.UR or Direction.UL => 1,
            Direction.D or Direction.DR or Direction.DL => -1,
            _ => 0
        };
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

    protected override Direction GetDirectionTowards(Coordinates dest)
    {
        return dest switch
        {
            _ when dest.X < X && dest.Y < Y => Direction.DL,
            _ when dest.X < X && dest.Y > Y => Direction.UL,
            _ when dest.X < X => Direction.L,

            _ when dest.X > X && dest.Y < Y => Direction.DR,
            _ when dest.X > X && dest.Y > Y => Direction.UR,
            _ when dest.X > X => Direction.R,

            _ when dest.Y < Y => Direction.D,
            _ when dest.Y > Y => Direction.U,

            _ => throw new ArgumentException()
        };
    }

    protected override Direction GetDirectionOpposite(Coordinates dest)
    {
        return dest switch
        {
            _ when dest.X < X && dest.Y < Y => Direction.UR,
            _ when dest.X < X && dest.Y > Y => Direction.DR,
            _ when dest.X < X => Direction.R,

            _ when dest.X > X && dest.Y < Y => Direction.UL,
            _ when dest.X > X && dest.Y > Y => Direction.DL,
            _ when dest.X > X => Direction.L,

            _ when dest.Y < Y => Direction.U,
            _ when dest.Y > Y => Direction.D,

            _ => throw new ArgumentException()
        };
    }
}
