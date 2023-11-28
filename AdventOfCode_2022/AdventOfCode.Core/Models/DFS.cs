using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Bases;
using System.Diagnostics;

namespace AdventOfCode.Core.Models;

public class DFS<TNode> where TNode : NodeBase
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; }
    private Func<TNode, bool> Prune { get; set; }
    private Action<TNode> VisitNode { get; set; }
    private Func<TNode, bool> ShouldCloseNode { get; set; }
    private Action<TNode> CloseParent { get; set; }
    private Action<TNode> CloseChildren { get; set; }

    public DFS(
        Func<TNode, List<TNode>> getNeighbours,
        Func<TNode, bool> shouldCloseNode,
        Action<TNode> visitNode = null,
        Action<TNode> closeParent = null,
        Action<TNode> closeChildren = null,
        Func<TNode, bool> prune = null)
    {
        GetNeighbours = getNeighbours;
        ShouldCloseNode = shouldCloseNode;
        VisitNode = visitNode;
        CloseParent = closeParent;
        CloseChildren = closeChildren;
        Prune = prune;
    }

    public Stack<TNode> Stack { get; set; } = new();
    public HashSet<TNode> Visited { get; set; } = new();
    public int VisitedCountHit { get; set; }
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

            foreach (var n in GetNeighbours(node))
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
            node.Closed = true;

            if (CloseParent is not null)
            {
                CloseParent(node);
            }

            var parent = (TNode)node.Parent;
            while (parent?.Children?.All(x => x.Closed) == true)
            {
                parent.Closed = true;

                if (CloseChildren is not null)
                {
                    CloseChildren(parent);
                }

                parent = (TNode)parent.Parent;
            }

            return true;
        }
        return false;
    }
}