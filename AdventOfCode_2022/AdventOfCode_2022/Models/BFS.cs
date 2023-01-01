using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Models.Bases;
using System.Diagnostics;

namespace AdventOfCode_2022.Models;

public class BFS<TNode> where TNode : NodeBase
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; }
    private Func<TNode, bool> Prune { get; set; }
    private Action<TNode> VisitNode { get; set; }
    private Func<TNode, bool> ShouldCloseNode { get; set; }

    public BFS(
        Func<TNode, List<TNode>> getNeighboursFunc,
        Func<TNode, bool> shouldCloseNode,
        Action<TNode> visitNode = null,
        Func<TNode, bool> prune = null)
    {
        GetNeighbours = getNeighboursFunc;
        ShouldCloseNode = shouldCloseNode;
        VisitNode = visitNode;
        Prune = prune;
    }

    public Queue<TNode> Queue { get; set; }
    public HashSet<TNode> Visited { get; set; }
    public Dictionary<TNode, int> Memos { get; set; } = new();
    public int VisitedCountHit { get; set; }
    public int PruneCountHit { get; set; }
    public int ShortestPath { get; set; }
    public bool PathFound { get; set; }

    public void SearchShortestPath(TNode start, bool debugMode = false)
    {
        Stopwatch sw = new();
        sw.Start();

        ResetNodes();
        TNode node = start;
        Queue.Enqueue(start);

        while (Queue.Count > 0)
        {
            node = Queue.Dequeue();

            Visit(node);

            if (Memos.ContainsKey(node) || Close(node))
            {
                PathFound = true;
                break;
            }

            foreach (NodeBase n in GetNeighbours(node))
            {
                if (Queue.Contains(n))
                    continue;

                if (Visited.Contains(n))
                {
                    VisitedCountHit++;
                }
                else if (Prune is not null && Prune(node))
                {
                    PruneCountHit++;
                }
                else
                {
                    node.AddChild(n);
                    n.SetParent(node);
                    Queue.Enqueue((TNode)n);
                }
            }
        }

        ShortestPath = PathFound ? CountParents(node) + Memos[node] : int.MaxValue;
        sw.Stop();

        if (debugMode)
        {
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            Console.WriteLine($"Visited: {Visited.Count}");
            Console.WriteLine($"Visited Hits: {VisitedCountHit}");

            if (Prune is not null)
            {
                Console.WriteLine($"Prune Hits: {PruneCountHit}");
            }
        }
    }

    private void Visit(TNode node)
    {
        if (VisitNode is not null)
        {
            VisitNode(node);
        }
        Visited.Add(node);
    }

    private bool Close(TNode node)
    {
        if (ShouldCloseNode(node))
        {
            int steps = 0;
            Memos.Add(node, steps);

            TNode parent = (TNode)node.Parent;
            while (parent is not null)
            {
                steps++;
                Memos.Add(parent, steps);
                parent = (TNode)parent.Parent;
            }

            PathFound = true;
            return true;
        }
        return false;
    }

    private int CountParents(TNode node)
    {
        int steps = 0;
        TNode parent = (TNode)node.Parent;
        while (parent is not null)
        {
            steps++;
            parent = (TNode)parent.Parent;
        }
        return steps;
    }

    private void ResetNodes()
    {
        Queue = new Queue<TNode>();
        Visited = new HashSet<TNode>();
        PathFound = false;
        VisitedCountHit = 0;
        PruneCountHit = 0;
        ShortestPath = -1;
    }
}