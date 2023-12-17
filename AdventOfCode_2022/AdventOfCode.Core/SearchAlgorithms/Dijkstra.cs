using AdventOfCode.Core.Extensions;

namespace AdventOfCode.Core.SearchAlgorithms;

public class Dijkstra<TNode>(
    Func<TNode, List<TNode>> getNeighbours,
    Func<TNode, TNode, int> getMoveCost,
    Func<TNode, bool> prune = null) where TNode : DijkstraNode
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; } = getNeighbours;
    private Func<TNode, TNode, int> GetMoveCost { get; set; } = getMoveCost;
    private Func<TNode, bool> Prune { get; set; } = prune;

    public List<TNode> GetShortestPath(TNode start, TNode end)
    {
        var prune = Prune is not null;
        var openSet = new PriorityQueue<TNode, int>();
        openSet.Enqueue(start, 0);

        var closedSet = new HashSet<TNode>();

        while (openSet.TryDequeue(out var currentNode, out _))
        {
            if (closedSet.Contains(currentNode))
            {
                continue;
            }
            else if (currentNode.Position.Equals(end.Position))
            {
                return RetracePath(start, currentNode);
            }

            closedSet.Add(currentNode);

            foreach (var neighbour in GetNeighbours(currentNode))
            {
                if ((prune && Prune(neighbour)) || closedSet.Contains(neighbour))
                {
                    continue;
                }

                var newStartCost = currentNode.Cost + GetMoveCost(currentNode, neighbour);

                if (newStartCost < neighbour.Cost || !closedSet.Contains(neighbour))
                {
                    neighbour.Cost = newStartCost;
                    neighbour.Parent = currentNode;
                    openSet.Enqueue(neighbour, neighbour.Cost);
                }
            }
        }

        return null;
    }

    private static List<TNode> RetracePath(TNode startNode, TNode endNode)
    {
        var path = new List<TNode>();
        var currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = (TNode)currentNode.Parent;
        }

        return path.ReverseList();
    }
}
