using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_12 : AoCBaseDay<int, int, IndexedGrid<int>>
{
    private static readonly int _startValue = 0;
    private static readonly int _endValue = 27;
    private static readonly Direction[] _allowedMoves = new Direction[] { Direction.L, Direction.R, Direction.U, Direction.D };

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var input = Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Replace("S", "0").Replace("E", "_"))
            .ToArray();

        var grid = GetGrid(input);

        return Solution(grid);
    }

    protected override int Part1(IndexedGrid<int> grid)
    {
        Node start = null;
        var pos = grid.GetCoordinates(true);

        while (pos.TraverseGrid())
        {
            if (grid[pos] == _startValue)
            {
                start = new Node(pos.Copy());
                break;
            }
        }

        var bfs = GetBFS(grid);
        bfs.Search(start);
        return bfs.PathLength;
    }

    protected override int Part2(IndexedGrid<int> grid)
    {
        List<Node> nodes = new();
        var pos = grid.GetCoordinates(true);

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
            bfs.Search(node);
            if (bfs.PathFound)
            {
                shortestPath = bfs.PathLength < shortestPath ? bfs.PathLength : shortestPath;
            }
        }

        return shortestPath;
    }

    private static BFS<Node> GetBFS(IndexedGrid<int> grid)
    {
        List<Node> getNeighbours(Node node)
        {
            List<Node> neighbours = new();
            List<Node> sameLevelNeighbours = new();

            foreach (var dir in _allowedMoves)
            {
                var next = node.Position.GetFromDirection(dir);
                if (next is not null && next.NotEquals(node.Position) && next.IsInsideOfBorder && grid[next] - grid[node.Position] <= 1)
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

    private static IndexedGrid<int> GetGrid(string[] input)
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

        return new IndexedGrid<int>(numbers);
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
