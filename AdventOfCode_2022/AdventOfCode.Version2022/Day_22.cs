using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_22 : AoCBaseDay<int, int, List<List<string>>>
{
    private string _password = string.Empty;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var inputParts = Helpers.FileCleanReadText(FileDescription(this, resourceType)).Split("\n\n");

        _password = inputParts[1];

        inputParts[0] = inputParts[0].Replace(" ", "O");
        var maxCols = inputParts[0].Split('\n').Max(x => x.Length);
        var inputRows = new List<string>();

        foreach (var l in inputParts[0].Split('\n'))
        {
            var line = l;
            if (l.Length < maxCols)
            {
                line += new string('O', maxCols - l.Length);
            }
            inputRows.Add(line);
        }

        var rows = inputRows.Select(x => x.Select(y => y.ToString()).ToList()).ToList();

        return new AoCSolution<int, int>(Part1(rows), Part2(rows));
    }

    protected override int Part1(List<List<string>> args)
    {
        return SharedSolution(1, args);
    }

    protected override int Part2(List<List<string>> args)
    {
        return SharedSolution(2, args);
    }

    private static bool MoveOver_Part1(IndexedGrid<string> grid, Coordinates pos, Direction dir)
    {
        int index;
        var next = pos.GetFromDirection(dir);
        var shouldTransfer = next.IsOutsideOfBorder || grid[next] == "O";

        if (!shouldTransfer)
        {
            return false;
        }

        switch (dir)
        {
            case Direction.L:
                index = grid.Row(pos.Y).FindLastIndex(x => x != "O");
                if (grid[index, pos.Y] == "#")
                {
                    return false;
                }
                pos.X = index;
                return true;
            case Direction.R:
                index = grid.Row(pos.Y).FindIndex(x => x != "O");
                if (grid[index, pos.Y] == "#")
                {
                    return false;
                }
                pos.X = index;
                return true;
            case Direction.U:
                index = grid.Column(pos.X).FindLastIndex(x => x != "O");
                if (grid[pos.X, index] == "#")
                {
                    return false;
                }
                pos.Y = index;
                return true;
            case Direction.D:
                var column = grid.Column(pos.X);
                index = grid.Column(pos.X).FindIndex(x => x != "O");
                if (grid[pos.X, index] == "#")
                {
                    return false;
                }
                pos.Y = index;
                return true;
        }
        return false;
    }

    private static bool MoveOver_Part2(IndexedGrid<string> grid, Coordinates pos, Direction dir, List<Plane> planes, out Direction walkTo)
    {
        walkTo = dir;
        var next = pos.GetFromDirection(dir);
        var shouldTransfer = next.IsOutsideOfBorder || grid[next] == "O";

        if (!shouldTransfer)
        {
            return false;
        }

        var fromPlane = planes.First(x => x.ContainsPosition(pos));
        var bridge = fromPlane.Bridges.First(x => x.FromSide == dir);
        var toPlane = planes.First(x => x.Type == bridge.To);

        var fromPlanePos = fromPlane.ToPlaneCoordinates(pos);
        var toPlanePos = bridge.TransformCoordinates(fromPlanePos);
        var toGridPos = toPlane.ToGridCoordinates(toPlanePos);

        if (grid[toGridPos] == "#")
        {
            return false;
        }

        walkTo = bridge.Strategy.EnterDirection;
        pos.X = toGridPos.X;
        pos.Y = toGridPos.Y;
        return true;
    }

    private int SharedSolution(int part, List<List<string>> inputRows)
    {
        var grid = new IndexedGrid<string>(inputRows, emptyValue: "O");
        var pos = grid.GetCoordinates(grid.Row(0).FindIndex(x => x == "."), 0);

        var planes = part == 1 ? null : IdentifyPlanes(grid, 4);

        var map = new InstructionReader(_password);
        var walkTo = Direction.R;
        bool shouldStop(Coordinates next) => next.IsInsideOfBorder && grid[next] is "#" or "O";

        while (map.HasMoreInstructions)
        {
            (var steps, var turnTo) = map.NextMove();

            for (var i = 0; i < steps; i++)
            {
                if (part == 1 && !MoveOver_Part1(grid, pos, walkTo) || part == 2 && !MoveOver_Part2(grid, pos, walkTo, planes, out walkTo))
                {
                    if (!pos.Move(walkTo, shouldStop))
                    {
                        break;
                    }
                }
                else if (part == 2)
                {
                    map.LastDirection = walkTo;
                }

                grid[pos] = InstructionReader.Step(walkTo);

                Console.Clear();
                Console.WriteLine(grid);
            };

            if (map.HasMoreInstructions)
            {
                walkTo = turnTo;
            }
        }

        Console.Clear();
        Console.WriteLine(grid);
        var dirValue = walkTo switch
        {
            Direction.R => 0,
            Direction.D => 1,
            Direction.L => 2,
            Direction.U => 3,
        };

        var value = (1000 * (pos.Y + 1)) + (4 * (pos.X + 1)) + dirValue;
        return value;
    }

    private static List<Plane> IdentifyPlanes(IndexedGrid<string> grid, int size)
    {
        static void setBridges(Plane plane1, Plane plane2, Strategy plane1To2Strategy, Strategy plane2To1Strategy, Direction plane1Side, Direction plane2Side)
        {
            plane1.Bridge(plane2.Type).SetSides(plane1To2Strategy, plane1Side, plane2Side);
            plane2.Bridge(plane1.Type).SetSides(plane2To1Strategy, plane2Side, plane1Side);
        }

        bool predicateBool(string x) => x != "O";
        bool predicateStr(string x) => x != "O";
        var fullMove = size - 1;

        var topPoints = grid.Row(0).Count(predicateBool);
        var midPoints = grid.Row(size).Count(predicateBool);
        var botPoints = grid.Row(grid.Rows.Count - 1).Count(predicateBool);

        var topFirstIndex = grid.Row(0).FindIndex(predicateStr);
        var topLastIndex = topFirstIndex + size - 1;

        Plane up = new(size, PlaneType.Up, topFirstIndex, topLastIndex, 0, size - 1);
        Plane bottom = new(size, PlaneType.Bottom, topFirstIndex, topLastIndex, size, (2 * size) - 1);
        setBridges(bottom, up, new Strategy(Direction.U, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsX), Direction.U, Direction.D);

        Plane down = new(size, PlaneType.Down, topFirstIndex, topLastIndex, (size * 2), (3 * size) - 1);
        setBridges(bottom, down, new Strategy(Direction.D, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsX), Direction.D, Direction.U);

        Plane left = null;
        Plane right = null;
        Plane top = null;

        if (topPoints == (size * 2))
        {
            var sliceRight = grid.RowSliceRight(topLastIndex, 0);

            if (sliceRight.Any(predicateBool))
            {
                right = new Plane(size, PlaneType.Right, topLastIndex + 1, topLastIndex + size, 0, size - 1);
            }
        }

        if (midPoints == (size * 3))
        {
            var sliceLeft = grid.RowSliceLeft(topFirstIndex, size);
            if (sliceLeft.Any(predicateBool))
            {
                left = new Plane(size, PlaneType.Left, topFirstIndex - size, topFirstIndex - 1, bottom.UpBorder, bottom.DownBorder);
                setBridges(left, bottom, new Strategy(Direction.R, TransposeAction.SameAsY), new Strategy(Direction.L, TransposeAction.SameAsY), Direction.R, Direction.L);
                setBridges(left, up, new Strategy(Direction.R, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsY), Direction.U, Direction.L);
                setBridges(left, down, new Strategy(Direction.R, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsY), Direction.D, Direction.L);

                if (sliceLeft.Count(predicateBool) > size)
                {
                    top = new Plane(size, PlaneType.Top, topFirstIndex - (2 * size), topFirstIndex - size - 1, bottom.UpBorder, bottom.DownBorder);
                    setBridges(top, left, new Strategy(Direction.R, TransposeAction.SameAsY), new Strategy(Direction.L, TransposeAction.SameAsY), Direction.R, Direction.L);
                    setBridges(top, up, new Strategy(Direction.D, TransposeAction.SameAsX), new Strategy(Direction.D, TransposeAction.SameAsX), Direction.U, Direction.D);
                    setBridges(top, down, new Strategy(Direction.U, TransposeAction.SameAsX), new Strategy(Direction.U, TransposeAction.SameAsX), Direction.D, Direction.U);
                }
            }
        }

        if (botPoints == (size * 2))
        {
            var sliceRight = grid.RowSliceRight(topLastIndex, size * 2);

            if (sliceRight.Any(predicateBool))
            {
                right = new Plane(size, PlaneType.Right, topLastIndex + 1, topLastIndex + size, down.UpBorder, down.DownBorder);
                setBridges(right, down, new Strategy(Direction.L, TransposeAction.SameAsY), new Strategy(Direction.R, TransposeAction.SameAsY), Direction.L, Direction.R);
                setBridges(right, bottom, new Strategy(Direction.L, TransposeAction.FlipX), new Strategy(Direction.D, TransposeAction.FlipY), Direction.U, Direction.R);
                setBridges(right, top, new Strategy(Direction.R, TransposeAction.FlipX), new Strategy(Direction.L, TransposeAction.FlipY), Direction.D, Direction.L);
                setBridges(right, up, new Strategy(Direction.L, TransposeAction.FlipX), new Strategy(Direction.L, TransposeAction.FlipY), Direction.R, Direction.R);
            }
        }

        var planes = new List<Plane> { up, bottom, down, left, right, top };

        foreach (var plane in planes)
        {
            foreach (var bridge in new List<Bridge>(plane.Bridges))
            {
                if (!bridge.IsSet)
                {
                    var bridgeToRemove = plane.Bridges.First(x => x.To == bridge.To);
                    plane.Bridges.Remove(bridgeToRemove);
                }
            }
        }

        return planes;
    }

    private class InstructionReader(string pwd)
    {
        public string Initial { get; set; } = pwd;

        public string Ongoing { get; set; } = pwd;

        public bool HasMoreInstructions => Ongoing.Length > 0;

        public Direction LastDirection { get; set; } = Direction.R;

        public static string Step(Direction dir) => dir is Direction.U ? "^" : dir is Direction.R ? ">" : dir is Direction.D ? "v" : "<";

        public (int steps, Direction dir) NextMove()
        {
            var turn = Direction.R;
            var stepsString = "";
            var stepsRemain = false;

            for (var i = 0; i < Ongoing.Length; i++)
            {
                if (char.IsDigit(Ongoing[i]))
                {
                    stepsString += Ongoing[i];
                }
                else
                {
                    turn = Ongoing[i] == 'R' ? Direction.R : Direction.L;
                    Ongoing = Ongoing[(i + 1)..];
                    stepsRemain = true;
                    break;
                }
            }

            if (!stepsRemain)
            {
                Ongoing = string.Empty;
            }

            turn = turn switch
            {
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

    private class Plane
    {
        public Plane(int size, PlaneType type, int leftBorder, int rightBorder, int upBorder, int downBorder)
        {
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

        public Coordinates ToPlaneCoordinates(Coordinates pos) => pos.Copy(pos.X - LeftBorder, pos.Y - UpBorder);

        public Coordinates ToGridCoordinates(Coordinates pos) => pos.Copy(pos.X + LeftBorder, pos.Y + UpBorder);

        public override string ToString() =>
            $"{Type} --> L: {LeftBorder}  - R: {RightBorder}  - U: {UpBorder}  - D: {DownBorder}";
    }

    private class Bridge(PlaneType from, PlaneType to, int size)
    {
        public bool IsSet { get; set; }
        public int Size { get; set; } = size;
        public int MaxBorder => Size - 1;
        public PlaneType From { get; set; } = from;
        public PlaneType To { get; set; } = to;
        public Direction FromSide { get; set; }
        public Direction ToSide { get; set; }
        public Strategy Strategy { get; set; }

        public void SetSides(Strategy strategy, Direction fromSide, Direction toSide)
        {
            IsSet = true;
            Strategy = strategy;
            FromSide = fromSide;
            ToSide = toSide;
        }

        public Coordinates TransformCoordinates(Coordinates from)
        {
            var to = from.Copy();
            Strategy.Transpose(to, from, MaxBorder);
            return to;
        }

        public override string ToString() => $"{From} <--> {To}  :||: {Strategy}";
    }

    private class Strategy(Direction enterDirection, TransposeAction transposeAction)
    {
        public TransposeAction TransposeAction { get; set; } = transposeAction;
        public Direction EnterDirection { get; set; } = enterDirection;

        public void Transpose(Coordinates pos, Coordinates from, int maxBorder)
        {
            if (EnterDirection is Direction.L or Direction.R)
            {
                pos.X = EnterDirection switch
                {
                    Direction.L => maxBorder,
                    Direction.R => 0,
                    _ => pos.X
                };

                pos.Y = TransposeAction switch
                {
                    TransposeAction.SameAsX => from.X,
                    TransposeAction.SameAsY => from.Y,
                    TransposeAction.FlipX => maxBorder - from.X,
                    TransposeAction.FlipY => maxBorder - from.Y,
                };
            }

            if (EnterDirection is Direction.U or Direction.D)
            {
                pos.Y = EnterDirection switch
                {
                    Direction.U => maxBorder,
                    Direction.D => 0,
                    _ => pos.Y
                };

                pos.X = TransposeAction switch
                {
                    TransposeAction.SameAsX => from.X,
                    TransposeAction.SameAsY => from.Y,
                    TransposeAction.FlipX => maxBorder - from.X,
                    TransposeAction.FlipY => maxBorder - from.Y,
                };
            }
        }

        public override string ToString() => $"Enter from: {EnterDirection}";
    }

    private enum PlaneType { Bottom, Up, Right, Left, Down, Top }

    private enum TransposeAction { SameAsX, SameAsY, FlipX, FlipY };
}
