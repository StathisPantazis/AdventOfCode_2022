namespace AdventOfCode.Core.Models.Bases;

public abstract class Coordinates
{
    internal Coordinates(IGrid grid, bool startFromNegative = false)
    {
        X_Border = grid.Width - 1;
        Y_Border = grid.Height - 1;
        X = startFromNegative ? -1 : 0;
    }

    internal Coordinates(IGrid grid, int startX, int startY) : this(grid)
    {
        X = startX;
        Y = startY;
    }

    protected Coordinates(int gridWidth, int gridHeight, bool startFromNegative = false)
    {
        X_Border = gridWidth - 1;
        Y_Border = gridHeight - 1;
        X = startFromNegative ? -1 : 0;
    }

    protected Coordinates(int gridWidth, int gridHeight, int startX, int startY) : this(gridWidth, gridHeight)
    {
        X = startX;
        Y = startY;
    }

    public int X { get; set; }
    public int Y { get; set; }
    protected int X_Border { get; set; }
    protected int Y_Border { get; set; }
    protected abstract bool BottomestRow { get; }
    protected abstract bool TopestRow { get; }

    public bool IsOnBorder => BottomestRow || LeftestCol || TopestRow || RightestCol;
    public bool IsOutsideOfBorder => X < 0 || Y < 0 || X > X_Border || Y > Y_Border;
    public bool IsInsideOfBorder => !IsOutsideOfBorder;
    public bool IsAtStart => BottomestRow && LeftestCol;
    public bool IsAtEnd => TopestRow && RightestCol;
    protected bool LeftestCol => X == 0;
    protected bool RightestCol => X == X_Border;

    public Coordinates R => X + 1 <= X_Border ? Copy(X + 1, Y) : null;
    public Coordinates L => X - 1 >= 0 ? Copy(X - 1, Y) : null;
    public abstract Coordinates U { get; }
    public abstract Coordinates D { get; }
    public abstract Coordinates UR { get; }
    public abstract Coordinates DR { get; }
    public abstract Coordinates UL { get; }
    public abstract Coordinates DL { get; }

    public abstract bool TraverseGrid();
    public abstract bool ReverseTraverseGrid();
    public abstract Coordinates Copy();
    public abstract Coordinates Copy(int newX, int newY);
    public abstract Coordinates GetFromDirectionWithDistance(Direction direction, int distance);
    protected abstract Direction GetDirectionTowards(Coordinates dest);
    protected abstract Direction GetDirectionOpposite(Coordinates awayFrom);

    public void GoToStart(bool startFromOutside = false)
    {
        X = startFromOutside ? -1 : 0;
        Y = 0;
    }

    public void GoToEnd(bool startFromOutside = false)
    {
        X = startFromOutside ? X_Border + 1 : X_Border;
        Y = Y_Border;
    }

    public bool TryGetNeighbour(Direction direction, out Coordinates neighbour)
    {
        neighbour = null;

        if (CanMove(direction))
        {
            neighbour = GetFromDirection(direction);
            return true;
        }

        return false;
    }

    public bool Move(Direction direction, Func<Coordinates, bool> shouldStop = null)
    {
        if (CantMove(direction))
        {
            return false;
        }

        var nextMove = GetFromDirection(direction);

        if (shouldStop != null && shouldStop(nextMove))
        {
            return false;
        }

        X += GetXmove(direction);
        Y += GetYmove(direction);
        return true;
    }

    public bool MoveTowards(Coordinates dest, bool allowOverlap = false, Func<Coordinates, bool> stopClause = null)
    {
        if (dest.Equals(this))
        {
            return false;
        }

        var direction = GetDirectionTowards(dest);
        stopClause = stopClause is null ? ((p) => !allowOverlap && p.Equals(dest)) : ((p) => stopClause(p) && !allowOverlap && p.Equals(dest));

        return Move(direction, stopClause);
    }

    public bool MoveOpposite(Coordinates awayFrom, Func<Coordinates, bool> stopClause = null)
    {
        if (awayFrom.Equals(this))
        {
            return false;
        }

        var direction = GetDirectionOpposite(awayFrom);
        return Move(direction, stopClause);
    }

    public bool Equals(Coordinates otherPos) => otherPos.X == X && Y == otherPos.Y;

    public bool NotEquals(Coordinates otherPos) => otherPos.X != X || Y != otherPos.Y;

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

    public List<Coordinates> GetFromDirections(List<Direction> directions)
    {
        var coords = new List<Coordinates>();

        foreach (var dir in directions)
        {
            coords.Add(GetFromDirection(dir));
        }

        return coords;
    }

    public List<Coordinates> GetAllNeighbours(bool allowDiagonal = true, Func<Coordinates, bool> filterCondition = null)
    {
        var isOnBorder = IsOnBorder;

        var neighbours = new List<Coordinates>();

        if (CanMove(Direction.R))
        {
            neighbours.Add(R);
        }
        if (CanMove(Direction.L))
        {
            neighbours.Add(L);
        }
        if (CanMove(Direction.U))
        {
            neighbours.Add(U);
        }
        if (CanMove(Direction.D))
        {
            neighbours.Add(D);
        }
        if (CanMove(Direction.UR) && allowDiagonal)
        {
            neighbours.Add(UR);
        }
        if (CanMove(Direction.DR) && allowDiagonal)
        {
            neighbours.Add(DR);
        }
        if (CanMove(Direction.UL) && allowDiagonal)
        {
            neighbours.Add(UL);
        }
        if (CanMove(Direction.DL) && allowDiagonal)
        {
            neighbours.Add(DL);
        }

        return filterCondition == null
            ? neighbours.ToList()
            : neighbours.Where(x => filterCondition(x)).ToList();
    }

    protected static int GetXmove(Direction direction)
    {
        return direction switch
        {
            Direction.L or Direction.UL or Direction.DL => -1,
            Direction.R or Direction.UR or Direction.DR => 1,
            _ => 0
        };
    }

    protected abstract int GetYmove(Direction direction);

    private bool CanMove(Direction direction) => !CantMove(direction);

    private bool CantMove(Direction direction)
    {
        return direction switch
        {
            Direction.R => RightestCol,
            Direction.L => LeftestCol,
            Direction.U => TopestRow,
            Direction.D => BottomestRow,
            Direction.UR => TopestRow || RightestCol,
            Direction.DR => BottomestRow || RightestCol,
            Direction.UL => TopestRow || LeftestCol,
            Direction.DL => BottomestRow || LeftestCol,
            _ => throw new NotImplementedException(),
        };
    }

    public override string ToString() => $"{X} , {Y}";

    public override bool Equals(object? obj) => obj is Coordinates coord && coord.X == X && coord.Y == Y;

    public override int GetHashCode() => $"{X}{Y}".GetHashCode();
}
