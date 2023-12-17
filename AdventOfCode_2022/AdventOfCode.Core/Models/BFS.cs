using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using System.Diagnostics;

namespace AdventOfCode.Core.Models;

public class BFS<TNode>(
    Func<TNode, List<TNode>> getNeighboursFunc,
    Func<TNode, bool> shouldCloseNode = null,
    Action<TNode> visitNode = null,
    Func<TNode, bool> prune = null,
    Action<BFS<TNode>, TNode> startNodeFinished = null) where TNode : NodeBase
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; } = getNeighboursFunc;
    private Func<TNode, bool> Prune { get; set; } = prune;
    private Action<TNode> VisitNode { get; set; } = visitNode;
    private Func<TNode, bool> ShouldCloseNode { get; set; } = shouldCloseNode;
    private Action<BFS<TNode>, TNode> StartNodeFinished { get; set; } = startNodeFinished;

    public Queue<TNode> Queue { get; set; }
    public Dictionary<NodeBase, bool> Visited { get; set; }
    public Dictionary<TNode, int> Memos { get; set; } = new();
    public int VisitedCountHit { get; set; }
    public int PruneCountHit { get; set; }
    public int PathLength { get; set; }
    public bool PathFound { get; set; }

    public void Search(TNode start, bool debugMode = false) => Search([start], debugMode);

    public void Search(List<TNode> startingNodes, bool debugMode = false)
    {
        Stopwatch sw = new();
        sw.Start();

        foreach (var start in startingNodes)
        {
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

            if (StartNodeFinished is not null)
            {
                StartNodeFinished(this, start);
            }
        }

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
        if (ShouldCloseNode is not null && ShouldCloseNode(node))
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
        Visited = [];
        PathFound = false;
        VisitedCountHit = 0;
        PruneCountHit = 0;
        PathLength = -1;
    }
}