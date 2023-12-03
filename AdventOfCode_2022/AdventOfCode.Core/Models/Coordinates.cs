namespace AdventOfCode.Core.Models;

public class Coordinates
{
    public Coordinates()
    {
        X_Border = int.MaxValue;
        Y_Border = int.MaxValue;
    }

    public Coordinates(int gridWidth, int gridHeight, bool startFromNegative = false)
    {
        X_Border = gridWidth - 1;
        Y_Border = gridHeight - 1;
        X = startFromNegative ? -1 : 0;
    }

    public Coordinates(int gridWidth, int gridHeight, int startX, int startY) : this(gridWidth, gridHeight)
    {
        X = startX;
        Y = startY;
    }

    public Coordinates(IGrid grid, bool startFromNegative = false)
    {
        X_Border = grid.Width - 1;
        Y_Border = grid.Height - 1;
        X = startFromNegative ? -1 : 0;
    }

    public Coordinates(IGrid grid, int startX, int startY) : this(grid)
    {
        X = startX;
        Y = startY;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int X_Border { get; }
    public int Y_Border { get; }
    public int StepsTraversed { get; private set; }
    public bool IsOnBorder => FirstRow || FirstCol || LastRow || LastCol;
    public bool IsOutsideOfBorder => X < 0 || Y < 0 || X > X_Border || Y > Y_Border;
    public bool IsInsideOfBorder => !IsOutsideOfBorder;
    public bool IsAtStart => FirstRow && FirstCol;
    public bool IsAtEnd => LastRow && LastCol;
    public Coordinates R => CopyBase(X + 1, Y);
    public Coordinates L => CopyBase(X - 1, Y);
    public Coordinates U => CopyBase(X, Y - 1);
    public Coordinates D => CopyBase(X, Y + 1);
    public Coordinates UR => CopyBase(X + 1, Y - 1);
    public Coordinates DR => CopyBase(X + 1, Y + 1);
    public Coordinates UL => CopyBase(X - 1, Y - 1);
    public Coordinates DL => CopyBase(X - 1, Y + 1);

    private bool FirstRow => Y == 0;
    private bool LastRow => Y == Y_Border;
    private bool FirstCol => X == 0;
    private bool LastCol => X == X_Border;

    public void GoToStart(bool startFromNegative = false)
    {
        X = startFromNegative ? -1 : 0;
        Y = 0;
    }

    public void GoToEnd()
    {
        X = X_Border;
        Y = Y_Border;
    }

    public bool TryGetNeighbour(Direction direction, out Coordinates neighbour)
    {
        neighbour = GetFromDirection(direction);
        return CanMove(direction, neighbour);
    }

    public bool Move(Direction direction, Func<Coordinates, bool> stopClause = null, bool allowOutside = false)
    {
        var nextMove = GetFromDirection(direction);
        if (CantMove(direction, nextMove, stopClause, allowOutside))
        {
            return false;
        }

        X += GetXmove(direction);
        Y += GetYmove(direction);
        return true;
    }

    public bool MoveTowards(Coordinates pos, bool allowOverlap = false, Func<Coordinates, bool> stopClause = null)
    {
        if (pos.Equals(this))
        {
            return false;
        }

        var direction = pos switch
        {
            _ when pos.X < X && pos.Y < Y => Direction.UL,
            _ when pos.X < X && pos.Y > Y => Direction.DL,
            _ when pos.X < X => Direction.L,

            _ when pos.X > X && pos.Y < Y => Direction.UR,
            _ when pos.X > X && pos.Y > Y => Direction.DR,
            _ when pos.X > X => Direction.R,

            _ when pos.Y < Y => Direction.U,
            _ when pos.Y > Y => Direction.D,

            _ => throw new ArgumentException()
        };

        int xMove = GetXmove(direction), yMove = GetYmove(direction);

        // overlap will take place on move
        if (!allowOverlap && xMove + X == pos.X && yMove + Y == pos.Y)
        {
            return false;
        }
        return Move(direction, stopClause: stopClause);
    }

    public bool MoveOpposite(Coordinates pos)
    {
        if (pos.Equals(this))
        {
            return false;
        }

        Direction? direction = pos switch
        {
            _ when pos.X == X && pos.Y < Y => Direction.D,
            _ when pos.X == X && pos.Y > Y => Direction.U,
            _ when pos.Y == Y && pos.X < X => Direction.R,
            _ when pos.Y == Y && pos.X > X => Direction.L,

            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X < X && pos.Y < Y => Direction.DR,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X < X && pos.Y > Y => Direction.UR,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X > X && pos.Y < Y => Direction.DL,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X > X && pos.Y > Y => Direction.UL,
            _ => null
        };

        return direction is not null && Move((Direction)direction);
    }

    public bool TraverseGrid()
    {
        if (X == -1) // for start with negative case
        {
            X = 0;
            return true;
        }
        else if (LastRow && LastCol)
        {
            return false;
        }

        var lastCol = LastCol;
        X += lastCol ? -X_Border : 1;
        Y += lastCol ? 1 : 0;
        StepsTraversed++;
        return true;
    }

    public override string ToString() => $"{X} , {Y}";

    public override bool Equals(object? obj) => obj is Coordinates coord && coord.X == X && coord.Y == Y;

    public override int GetHashCode() => $"{X}{Y}".GetHashCode();

    public bool Equals(Coordinates otherPos) => otherPos.X == X && Y == otherPos.Y;

    public bool NotEquals(Coordinates otherPos) => otherPos.X != X || Y != otherPos.Y;

    public Coordinates Copy() => new(X_Border + 1, Y_Border + 1, X, Y);

    private static int GetXmove(Direction direction)
    {
        return direction switch
        {
            Direction.L or Direction.UL or Direction.DL => -1,
            Direction.R or Direction.UR or Direction.DR => 1,
            _ => 0
        };
    }

    private static int GetYmove(Direction direction)
    {
        return direction switch
        {
            Direction.U or Direction.UR or Direction.UL => -1,
            Direction.D or Direction.DR or Direction.DL => 1,
            _ => 0
        };
    }

    public Coordinates CopyBase(int newX, int newY) => new(X_Border + 1, Y_Border + 1, newX, newY);

    public Coordinates GetFromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.R => R,
            Direction.L => L,
            Direction.U => U,
            Direction.D => D,
            Direction.UR => UR,
            Direction.DR => DR,
            Direction.UL => UL,
            Direction.DL => DL,
            _ => throw new NotImplementedException(),
        };
    }

    public Coordinates GetFromDirectionWithDistance(Direction direction, int distance)
    {
        return direction switch
        {
            Direction.R => CopyBase(X + distance, Y),
            Direction.L => CopyBase(X - distance, Y),
            Direction.U => CopyBase(X, Y - distance),
            Direction.D => CopyBase(X, Y + distance),
            Direction.UR => CopyBase(X + distance, Y - distance),
            Direction.DR => CopyBase(X + distance, Y + distance),
            Direction.UL => CopyBase(X - distance, Y - distance),
            Direction.DL => CopyBase(X - distance, Y + distance),
            _ => throw new NotImplementedException(),
        };
    }

    private bool CanMove(Direction direction, Coordinates nextMove, Func<Coordinates, bool> stopClause = null)
    {
        return !CantMove(direction, nextMove, stopClause);
    }

    private bool CantMove(Direction direction, Coordinates nextMove, Func<Coordinates, bool> stopClause = null, bool allowOutside = false)
    {
        if (stopClause is not null && stopClause(nextMove))
        {
            return true;
        }

        return !allowOutside && direction switch
        {
            Direction.R => LastCol,
            Direction.L => FirstCol,
            Direction.U => FirstRow,
            Direction.D => LastRow,
            Direction.UR => FirstRow || LastCol,
            Direction.DR => LastRow || LastCol,
            Direction.UL => FirstRow || FirstCol,
            Direction.DL => LastRow || LastCol,
            _ => throw new NotImplementedException(),
        };
    }

    public List<Coordinates> GetAllNeighbours(Func<Coordinates, bool> condition = null)
    {
        var neighbours = new List<Coordinates>()
        {
            R,
            L,
            U,
            D,
            UR,
            DR,
            UL,
            DL,
        };

        return condition == null
            ? neighbours.Where(x => x.IsInsideOfBorder).ToList()
            : neighbours.Where(x => x.IsInsideOfBorder && condition(x)).ToList();
    }
}
