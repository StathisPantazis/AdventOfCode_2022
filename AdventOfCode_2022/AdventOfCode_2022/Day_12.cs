using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Utils;
using System.ComponentModel;
using System.IO;

namespace AdventOfCode_2022;

internal static class Day_12 {
    private static readonly Direction[] AllowedMoves = new Direction[] { Direction.L, Direction.R, Direction.U, Direction.D };

    public static void Solve() {
        LegacyGrid<char> grid = new(Helpers.File_CleanReadLines(12), singleCharacters: true);
        LegacyCoordinates pos = new(grid, true);
        LegacyCoordinates goal = new(grid);
        List<NodePath> paths = new();
        Dictionary<LegacyCoordinates, int> availableNodes = new();
        List<Node> availablePaths = new();
        Dictionary<LegacyCoordinates, int> fastestPaths = new();

        while (pos.TraverseGrid()) {
            fastestPaths.Add(pos.Copy(), 0);
        }
        pos.GoToStart(true);

        // Find start and finish
        while (pos.TraverseGrid()) {
            if (grid[pos] == 'S') {
                grid[pos] = 'a';
                break;
            }
        }

        while (goal.TraverseGrid()) {
            if (grid[goal] == 'E') {
                grid[goal] = '{';
                break;
            }
        }

        Node currentNode = new(pos, null);
        List<Node> nodes = new();
        bool started = true;
        while (started || availablePaths.Count > 0) {
            started = false;
            NodePath path = new();
            if (Move(grid, currentNode, nodes, path, availableNodes, availablePaths, fastestPaths)) {
                paths.Add(path);
            }

            for (int i = path.Nodes.Count - 2; i > 0; i--) {
                availableNodes[path.Nodes[i].Coordinates] -= 1;

                if (availableNodes[path.Nodes[i].Coordinates] > 0) {
                    break;
                }
            }

            if (availablePaths.Count > 0) {
                currentNode = availablePaths.First();
            }
        }

        var lala = paths.Where(x => x.Nodes.Last().Coordinates.Equals(goal)).OrderBy(x => x.Nodes.Count).ToList().First().Nodes.Count - 1;
        Console.WriteLine(lala);
    }

    private static bool Move(LegacyGrid<char> grid, Node currentNode, List<Node> visitedNodes, NodePath path, Dictionary<LegacyCoordinates, int> availableNodes, List<Node> availblePaths, Dictionary<LegacyCoordinates, int> fastestPaths) {
        Node nextNode = GetNode(grid, currentNode, path, availableNodes, availblePaths);

        if (nextNode is null) {
            return false;
        }

        currentNode = new(currentNode.Coordinates, (Direction)nextNode.Direction);
        visitedNodes.Add(currentNode);
        path.Nodes.Add(currentNode);

        if (grid[nextNode.Coordinates] == '{') {
            path.Nodes.Add(nextNode);

            for (int i = path.Nodes.Count - 1; i > 0; i--) {
                fastestPaths[path.Nodes[i].Coordinates] = i;
            }

            return true;
        }
        else {
            return Move(grid, nextNode, visitedNodes, path, availableNodes, availblePaths, fastestPaths);
        }
    }

    private static Node GetNode(LegacyGrid<char> grid, Node currentNode, NodePath path, Dictionary<LegacyCoordinates, int> availableNodes, List<Node> availblePaths) {
        Node nextNode = null;

        List<Node> nextNodes = GetNextMoves(grid, currentNode, path);

        if (!availableNodes.ContainsKey(currentNode.Coordinates)) {
            availableNodes.Add(currentNode.Coordinates, nextNodes.Count);

            foreach (Node node in nextNodes) {
                availblePaths.Add(new Node(currentNode.Coordinates, node.Direction));
            }
        }

        if (availableNodes[currentNode.Coordinates] != 0) {
            foreach (Direction direction in nextNodes.Select(x => x.Direction)) {
                if (availblePaths.FirstOrDefault(x => x.Equals(new Node(currentNode.Coordinates, direction))) is Node n) {
                    nextNode = nextNodes.First(x => x.Direction == direction);
                    availblePaths.Remove(n);
                    break;
                }
            }
        }

        return nextNode;
    }

    private static List<Node> GetNextMoves(LegacyGrid<char> grid, Node currentNode, NodePath path) {
        Direction? previous = currentNode.Direction is null ? null : currentNode.Direction switch {
            Direction.L => Direction.R,
            Direction.R => Direction.L,
            Direction.U => Direction.D,
            Direction.D => Direction.U,
            _ => null
        };

        List<Node> nodes = new();

        foreach (Direction d in AllowedMoves.Where(x => x != previous)) {
            if (currentNode.Coordinates.TryGetNeighbour(d, out LegacyCoordinates near)
                && path.Nodes.None(x => x.Coordinates.Equals(near))
                && grid[near] - grid[currentNode.Coordinates] is int dif && dif is 0 or 1) {
                nodes.Add(new Node(near, d));
            }
        }

        return nodes;
    }

    private class NodePath {
        public List<Node> Nodes { get; set; } = new();

        public override string ToString() => string.Join("", Nodes.Select(x => x.Direction));
    }

    private class Node {
        public Node(LegacyCoordinates coordinates, Direction? direction) {
            Coordinates = coordinates;
            Direction = direction;
        }

        public LegacyCoordinates Coordinates { get; set; }

        public Direction? Direction { get; set; }

        public bool Equals(Node other) => other.Coordinates.Equals(Coordinates) && other.Direction == Direction;

        public Node Copy() => new(Coordinates, Direction);

        public override string ToString() => $"[{Coordinates.X} , {Coordinates.Y}] - {Direction}";
    }
}
