using System.Diagnostics;
using System.Net;

namespace AdventOfCode_2022.Models;

public class DFS<TNode, TMemoValue>
    where TNode : class, INode
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; }
    private Func<TNode, bool> Prune { get; set; }
    private Action<TNode> VisitNode { get; set; }
    private Func<TNode, bool> CloseNode { get; set; }

    public DFS(
        Func<TNode, List<TNode>> getNeighboursFunc,
        Func<TNode, bool> closeNodeAction,
        Action<TNode> visitNodeAction = null,
        Func<TNode, bool> pruneFunc = null)
    {
        GetNeighbours = getNeighboursFunc;
        CloseNode = closeNodeAction;
        VisitNode = visitNodeAction;
        Prune = pruneFunc;
    }

    public Stack<TNode> Stack { get; set; } = new();

    public HashSet<TNode> Visited { get; set; } = new();

    public int VisitedCountHit { get; set; }
    public int MemoCountHit { get; set; }
    public int PruneCountHit { get; set; }

    public void Search(TNode start, bool debugMode = false)
    {
        Stopwatch sw = new();
        sw.Start();

        TNode node;
        Stack.Push(start);

        while (Stack.Count > 0)
        {
            node = Stack.Pop();

            Visit(node);

            if (Close(node))
                continue;

            foreach (TNode n in GetNeighbours(node))
            {
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
                    Stack.Push(n);
                }
            }
        }

        sw.Stop();

        if (debugMode)
        {
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            Console.WriteLine($"Visited: {Visited.Count}");
            Console.WriteLine($"Visited Hits: {VisitedCountHit}");
            Console.WriteLine($"Prune Hits: {PruneCountHit}");
        }
    }

    public void Visit(TNode node)
    {
        if (VisitNode is not null)
        {
            VisitNode(node);
        }
        Visited.Add(node);
    }

    public bool Close(TNode node)
    {
        return CloseNode(node);
    }
}