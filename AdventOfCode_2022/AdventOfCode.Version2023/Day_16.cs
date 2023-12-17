using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_16 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        //var grid = new IndexedGrid<Node>(Helpers.FileCleanReadLines(FileDescription(this, resourceType))
        //    .Select(x => x.Select(y => new Node(y.ToString())).ToList()).ToList());

        Console.WriteLine(Part2(default));

        //var right = grid.GetAllPoints(x => x.Position.X == grid.Width - 1);
        //right.ForEachDo(x =>
        //{
        //    x.Position.X += 1;
        //    x.Directions = [Direction.L];
        //});

        //var energized = 0;

        //foreach (var startNode in right)
        //{
        //    grid = new IndexedGrid<Node>(Helpers.FileCleanReadLines(FileDescription(this, resourceType))
        //        .Select(x => x.Select(y => new Node(y.ToString())).ToList()).ToList());

        //    var bfs = GetBFS(grid);
        //    bfs.Search(startNode, true);

        //    var energizedCount = grid.GetAllPoints(x => x.Energized > 0).Count;

        //    if (energizedCount > energized)
        //    {
        //        energized = energizedCount;
        //    }
        //}

        //grid.GetAllPoints(x => x.Energized == 0).ForEachDo(x => x.Text = ".");

        //var allPoints = grid.GetAllPoints(x => x.Energized > 0);

        //allPoints.ForEachDo(x => x.Text = "#");
        //grid.Print();

        //Console.WriteLine(energized);

        return default;
    }

    protected override int Part1(string[] args)
    {
        //var grid = new IndexedGrid<Node>(Helpers.FileCleanReadLines(FileDescription(this, AoCResourceType.Solution))
        //    .Select(x => x.Select(y => new Node(y.ToString())).ToList()).ToList());

        //var startNode = new Node(".")
        //{
        //    Directions = [Direction.R],
        //    Position = grid.GetCoordinates(true)
        //};

        //List<Node> getNeighbours(Node previousNode)
        //{
        //    var neighbours = new List<Node>();

        //    foreach (var dir in previousNode.Directions)
        //    {
        //        if (previousNode.Position.GetFromDirection(dir) is not Coordinates nextPos || grid[nextPos] is not Node nextNode)
        //        {
        //            continue;
        //        }

        //        nextNode.Directions.AddRange((nextNode.Text switch
        //        {
        //            var t when (t == @"\" && dir is Direction.R) || (t == "/" && dir is Direction.L) => [Direction.D],
        //            var t when (t == @"\" && dir is Direction.L) || (t == "/" && dir is Direction.R) => [Direction.U],
        //            var t when (t == @"\" && dir is Direction.U) || (t == "/" && dir is Direction.D) => [Direction.L],
        //            var t when (t == @"\" && dir is Direction.D) || (t == "/" && dir is Direction.U) => [Direction.R],
        //            "-" when dir is Direction.U or Direction.D => [Direction.R, Direction.L],
        //            "|" when dir is Direction.L or Direction.R => [Direction.U, Direction.D],
        //            var t when t == "." || (t == "-" && dir is Direction.R or Direction.L) || (t == "|" && dir is Direction.U or Direction.D) => [dir],
        //            _ => new List<Direction>()
        //        })
        //        .Where(x => !nextNode.DirectionDepth.ContainsKey(x)));

        //        neighbours.Add(nextNode);
        //    }

        //    previousNode.Directions.ForEachDo(dir => previousNode.DirectionDepth.TryAdd(dir, previousNode.Depth));
        //    previousNode.Directions.Clear();

        //    return neighbours;
        //};
        //void visitNode(Node node) => node.Depth++;

        //var bfs = new BFS<Node>(getNeighbours, visitNode: visitNode);
        //bfs.Search(startNode, true);

        //var points = grid.GetAllPoints(x => x.Depth > 0).ToList();

        //return points.Count;
        return 0;
    }

    protected override int Part2(string[] args)
    {
        var resource = AoCResourceType.Solution;

        var grid = new IndexedGrid<string>(Helpers.FileCleanReadLines(FileDescription(this, resource))
            .Select(x => x.Select(y => y.ToString()).ToList()).ToList());

        var startNodes = ListBuilder.ForI(grid.Width)
                .Select(x => new Node(Direction.D) { Position = grid.GetCoordinates(x, -1) })
            .Concat(ListBuilder.ForI(grid.Width)
                .Select(x => new Node(Direction.U) { Position = grid.GetCoordinates(x, grid.Height) }))
            .Concat(ListBuilder.ForI(grid.Height)
                .Select(y => new Node(Direction.R) { Position = grid.GetCoordinates(-1, y) }))
            .Concat(ListBuilder.ForI(grid.Height)
                .Select(y => new Node(Direction.L) { Position = grid.GetCoordinates(grid.Width, y) }))
                .ToList();

        //var startNode = new Node(Direction.R) { Position = grid.GetCoordinates(-1, 0) };
        //startNodes = [startNode, new(Direction.D) { Position = grid.GetCoordinates(3, -1) }];

        var newNodesDictionary = new Dictionary<Node, int>();
        var bestResult = 0;

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

            if (newResult > bestResult)
            {
                bestResult = newResult;
            }
        }
        //bool shouldCloseNode(Node node) => dictionary.TryGetValue(node, out var depth) && depth < node.Depth;

        var bfs = new BFS<Node>(getNeighbours, visitNode: visitNode, startNodeFinished: nodeSearchFinished);
        bfs.Search(startNodes, true);

        foreach (var node in bfs.Visited)
        {
            try
            {
                grid[((CoordinatesNode)node.Key).Position] = "O";
            }
            catch (Exception)
            {
            }
        }

        //Console.WriteLine(bfs.Visited.Keys.OfType<Node>().Select(x => x.Position).Distinct().Where(x => x.IsInsideOfBorder).OrderBy(x => x.X).ThenBy(y => y.Y).Count());
        Console.WriteLine(bestResult);

        return 0;
    }

    private static BFS<Node> GetBFS(IndexedGrid<Node> grid)
    {
        //List<Node> getNeighbours(Node previousNode)
        //{
        //    var neighbours = new List<Node>();

        //    foreach (var dir in previousNode.Directions)
        //    {
        //        if (previousNode.Position.GetFromDirection(dir) is not Coordinates nextPos)
        //        {
        //            continue;
        //        }

        //        var nextNode = grid[nextPos];

        //        if ((nextNode.Text == "-" && dir is Direction.R or Direction.L) || (nextNode.Text == "|" && dir is Direction.U or Direction.D))
        //        {
        //            nextNode.Directions.AddIf(dir, !nextNode.DirectionsFinished.Contains(dir));
        //        }
        //        else if (nextNode.Text == "-" && dir is Direction.U or Direction.D)
        //        {
        //            nextNode.Directions.AddIf(Direction.R, !nextNode.DirectionsFinished.Contains(Direction.R));
        //            nextNode.Directions.AddIf(Direction.L, !nextNode.DirectionsFinished.Contains(Direction.L));
        //        }
        //        else if (nextNode.Text == "|" && dir is Direction.L or Direction.R)
        //        {
        //            nextNode.Directions.AddIf(Direction.U, !nextNode.DirectionsFinished.Contains(Direction.U));
        //            nextNode.Directions.AddIf(Direction.D, !nextNode.DirectionsFinished.Contains(Direction.D));
        //        }

        //        else if (nextNode.Text == @"\" && dir is Direction.R)
        //        {
        //            nextNode.Directions.AddIf(Direction.D, !nextNode.DirectionsFinished.Contains(Direction.D));
        //        }
        //        else if (nextNode.Text == @"\" && dir is Direction.L)
        //        {
        //            nextNode.Directions.AddIf(Direction.U, !nextNode.DirectionsFinished.Contains(Direction.U));
        //        }
        //        else if (nextNode.Text == @"\" && dir is Direction.U)
        //        {
        //            nextNode.Directions.AddIf(Direction.L, !nextNode.DirectionsFinished.Contains(Direction.L));
        //        }
        //        else if (nextNode.Text == @"\" && dir is Direction.D)
        //        {
        //            nextNode.Directions.AddIf(Direction.R, !nextNode.DirectionsFinished.Contains(Direction.R));
        //        }

        //        else if (nextNode.Text == "/" && dir is Direction.R)
        //        {
        //            nextNode.Directions.AddIf(Direction.U, !nextNode.DirectionsFinished.Contains(Direction.U));
        //        }
        //        else if (nextNode.Text == "/" && dir is Direction.L)
        //        {
        //            nextNode.Directions.AddIf(Direction.D, !nextNode.DirectionsFinished.Contains(Direction.D));
        //        }
        //        else if (nextNode.Text == "/" && dir is Direction.U)
        //        {
        //            nextNode.Directions.AddIf(Direction.R, !nextNode.DirectionsFinished.Contains(Direction.R));
        //        }
        //        else if (nextNode.Text == "/" && dir is Direction.D)
        //        {
        //            nextNode.Directions.AddIf(Direction.L, !nextNode.DirectionsFinished.Contains(Direction.L));
        //        }

        //        else
        //        {
        //            nextNode.Text = TextFromDir(dir);
        //            nextNode.Directions.AddIf(dir, !nextNode.DirectionsFinished.Contains(dir));
        //        }

        //        neighbours.Add(nextNode);
        //    }

        //    previousNode.DirectionsFinished.AddRange(previousNode.Directions);
        //    previousNode.Directions.Clear();
        //    return neighbours;
        //};
        //void visitNode(Node node) => node.Energized++;

        //return new BFS<Node>(getNeighbours, visitNode: visitNode);
        return default;
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