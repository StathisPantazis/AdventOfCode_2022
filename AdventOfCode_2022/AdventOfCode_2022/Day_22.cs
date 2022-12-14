using AdventOfCode_2022.Models;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_22 {
    public static void Solve() {
        string[] inputParts = Helpers.File_CleanReadText(22).Split("\n\n");
        string password = inputParts[1];

        inputParts[0] = inputParts[0].Replace(" ", "O");
        int maxCols = inputParts[0].Split('\n').Max(x => x.Length);
        List<string> inputRows = new();

        foreach (string l in inputParts[0].Split('\n')) {
            string line = l;
            if (l.Length < maxCols) {
                line += new string('O', maxCols - l.Length);
            }
            inputRows.Add(line);
        }

        for (int part = 1; part < 3; part++) {
            if (part == 1) {
                continue;
            }
            Grid<string> grid = new(inputRows, singleCharacters: true, emptyValue: "O");
            Coordinates pos = new(grid, grid.Row(0).FindIndex(x => x == "."), 0);

            List<Plane> planes = part == 1 ? null : IdentifyPlanes(grid, 4);

            InstructionReader map = new(password);
            Direction walkTo = Direction.R;
            bool stopClause(Coordinates next) => next.IsInsideOfBorder && grid[next] is "#" or "O";

            while (map.HasMoreInstructions) {
                (int steps, Direction turnTo) = map.NextMove();
                var la = "";

                for (int i = 0; i < steps; i++) {
                    if ((part == 1 && !MoveOver_Part1(grid, pos, walkTo)) || (part == 2 && !MoveOver_Part2(grid, pos, walkTo, planes, out walkTo))) {
                        if (!pos.Move(walkTo, stopClause)) {
                            break;
                        }
                    }
                    else if (part == 2) {
                        map.LastDirection = walkTo;
                    }

                    grid[pos] = map.Step(walkTo);

                    Console.Clear();
                    Console.WriteLine(grid);
                    var lala = "";
                };

                if (map.HasMoreInstructions) {
                    walkTo = turnTo;
                }
            }

            Console.Clear();
            Console.WriteLine(grid);
            int dirValue = walkTo switch {
                Direction.R => 0,
                Direction.D => 1,
                Direction.L => 2,
                Direction.U => 3,
            };
            int value = (1000 * (pos.Y + 1)) + (4 * (pos.X + 1)) + dirValue;
            Console.WriteLine(value);
        }
    }

    public static bool MoveOver_Part2(Grid<string> grid, Coordinates pos, Direction dir, List<Plane> planes, out Direction walkTo) {
        walkTo = dir;
        Coordinates next = pos.GetFromDirection(dir);
        bool shouldTransfer = next.IsOutsideOfBorder || grid[next] == "O";

        if (!shouldTransfer) {
            return false;
        }

        Plane fromPlane = planes.First(x => x.ContainsPosition(pos));
        Bridge bridge = fromPlane.Bridges.First(x => x.FromSide == dir);
        Plane toPlane = planes.First(x => x.Type == bridge.To);

        Coordinates fromPlanePos = fromPlane.ToPlaneCoordinates(pos);
        Coordinates toPlanePos = bridge.TransformCoordinates(fromPlanePos);
        Coordinates toGridPos = toPlane.ToGridCoordinates(toPlanePos);

        if (grid[toGridPos] == "#") {
            return false;
        }

        walkTo = bridge.Strategy.EnterDirection;
        pos.X = toGridPos.X;
        pos.Y = toGridPos.Y;
        return true;
    }

    public static bool MoveOver_Part1(Grid<string> grid, Coordinates pos, Direction dir) {
        int index;
        Coordinates next = pos.GetFromDirection(dir);
        bool shouldTransfer = next.IsOutsideOfBorder || grid[next] == "O";

        if (!shouldTransfer) {
            return false;
        }

        switch (dir) {
            case Direction.L:
                index = grid.Row(pos.Y).FindLastIndex(x => x != "O");
                if (grid[index, pos.Y] == "#") {
                    return false;
                }
                pos.X = index;
                return true;
            case Direction.R:
                index = grid.Row(pos.Y).FindIndex(x => x != "O");
                if (grid[index, pos.Y] == "#") {
                    return false;
                }
                pos.X = index;
                return true;
            case Direction.U:
                index = grid.Column(pos.X).FindLastIndex(x => x != "O");
                if (grid[pos.X, index] == "#") {
                    return false;
                }
                pos.Y = index;
                return true;
            case Direction.D:
                List<string> column = grid.Column(pos.X);
                index = grid.Column(pos.X).FindIndex(x => x != "O");
                if (grid[pos.X, index] == "#") {
                    return false;
                }
                pos.Y = index;
                return true;
        }
        return false;
    }

    public static List<Plane> IdentifyPlanes(Grid<string> grid, int size) {
        static void setBridges(Plane plane1, Plane plane2, Strategy plane1To2Strategy, Strategy plane2To1Strategy, Direction plane1Side, Direction plane2Side) {
            plane1.Bridge(plane2.Type).SetSides(plane1To2Strategy, plane1Side, plane2Side);
            plane2.Bridge(plane1.Type).SetSides(plane2To1Strategy, plane2Side, plane1Side);
        }

        Func<string, bool> predicateBool = (string x) => x != "O";
        Predicate<string> predicateStr = (string x) => x != "O";
        int fullMove = size - 1;

        int topPoints = grid.Row(0).Count(predicateBool);
        int midPoints = grid.Row(size).Count(predicateBool);
        int botPoints = grid.Row(grid.Rows.Count - 1).Count(predicateBool);

        int topFirstIndex = grid.Row(0).FindIndex(predicateStr);
        int topLastIndex = topFirstIndex + size - 1;

        Plane up = new(size, PlaneType.Up, topFirstIndex, topLastIndex, 0, size - 1);
        Plane bottom = new(size, PlaneType.Bottom, topFirstIndex, topLastIndex, size, (2 * size) - 1);
        setBridges(bottom, up, new Strategy(Direction.U, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsX), Direction.U, Direction.D);

        Plane down = new(size, PlaneType.Down, topFirstIndex, topLastIndex, size * 2, (3 * size) - 1);
        setBridges(bottom, down, new Strategy(Direction.D, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsX), Direction.D, Direction.U);

        Plane left = null;
        Plane right = null;
        Plane top = null;

        if (topPoints == size * 2) {
            List<string> sliceRight = grid.RowSliceRight(topLastIndex, 0);

            if (sliceRight.Any(predicateBool)) {
                right = new Plane(size, PlaneType.Right, topLastIndex + 1, topLastIndex + size, 0, size - 1);
            }
        }

        if (midPoints == size * 3) {
            List<string> sliceLeft = grid.RowSliceLeft(topFirstIndex, size);
            if (sliceLeft.Any(predicateBool)) {
                left = new Plane(size, PlaneType.Left, topFirstIndex - size, topFirstIndex - 1, bottom.UpBorder, bottom.DownBorder);
                setBridges(left, bottom, new Strategy(Direction.R, TransposeAction.SameAsY), new Strategy(Direction.L, TransposeAction.SameAsY), Direction.R, Direction.L);
                setBridges(left, up, new Strategy(Direction.R, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsY), Direction.U, Direction.L);
                setBridges(left, down, new Strategy(Direction.R, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsY), Direction.D, Direction.L);

                if (sliceLeft.Count(predicateBool) > size) {
                    top = new Plane(size, PlaneType.Top, topFirstIndex - (2 * size), topFirstIndex - size - 1, bottom.UpBorder, bottom.DownBorder);
                    setBridges(top, left, new Strategy(Direction.R, TransposeAction.SameAsY), new Strategy(Direction.L, TransposeAction.SameAsY), Direction.R, Direction.L);
                    setBridges(top, up, new Strategy(Direction.D, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsX), Direction.U, Direction.D);
                    setBridges(top, down, new Strategy(Direction.U, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsX), Direction.D, Direction.U);
                }
            }
        }

        if (botPoints == size * 2) {
            List<string> sliceRight = grid.RowSliceRight(topLastIndex, size * 2);

            if (sliceRight.Any(predicateBool)) {
                right = new Plane(size, PlaneType.Right, topLastIndex + 1, topLastIndex + size, down.UpBorder, down.DownBorder);
                setBridges(right, down, new Strategy(Direction.L, TransposeAction.SameAsY), new Strategy(Direction.R, TransposeAction.SameAsY), Direction.L, Direction.R);
                setBridges(right, bottom, new Strategy(Direction.L, TransposeAction.FlipX), new Strategy(Direction.D, TransposeAction.FlipY), Direction.U, Direction.R);
                setBridges(right, top, new Strategy(Direction.R, TransposeAction.FlipX), new Strategy(Direction.L, TransposeAction.FlipY), Direction.D, Direction.L);
                setBridges(right, up, new Strategy(Direction.L, TransposeAction.FlipX), new Strategy(Direction.L, TransposeAction.FlipY), Direction.R, Direction.R);
            }
        }

        List<Plane> planes = new() { up, bottom, down, left, right, top };

        foreach (Plane plane in planes) {
            foreach (Bridge bridge in new List<Bridge>(plane.Bridges)) {
                if (!bridge.IsSet) {
                    Bridge bridgeToRemove = plane.Bridges.First(x => x.To == bridge.To);
                    plane.Bridges.Remove(bridgeToRemove);
                }
            }
        }

        return planes;
    }

    public class InstructionReader {
        public InstructionReader(string pwd) {
            Initial = pwd;
            Ongoing = pwd;
        }

        public string Initial { get; set; }

        public string Ongoing { get; set; }

        public bool HasMoreInstructions => Ongoing.Length > 0;

        public Direction LastDirection { get; set; } = Direction.R;

        public string Step(Direction dir) => dir is Direction.U ? "^" : dir is Direction.R ? ">" : dir is Direction.D ? "v" : "<";

        public (int steps, Direction dir) NextMove() {
            Direction turn = Direction.R;
            string stepsString = "";
            bool stepsRemain = false;

            for (int i = 0; i < Ongoing.Length; i++) {
                if (char.IsDigit(Ongoing[i])) {
                    stepsString += Ongoing[i];
                }
                else {
                    turn = Ongoing[i] == 'R' ? Direction.R : Direction.L;
                    Ongoing = Ongoing[(i + 1)..];
                    stepsRemain = true;
                    break;
                }
            }

            if (!stepsRemain) {
                Ongoing = string.Empty;
            }

            turn = turn switch {
                Direction.R when LastDirection is Direction.R => Direction.D,
                Direction.R when LastDirection is Direction.D => Direction.L,
                Direction.R when LastDirection is Direction.L => Direction.U,
                Direction.R when LastDirection is Direction.U => Direction.R,
                Direction.L when LastDirection is Direction.R => Direction.U,
                Direction.L when LastDirection is Direction.U => Direction.L,
                Direction.L when LastDirection is Direction.L => Direction.D,
                Direction.L when LastDirection is Direction.D => Direction.R,
            };

            LastDirection = turn;

            return (int.Parse(stepsString), turn);
        }
    }

    public class Plane {
        public Plane(int size, PlaneType type, int leftBorder, int rightBorder, int upBorder, int downBorder) {
            Size = size;
            Type = type;
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            UpBorder = upBorder;
            DownBorder = downBorder;

            Enum.GetValues<PlaneType>().ToList().ForEach(x => Bridges.Add(new Bridge(type, x, size)));
        }

        public int Size { get; set; }
        public PlaneType Type { get; set; }
        public int LeftBorder { get; set; }
        public int RightBorder { get; set; }
        public int UpBorder { get; set; }
        public int DownBorder { get; set; }
        public List<Bridge> Bridges { get; set; } = new();

        public Bridge Bridge(PlaneType to) => Bridges.First(x => x.To == to);

        public bool ContainsPosition(Coordinates pos) => pos.X >= LeftBorder && pos.X <= RightBorder && pos.Y >= UpBorder && pos.Y <= DownBorder;

        public Coordinates ToPlaneCoordinates(Coordinates pos) => pos.CopyBase(pos.X - LeftBorder, pos.Y - UpBorder);

        public Coordinates ToGridCoordinates(Coordinates pos) => pos.CopyBase(pos.X + LeftBorder, pos.Y + UpBorder);

        public override string ToString() =>
            $"{Type} --> L: {LeftBorder}  - R: {RightBorder}  - U: {UpBorder}  - D: {DownBorder}";
    }

    public class Bridge {
        public Bridge(PlaneType from, PlaneType to, int size) {
            From = from;
            To = to;
            Size = size;
        }

        public bool IsSet { get; set; }
        public int Size { get; set; }
        public int MaxBorder => Size - 1;
        public PlaneType From { get; set; }
        public PlaneType To { get; set; }
        public Direction FromSide { get; set; }
        public Direction ToSide { get; set; }
        public Strategy Strategy { get; set; }

        public void SetSides(Strategy strategy, Direction fromSide, Direction toSide) {
            IsSet = true;
            Strategy = strategy;
            FromSide = fromSide;
            ToSide = toSide;
        }

        public Coordinates TransformCoordinates(Coordinates from) {
            Coordinates to = from.Copy();
            Strategy.Transpose(to, from, MaxBorder);
            return to;
        }

        public override string ToString() => $"{From} <--> {To}  :||: {Strategy}";
    }

    public class Strategy {
        public Strategy(Direction enterDirection, TransposeAction transposeAction) {
            EnterDirection = enterDirection;
            TransposeAction = transposeAction;
        }

        public TransposeAction TransposeAction { get; set; }
        public Direction EnterDirection { get; set; }

        public void Transpose(Coordinates pos, Coordinates from, int maxBorder) {
            if (EnterDirection is Direction.L or Direction.R) {
                pos.X = EnterDirection switch {
                    Direction.L => maxBorder,
                    Direction.R => 0,
                    _ => pos.X
                };

                pos.Y = TransposeAction switch {
                    TransposeAction.SameAsX => from.X,
                    TransposeAction.SameAsY => from.Y,
                    TransposeAction.FlipX => maxBorder - from.X,
                    TransposeAction.FlipY => maxBorder - from.Y,
                };
            }

            if (EnterDirection is Direction.U or Direction.D) {
                pos.Y = EnterDirection switch {
                    Direction.U => maxBorder,
                    Direction.D => 0,
                    _ => pos.Y
                };

                pos.X = TransposeAction switch {
                    TransposeAction.SameAsX => from.X,
                    TransposeAction.SameAsY => from.Y,
                    TransposeAction.FlipX => maxBorder - from.X,
                    TransposeAction.FlipY => maxBorder - from.Y,
                };
            }
        }

        public override string ToString() => $"Enter from: {EnterDirection}";
    }

    public enum PlaneType { Bottom, Up, Right, Left, Down, Top }

    public enum TransposeAction { SameAsX, SameAsY, FlipX, FlipY };
}
