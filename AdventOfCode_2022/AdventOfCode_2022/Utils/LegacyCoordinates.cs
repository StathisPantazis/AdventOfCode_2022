namespace AdventOfCode_2022.Utils;

internal class LegacyCoordinates {
    public LegacyCoordinates() {
        X_Border = int.MaxValue;
        Y_Border = int.MaxValue;
    }

    public LegacyCoordinates(int gridWidth, int gridHeight, bool startFromNegative = false) {
        X_Border = gridWidth - 1;
        Y_Border = gridHeight - 1;
        X = startFromNegative ? -1 : 0;
    }

    public LegacyCoordinates(int gridWidth, int gridHeight, int startX, int startY) : this(gridWidth, gridHeight) {
        X = startX;
        Y = startY;
    }

    public LegacyCoordinates(ILegacyGrid grid, bool startFromNegative = false) {
        X_Border = grid.Height - 1;
        Y_Border = grid.Width - 1;
        X = startFromNegative ? -1 : 0;
    }

    public LegacyCoordinates(ILegacyGrid grid, int startX, int startY) : this(grid) {
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
    public LegacyCoordinates R => CopyBase(X, Y + 1);
    public LegacyCoordinates L => CopyBase(X, Y - 1);
    public LegacyCoordinates U => CopyBase(X - 1, Y);
    public LegacyCoordinates D => CopyBase(X + 1, Y);
    public LegacyCoordinates UR => CopyBase(X - 1, Y + 1);
    public LegacyCoordinates DR => CopyBase(X + 1, Y + 1);
    public LegacyCoordinates UL => CopyBase(X - 1, Y - 1);
    public LegacyCoordinates DL => CopyBase(X + 1, Y - 1);

    private bool FirstRow => X == 0;
    private bool LastRow => X == X_Border;
    private bool FirstCol => Y == 0;
    private bool LastCol => Y == Y_Border;

    public void GoToStart(bool startFromNegative = false) {
        X = startFromNegative ? -1 : 0;
        Y = 0;
    }

    public void GoToEnd() {
        X = X_Border;
        Y = Y_Border;
    }

    public bool TryGetNeighbour(Direction direction, out LegacyCoordinates neighbour) {
        neighbour = new(X_Border + 1, Y_Border + 1, X + GetXmove(direction), Y + GetYmove(direction));
        return CanMove(direction, neighbour);
    }

    public bool Move(Direction direction, Func<LegacyCoordinates, bool> stopClause = null) {
        LegacyCoordinates nextMove = GetFromDirection(direction);
        if (CantMove(direction, nextMove, stopClause)) {
            return false;
        }

        X += GetXmove(direction);
        Y += GetYmove(direction);
        return true;
    }

    public bool MoveTowards(LegacyCoordinates pos, bool allowOverlap = false, Func<LegacyCoordinates, bool> stopClause = null) {
        if (pos.Equals(this)) {
            return false;
        }

        Direction direction = pos switch {
            _ when pos.X < X && pos.Y < Y => Direction.UL,
            _ when pos.X < X && pos.Y > Y => Direction.UR,
            _ when pos.X < X => Direction.U,

            _ when pos.X > X && pos.Y < Y => Direction.DL,
            _ when pos.X > X && pos.Y > Y => Direction.DR,
            _ when pos.X > X => Direction.D,

            _ when pos.Y < Y => Direction.L,
            _ when pos.Y > Y => Direction.R,
        };

        int xMove = GetXmove(direction), yMove = GetYmove(direction);

        // overlap will take place on move
        if (!allowOverlap && xMove + X == pos.X && yMove + Y == pos.Y) {
            return false;
        }
        return Move(direction, stopClause: stopClause);
    }

    public bool MoveOpposite(LegacyCoordinates pos) {
        if (pos.Equals(this)) {
            return false;
        }

        Direction? direction = pos switch {
            _ when pos.X == X && pos.Y < Y => Direction.R,
            _ when pos.X == X && pos.Y > Y => Direction.L,
            _ when pos.Y == Y && pos.X < X => Direction.D,
            _ when pos.Y == Y && pos.X > X => Direction.U,

            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X < X && pos.Y < Y => Direction.DR,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X < X && pos.Y > Y => Direction.DL,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X > X && pos.Y < Y => Direction.UR,
            _ when Math.Abs(pos.Y - Y) == Math.Abs(pos.X - X) && pos.X > X && pos.Y > Y => Direction.UL,
            _ => null
        };

        return direction is not null && Move((Direction)direction);
    }

    public bool TraverseGrid() {
        if (X == -1) { // for start with negative case
            X = 0;
            return true;
        }
        else if (LastRow && LastCol) {
            return false;
        }

        X += LastCol ? 1 : 0;
        Y += LastCol ? -Y_Border : 1;
        StepsTraversed++;
        return true;
    }

    public bool TraverseGridReverse() {
        if (FirstRow && FirstCol) {
            return false;
        }

        X += FirstCol ? -1 : 0;
        Y += FirstCol ? Y_Border : -1;
        StepsTraversed++;
        return true;
    }

    public override string ToString() => $"{X} , {Y}";

    public bool Equals(LegacyCoordinates otherPos) => otherPos.X == X && Y == otherPos.Y;

    public bool NotEquals(LegacyCoordinates otherPos) => otherPos.X != X || Y != otherPos.Y;

    public LegacyCoordinates Copy() => new(X_Border + 1, Y_Border + 1, X, Y);

    private static int GetXmove(Direction direction) {
        return direction switch {
            Direction.U or Direction.UR or Direction.UL => -1,
            Direction.D or Direction.DR or Direction.DL => 1,
            _ => 0
        };
    }

    private static int GetYmove(Direction direction) {
        return direction switch {
            Direction.R or Direction.UR or Direction.DR => 1,
            Direction.L or Direction.UL or Direction.DL => -1,
            _ => 0
        };
    }

    public LegacyCoordinates CopyBase(int newX, int newY) => new(X_Border + 1, Y_Border + 1, newX, newY);

    private LegacyCoordinates GetFromDirection(Direction direction) {
        return direction switch {
            Direction.R => R,
            Direction.L => L,
            Direction.U => U,
            Direction.D => D,
            Direction.UR => UR,
            Direction.DR => DR,
            Direction.UL => UL,
            Direction.DL => DL,
        };
    }

    private bool CanMove(Direction direction, LegacyCoordinates nextMove, Func<LegacyCoordinates, bool> stopClause = null) {
        return !CantMove(direction, nextMove, stopClause);
    }

    private bool CantMove(Direction direction, LegacyCoordinates nextMove, Func<LegacyCoordinates, bool> stopClause = null) {
        if (stopClause is not null && stopClause(nextMove)) {
            return true;
        }

        return direction switch {
            Direction.R => LastCol,
            Direction.L => FirstCol,
            Direction.U => FirstRow,
            Direction.D => LastRow,
            Direction.UR => FirstRow || LastCol,
            Direction.DR => LastRow || LastCol,
            Direction.UL => FirstRow || FirstCol,
            Direction.DL => LastRow || LastCol,
        };
    }
}
