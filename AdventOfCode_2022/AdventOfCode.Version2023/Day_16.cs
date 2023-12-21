using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_16 : AoCBaseDay<int, int, IndexedGrid<string>>
{
    private int _bestResult = 0;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var grid = new IndexedGrid<string>(Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Select(y => y.ToString()).ToList()).ToList());

        return Solution(grid);
    }

    protected override int Part1(IndexedGrid<string> grid)
    {
        var startNode = new Node(Direction.R) { Position = grid.GetCoordinates(true) };
        var bfs = GetBFS(grid);
        bfs.Search(startNode);
        return _bestResult;
    }

    protected override int Part2(IndexedGrid<string> grid)
    {
        var startNodes = ListBuilder.ForI(grid.Width)
                .Select(x => new Node(Direction.D) { Position = grid.GetCoordinates(x, -1) })
            .Concat(ListBuilder.ForI(grid.Width)
                .Select(x => new Node(Direction.U) { Position = grid.GetCoordinates(x, grid.Height) }))
            .Concat(ListBuilder.ForI(grid.Height)
                .Select(y => new Node(Direction.R) { Position = grid.GetCoordinates(-1, y) }))
            .Concat(ListBuilder.ForI(grid.Height)
                .Select(y => new Node(Direction.L) { Position = grid.GetCoordinates(grid.Width, y) }))
                .ToList();

        var bfs = GetBFS(grid);
        bfs.Search(startNodes);
        return _bestResult;
    }

    private BFS<Node> GetBFS(IndexedGrid<string> grid)
    {
        var newNodesDictionary = new Dictionary<Node, int>();
        _bestResult = 0;

        List<Node> getNeighbours(Node previousNode)
        {
            var neighbours = new List<Node>();
            var dir = previousNode.Direction;

            if (previousNode.Position.GetFromDirection(dir) is Coordinates nextPos && grid[nextPos] is string nextNode)
            {
                var newNodes = (nextNode switch
                {
                    var t when t == "." || (t == "-" && dir is Direction.R or Direction.L) || (t == "|" && dir is Direction.U or Direction.D) => [dir],
                    var t when (t == @"\" && dir is Direction.R) || (t == "/" && dir is Direction.L) => [Direction.D],
                    var t when (t == @"\" && dir is Direction.L) || (t == "/" && dir is Direction.R) => [Direction.U],
                    var t when (t == @"\" && dir is Direction.U) || (t == "/" && dir is Direction.D) => [Direction.L],
                    var t when (t == @"\" && dir is Direction.D) || (t == "/" && dir is Direction.U) => [Direction.R],
                    "-" when dir is Direction.U or Direction.D => [Direction.R, Direction.L],
                    "|" when dir is Direction.L or Direction.R => [Direction.U, Direction.D],
                    _ => new List<Direction>()
                })
                .Select(x => new Node(x) { Position = nextPos, Depth = previousNode.Depth })
                .ToList();

                newNodes.ForEachDo(x => newNodesDictionary.TryAdd(x, x.Depth));

                neighbours.AddRange(newNodes);
            }

            return neighbours;
        };
        void visitNode(Node node) => node.Depth++;
        void nodeSearchFinished(BFS<Node> bfs, Node nodeFinished)
        {
            var newResult = bfs.Visited.Keys.OfType<Node>().Select(x => x.Position).Distinct().Where(x => x.IsInsideOfBorder).OrderBy(x => x.X).ThenBy(y => y.Y).Count();

            if (newResult > _bestResult)
            {
                _bestResult = newResult;
            }
        }

        return new BFS<Node>(getNeighbours, visitNode: visitNode, startNodeFinished: nodeSearchFinished);
    }

    private static string TextFromDir(Direction dir)
    {
        return dir switch
        {
            Direction.R => ">",
            Direction.L => "<",
            Direction.U => "^",
            Direction.D => "V",
        };
    }

    public class Node(Direction direction) : CoordinatesNode
    {
        public Direction Direction { get; init; } = direction;
        public int Depth { get; set; }

        public override bool Equals(object? obj) => obj is Node other && other.Position.Equals(Position) && other.Direction == Direction;
        public override int GetHashCode() => $"{Position}{Direction}".GetHashCode();
    }
}