using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.SearchAlgorithms;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2023.Day_17;

namespace AdventOfCode.Version2023;

public class Day_17 : AoCBaseDay<int, int, IndexedGrid<Node>>
{
    private Node _startNode;
    private Coordinates _endPosition;
    private List<Direction> _directions;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var grid = new IndexedGrid<Node>(Helpers.FileCleanReadLines(FileDescription(this, AoCResourceType.Solution))
            .Select(x => x.Select(y => new Node(int.Parse(y.ToString()))).ToList()).ToList());

        _directions = [Direction.U, Direction.D, Direction.L, Direction.R];
        _startNode = grid[grid.GetCoordinates(GridCorner.TopLeft)];
        _endPosition = grid.GetCoordinates(GridCorner.BottomRight);

        return Solution(grid);
    }

    protected override int Part1(IndexedGrid<Node> grid)
    {
        var pruneCoordinates = new Dictionary<Coordinates, int>();

        List<Node> getNeighbours(Node previousNode)
        {
            var neighbours = new List<Node>();

            foreach (var direction in _directions)
            {
                if (direction.IsOpposite(previousNode.Direction) || (previousNode.Direction == direction && previousNode.ConsecutiveStraight > 2))
                {
                    continue;
                }
                else if (previousNode.Position.GetFromDirection(direction) is Coordinates nextPos)
                {
                    var nextNode = grid[nextPos];

                    neighbours.Add(new Node(nextPos, nextNode.Heatloss)
                    {
                        Direction = direction,
                        ConsecutiveStraight = direction == previousNode.Direction ? previousNode.ConsecutiveStraight + 1 : 1,
                    });
                }
            }

            return neighbours.ToList();
        };

        int getMoveCost(Node from, Node to) => to.Heatloss;

        bool prune(Node neighbour)
        {
            if (pruneCoordinates.TryGetValue(neighbour.Position, out var count) && count > 12)
            {
                return true;
            }
            else if (!pruneCoordinates.TryAdd(neighbour.Position, 1))
            {
                pruneCoordinates[neighbour.Position] += 1;
            }

            return false;
        };

        var aStar = new Dijkstra<Node>(getNeighbours, getMoveCost, prune);
        var path = aStar.GetShortestPath(_startNode, new Node(_endPosition, grid[_endPosition].Heatloss));

        return path.Last().Cost;
    }

    protected override int Part2(IndexedGrid<Node> grid)
    {
        var pruneCoordinates = new Dictionary<Coordinates, int>();

        List<Node> getNeighbours(Node previousNode)
        {
            var neighbours = new List<Node>();

            foreach (var direction in _directions)
            {
                if (previousNode.Direction != Direction.None && (
                       direction.IsOpposite(previousNode.Direction)
                    || previousNode.ConsecutiveStraight > 10
                    || (previousNode.Direction != direction && previousNode.ConsecutiveStraight < 4)))
                {
                    continue;
                }

                if (previousNode.Position.GetFromDirection(direction) is Coordinates nextPos)
                {
                    if (nextPos.Equals(_endPosition) && (previousNode.Direction != direction || previousNode.ConsecutiveStraight < 3))
                    {
                        continue;
                    }

                    var nextNode = grid[nextPos];

                    neighbours.Add(new Node(nextPos, nextNode.Heatloss)
                    {
                        Direction = direction,
                        ConsecutiveStraight = direction == previousNode.Direction ? previousNode.ConsecutiveStraight + 1 : 1,
                    });
                }
            }

            return neighbours.ToList();
        };

        int getMoveCost(Node from, Node to) => to.Heatloss;

        bool prune(Node neighbour)
        {
            if (pruneCoordinates.TryGetValue(neighbour.Position, out var count) && count > 20)
            {
                return true;
            }
            else if (!pruneCoordinates.TryAdd(neighbour.Position, 1))
            {
                pruneCoordinates[neighbour.Position] += 1;
            }

            return false;
        };

        var aStar = new Dijkstra<Node>(getNeighbours, getMoveCost, prune);
        var path = aStar.GetShortestPath(_startNode, new Node(_endPosition, grid[_endPosition].Heatloss));

        return path.Last().Cost;
    }

    public class Node(int heatloss) : DijkstraNode
    {
        public Node(Coordinates position, int heatloss) : this(heatloss)
        {
            Position = position;
        }

        public int Heatloss { get; init; } = heatloss;
        public Direction Direction { get; set; }
        public int ConsecutiveStraight { get; set; }

        public override bool Equals(object? obj) => obj is Node n && n.Position.Equals(Position) && n.ConsecutiveStraight == ConsecutiveStraight && n.Direction == Direction;
        public override int GetHashCode() => $"{Position}{ConsecutiveStraight}{Direction}".GetHashCode();

        public override string ToString() => Heatloss.ToString();
    }
}