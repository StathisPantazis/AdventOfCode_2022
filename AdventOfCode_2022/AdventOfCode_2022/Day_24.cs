using AdventOfCode_2022.Utils;
using System.Security.Cryptography.X509Certificates;
using static AdventOfCode_2022.Day_24;

namespace AdventOfCode_2022;

internal static class Day_24 {
    public static void Solve() {
        Grid<string> grid = new(Helpers.File_CleanReadLines(24), singleCharacters: true);
        Coordinates start = new(grid, grid.Row(0).IndexOf("."), 0);
        Coordinates end = new (grid, grid.Rows.Count - 1, grid.Row(grid.Rows.Count - 1).IndexOf("."));
        List<Blizzard> blizzards = GetBlizzards(grid);
        Queue<Node> queue = new();
        List<Node> visited = new();

        
    }

    public class Node {
        public Node(Coordinates pos) {
            X = pos.X;
            Y = pos.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public override bool Equals(object? obj) => ((Node)obj).X == X && ((Node)obj).Y == Y;
        public override int GetHashCode() => $"{X}-{Y}".GetHashCode();
    }

    public static List<Blizzard> GetBlizzards(Grid<string> grid) {
        List<Blizzard> blizzards = new();

        for (int r = 0; r < grid.Rows.Count; r++) {
            List<string> row = grid.Rows[r];

            for (int c = 0; c < grid.Rows[r].Count; c++) {
                string point = row[c];

                if (point is "<" or "^" or ">" or "v") {
                    blizzards.Add(new Blizzard(new Coordinates(grid, r, c), point.ToDirection()));
                }
            }
        }

        return blizzards;
    }

    public static Direction ToDirection(this string str) {
        return str switch {
            "<" => Direction.L,
            "^" => Direction.U,
            ">" => Direction.R,
            "v" => Direction.D,
            _ => throw new Exception("WRONG")
        };
    }

    public class Blizzard {
        public Blizzard(Coordinates pos, Direction dir) {
            Pos = pos;
            Dir = dir;
        }

        public Coordinates Pos { get; set; }
        public Direction Dir { get; set; }
    }
}
