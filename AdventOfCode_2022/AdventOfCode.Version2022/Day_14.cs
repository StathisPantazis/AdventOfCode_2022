using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using AdventOfCode.Version2022.Models;

namespace AdventOfCode.Version2022;

public class Day_14 : AoCBaseDay<int, int, List<(int x, int y)>>
{
    private static readonly string[] _blocks = new string[] { "#", "O" };
    private AoCResourceType _resourceType;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        _resourceType = resourceType;

        List<(int x, int y)> coordPairs = Helpers.File_CleanReadText(FileDescription(this, resourceType))
            .Replace("\n", "_")
            .Replace(" -> ", "_")
            .Split('_')
            .Select(x => (int.Parse(x.Split(',')[1]), int.Parse(x.Split(',')[0])))
            .ToList();

        return Solution(coordPairs);
    }

    protected override int Part1(List<(int x, int y)> coordPairs)
    {
        var cave = BuildCave(coordPairs, _resourceType, out var pour);

        while (Fall(cave, pour, false) is LegacyCoordinates sand)
        {
            cave[sand] = "O";
        }

        return cave.ToString().Count(x => x == 'O');
    }

    protected override int Part2(List<(int x, int y)> coordPairs)
    {
        var cave = BuildCave(coordPairs, _resourceType, out var pour);
        cave.AddRow(".", rebuildColumns: false);
        cave.AddRow("#");

        var top = false;

        while (!top)
        {
            pour = pour.CopyBase(0, cave.Row(0).IndexOf("+"));

            while (Fall(cave, pour, true) is LegacyCoordinates sand)
            {
                pour = pour.CopyBase(0, cave.Row(0).IndexOf("+"));
                cave[sand] = "O";

                if (sand.Equals(pour))
                {
                    top = true;
                    break;
                }
            }
        }

        return cave.ToString().Count(x => x == 'O');
    }

    private static LegacyCoordinates Fall(LegacyGrid<string> cave, LegacyCoordinates pour, bool expandBorders)
    {
        var initialDrop = GetTarget(cave, pour);
        var grain = initialDrop.Copy();
        var nextMove = MoveType.Roll;

        bool stopClause(LegacyCoordinates pos) => pos.IsInsideOfBorder && _blocks.Contains(cave[pos]);
        bool outOfBorder(LegacyCoordinates pos) => pos.IsOutsideOfBorder;
        bool fallAgain(LegacyCoordinates pos) => cave[pos.D] == ".";
        LegacyCoordinates move(LegacyCoordinates sand, out MoveType nextMove)
        {
            if (expandBorders)
            {
                if (sand.Y == 0)
                {
                    cave.InsertColumn(0, ".", 2, true);
                    cave[cave.BottomLeft] = "#";
                    cave[cave.BottomLeft.R] = "#";
                }
                else if (sand.Y == cave.Width - 2)
                {
                    cave.AddColumn(".", 2, true);
                    cave[cave.BottomRight] = "#";
                    cave[cave.BottomRight.L] = "#";
                }
            }

            nextMove = MoveType.Roll;
            if (outOfBorder(sand))
            {
                sand = null;
                nextMove = MoveType.Stop;
            }
            else if (fallAgain(sand))
            {
                sand = Fall(cave, sand, expandBorders);
                nextMove = MoveType.Fall;
            }
            return sand;
        };

        while (nextMove is MoveType.Roll)
        {
            nextMove = MoveType.Stop;

            while (grain?.Move(Direction.DL, stopClause) is true)
            {
                grain = move(grain, out nextMove);
            }
            if (grain?.Move(Direction.DR, stopClause) is true)
            {
                grain = move(grain, out nextMove);
            }
        }

        return grain;
    }

    private LegacyGrid<string> BuildCave(List<(int x, int y)> coordPairs, AoCResourceType resourceType, out LegacyCoordinates pour)
    {
        var maxX = coordPairs.Max(x => x.x);
        int minY = coordPairs.Min(x => x.y), maxY = coordPairs.Max(x => x.y);

        var cave = LegacyGrid<string>.CreateGrid(maxX + 1, maxY - minY + 1);
        pour = new(cave, 0, cave.Width + 500 - maxY - 1);

        var rockPaths = Helpers.File_CleanReadText(FileDescription(this, resourceType)).Replace(" -> ", "_")
            .Split('\n').Select(x => x.Split('_').Select(y => new LegacyCoordinates(cave, int.Parse(y.Split(',')[1]), cave.Width + int.Parse(y.Split(',')[0]) - maxY - 1)).ToList())
            .ToList();

        cave[pour] = "+";
        rockPaths.ForEach(path =>
        {
            ListExtensions.ForNTimesDo(path.Count - 1, (int i) =>
            {
                var start = path[0];
                cave[start] = "#";

                while (start.NotEquals(path[i + 1]))
                {
                    start.MoveTowards(path[i + 1], true);
                    cave[start] = "#";
                }
            });
        });
        cave.RebuildColumns();

        return cave;
    }

    private static LegacyCoordinates GetTarget(LegacyGrid<string> cave, LegacyCoordinates pour)
    {
        var column = cave.ColumnSliceDown(pour, true);
        int rock = column.IndexOf("#"), sand = column.IndexOf("O");
        var x = pour.X + (sand == -1 ? rock - 1 : Math.Min(rock - 1, sand - 1));
        return new(cave, x, pour.Y);
    }

    private enum MoveType { Fall, Roll, Stop }
}
