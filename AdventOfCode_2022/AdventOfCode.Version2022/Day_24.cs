using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2022.Day_24;

namespace AdventOfCode.Version2022;

public class Day_24 : AoCBaseDay<int, int, List<Blizzard>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.File_CleanReadLines(24, 2022, resourceType);
        var grid = new Grid<string>(lines, singleCharacters: true);
        var start = new Coordinates(grid, grid.Row(0).IndexOf("."), 0);
        var end = new Coordinates(grid, grid.Rows.Count - 1, grid.Row(grid.Rows.Count - 1).IndexOf("."));
        var queue = new Queue<Node>();
        var visited = new List<Node>();

        var blizzards = new List<Blizzard>();

        for (var r = 0; r < grid.Rows.Count; r++)
        {
            var row = grid.Rows[r];

            for (var c = 0; c < grid.Rows[r].Count; c++)
            {
                var point = row[c];

                if (point is "<" or "^" or ">" or "v")
                {
                    blizzards.Add(new Blizzard(new Coordinates(grid, r, c), ToDirection(point)));
                }
            }
        }

        return new AoCSolution<int, int>(Part1(blizzards), Part2(blizzards));
    }

    protected override int Part1(List<Blizzard> args)
    {
        throw new NotImplementedException();
    }

    protected override int Part2(List<Blizzard> args)
    {
        throw new NotImplementedException();
    }

    private static Direction ToDirection(string str)
    {
        return str switch
        {
            "<" => Direction.L,
            "^" => Direction.U,
            ">" => Direction.R,
            "v" => Direction.D,
            _ => throw new Exception("WRONG")
        };
    }

    public class Blizzard
    {
        public Blizzard(Coordinates pos, Direction dir)
        {
            Pos = pos;
            Dir = dir;
        }

        public Coordinates Pos { get; set; }
        public Direction Dir { get; set; }
    }

    private class Node
    {
        public Node(Coordinates pos)
        {
            X = pos.X;
            Y = pos.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public override bool Equals(object? obj) => ((Node)obj).X == X && ((Node)obj).Y == Y;
        public override int GetHashCode() => $"{X}-{Y}".GetHashCode();
    }
}
