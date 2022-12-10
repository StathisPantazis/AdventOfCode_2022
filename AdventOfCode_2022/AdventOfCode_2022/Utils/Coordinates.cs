namespace AdventOfCode_2022.Utils;

internal class Coordinates {
    public Coordinates() {
        X_Border = int.MaxValue;
        Y_Border = int.MaxValue;
    }

    public Coordinates(int gridWidth, int gridHeight, bool startFromNegative = false) {
        X_Border = gridWidth - 1;
        Y_Border = gridHeight - 1;
        X = startFromNegative ? -1 : 0;
    }

    public Coordinates(int gridWidth, int gridHeight, int startX, int startY) : this(gridWidth, gridHeight) {
        X = startX;
        Y = startY;
    }

    public Coordinates(IGrid grid, bool startFromNegative = false) {
        X_Border = grid.MaxRows - 1;
        Y_Border = grid.MaxColumns - 1;
        X = startFromNegative ? -1 : 0;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int X_Border { get; }
    public int Y_Border { get; }
    public bool IsOnBorder => FirstRow || FirstCol || LastRow || LastCol;
    public bool IsAtStart => FirstRow && FirstCol;
    public bool IsAtEnd => LastRow && LastCol;

    private bool FirstRow => X == 0;
    private bool LastRow => X == X_Border;
    private bool FirstCol => Y == 0;
    private bool LastCol => Y == Y_Border;

    public void GoToEnd() {
        X = X_Border;
        Y = Y_Border;
    }

    public bool Move(Direction direction) {
        bool cantMove = direction switch {
            Direction.R => LastCol,
            Direction.L => FirstCol,
            Direction.U => FirstRow,
            Direction.D => LastRow,
            Direction.UR => FirstRow || LastCol,
            Direction.DR => LastRow || LastCol,
            Direction.UL => FirstRow || FirstCol,
            Direction.DL => LastRow || LastCol,
        };

        if (cantMove) {
            return false;
        }

        X += GetXmove(direction);
        Y += GetYmove(direction);

        return true;
    }

    public bool MoveTowards(Coordinates pos, bool allowOverlap = false) {
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
        return Move(direction);
    }

    public bool MoveOpposite(Coordinates pos) {
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
        return true;
    }

    public bool TraverseGridReverse() {
        if (FirstRow && FirstCol) {
            return false;
        }

        X += FirstCol ? -1 : 0;
        Y += FirstCol ? Y_Border : -1;
        return true;
    }

    public override string ToString() => $"{X} , {Y}";

    public bool Equals(Coordinates otherPos) => otherPos.X == X && Y == otherPos.Y;

    public Coordinates Copy() => new(X_Border + 1, Y_Border + 1, X, Y);

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
}
