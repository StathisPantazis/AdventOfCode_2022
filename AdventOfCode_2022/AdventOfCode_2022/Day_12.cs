using AdventOfCode_2022.Models;
using AdventOfCode_2022.Models.Bases;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_12
{
    private static readonly int _startValue = 0;
    private static readonly int _endValue = 27;
    private static readonly Direction[] AllowedMoves = new Direction[] { Direction.L, Direction.R, Direction.U, Direction.D };

    public static void Solve()
    {
        string[] input = Helpers.File_CleanReadLines(12).Select(x => x.Replace("S", "0").Replace("E", "_")).ToArray();
        Grid<int> grid = GetGrid(input);

        Console.WriteLine($"Part_1: {Part_1(grid)}");
        Console.WriteLine($"Part_2: {Part_2(grid)}");
    }

    private static int Part_1(Grid<int> grid)
    {
        Node start = null;
        Coordinates pos = new(grid, true);

        while (pos.TraverseGrid())
        {
            if (grid[pos] == _startValue)
            {
                start = new Node(pos.Copy());
                break;
            }
        }

        BFS<Node> bfs = GetBFS(grid);
        bfs.SearchShortestPath(start);
        return bfs.ShortestPath;
    }

    private static int Part_2(Grid<int> grid)
    {
        List<Node> nodes = new();
        Coordinates pos = new(grid, true);

        while (pos.TraverseGrid())
        {
            int elevation = grid[pos];

            if (elevation == _startValue || elevation == 1)
            {
                nodes.Add(new Node(pos.Copy()));
            }
        }

        int shortestPath = int.MaxValue;
        BFS<Node> bfs = GetBFS(grid);

        foreach (Node node in nodes)
        {
            bfs.SearchShortestPath(node);
            if (bfs.PathFound)
            {
                shortestPath = bfs.ShortestPath < shortestPath ? bfs.ShortestPath : shortestPath;
            }
        }

        return shortestPath;
    }

    private static BFS<Node> GetBFS(Grid<int> grid)
    {
        List<Node> getNeighbours(Node node)
        {
            List<Node> neighbours = new();
            List<Node> sameLevelNeighbours = new();

            foreach (Direction dir in AllowedMoves)
            {
                Coordinates next = node.Position.GetFromDirection(dir);
                if (next.NotEquals(node.Position) && next.IsInsideOfBorder && grid[next] - grid[node.Position] <= 1)
                {
                    neighbours.Add(new Node(next));
                }
            }

            return neighbours.Count > 0 ? neighbours : sameLevelNeighbours;
        };
        bool shouldCloseNode(Node node) => grid[node.Position] == _endValue;

        BFS<Node> bfs = new(getNeighbours, shouldCloseNode);
        return bfs;
    }

    private static Grid<int> GetGrid(string[] input)
    {
        List<List<int>> numbers = new();

        foreach (string line in input)
        {
            List<int> ints = new();
            foreach (char letter in line)
            {
                ints.Add(char.IsDigit(letter) ? int.Parse(letter.ToString()) : letter == '_' ? 27 : letter - 96);
            }
            numbers.Add(ints);
        }

        return new Grid<int>(numbers);
    }

    public class Node : NodeBase
    {
        public Node(Coordinates pos)
        {
            Position = pos;
        }

        public Coordinates Position { get; set; }

        public override bool Equals(object? obj) => ((Node)obj).Position.Equals(Position);
        public override int GetHashCode() => Position.GetHashCode();
        public override string ToString() => Position.ToString();
    }
}
