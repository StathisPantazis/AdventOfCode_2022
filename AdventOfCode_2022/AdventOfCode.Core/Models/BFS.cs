using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using System.Diagnostics;

namespace AdventOfCode.Core.Models;

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
    public Dictionary<NodeBase, bool> Visited { get; set; }
    public Dictionary<TNode, int> Memos { get; set; } = new();
    public int VisitedCountHit { get; set; }
    public int PruneCountHit { get; set; }
    public int PathLength { get; set; }
    public bool PathFound { get; set; }

    public void Search(TNode start, bool debugMode = false)
    {
        Stopwatch sw = new();
        sw.Start();

        ResetNodes();
        var node = start;
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

                if (Visited.ContainsKey(n))
                {
                    VisitedCountHit++;
                }
                else if (debugMode && Prune is not null && Prune(node))
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

        PathLength = PathFound ? BFS<TNode>.CountParents(node) + Memos[node] : int.MaxValue;
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
        Visited.Add(node, true);
    }

    private bool Close(TNode node)
    {
        if (ShouldCloseNode(node))
        {
            var steps = 0;
            Memos.Add(node, steps);

            var parent = (TNode)node.Parent;
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

    private static int CountParents(TNode node)
    {
        var steps = 0;
        var parent = (TNode)node.Parent;
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
        Visited = new Dictionary<NodeBase, bool>();
        PathFound = false;
        VisitedCountHit = 0;
        PruneCountHit = 0;
        PathLength = -1;
    }
}