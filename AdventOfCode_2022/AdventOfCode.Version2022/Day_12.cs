using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_12 : AoCBaseDay<int, int, Grid<int>>
{
    private static readonly int _startValue = 0;
    private static readonly int _endValue = 27;
    private static readonly Direction[] _allowedMoves = new Direction[] { Direction.L, Direction.R, Direction.U, Direction.D };

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var input = Helpers.File_CleanReadLines(12, 2022, resourceType)
            .Select(x => x.Replace("S", "0").Replace("E", "_"))
            .ToArray();

        var grid = GetGrid(input);

        return new AoCSolution<int, int>(Part1(grid), Part2(grid));
    }

    protected override int Part1(Grid<int> grid)
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

        var bfs = GetBFS(grid);
        bfs.SearchShortestPath(start);
        return bfs.ShortestPath;
    }

    protected override int Part2(Grid<int> grid)
    {
        List<Node> nodes = new();
        Coordinates pos = new(grid, true);

        while (pos.TraverseGrid())
        {
            var elevation = grid[pos];

            if (elevation == _startValue || elevation == 1)
            {
                nodes.Add(new Node(pos.Copy()));
            }
        }

        var shortestPath = int.MaxValue;
        var bfs = GetBFS(grid);

        foreach (var node in nodes)
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

            foreach (var dir in _allowedMoves)
            {
                var next = node.Position.GetFromDirection(dir);
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

        foreach (var line in input)
        {
            List<int> ints = new();
            foreach (var letter in line)
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
