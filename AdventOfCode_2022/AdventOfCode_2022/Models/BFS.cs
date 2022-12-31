namespace AdventOfCode_2022.Models;

public class BFS<TNode, TMemo, TData>
    where TNode : class, INode
    where TMemo : struct, IMemo
    where TData : class
{
    private Func<TNode, List<TNode>> GetNeighbours { get; set; }
    private Func<TNode, bool> Prune { get; set; }
    private Action<TNode> VisitNode { get; set; }
    private Func<TNode, TData, bool> CloseNode { get; set; }

    public BFS(
        TData iterationData,
        Func<TNode, List<TNode>> getNeighboursFunc,
        Func<TNode, TData, bool> closeNodeAction,
        Action<TNode> visitNodeAction = null,
        Func<TNode, bool> pruneFunc = null)
    {
        IterationData = iterationData;
        GetNeighbours = getNeighboursFunc;
        CloseNode = closeNodeAction;
        VisitNode = visitNodeAction;
        Prune = pruneFunc;
    }

    public Queue<TNode> Queue { get; set; } = new();

    public HashSet<TNode> Visited { get; set; } = new();

    public HashSet<IMemo> Memos { get; set; } = new();

    private TData IterationData { get; set; }

    public void Search(TNode start)
    {
        TNode node;
        Queue.Enqueue(start);

        while (Queue.Count > 0)
        {
            node = Queue.Dequeue();

            if (Prune is not null && Prune(node))
                continue;
            else if (Visited.Contains(node))
                continue;

            Visit(node);

            if (Close(node))
                continue;

            foreach (TNode n in GetNeighbours(node))
            {
                if (!Visited.Contains(n))
                {
                    Queue.Enqueue(n);
                }
            }
        }
    }

    public void Visit(TNode node)
    {
        Visited.Add(node);
        if (VisitNode is not null)
        {
            VisitNode(node);
        }
    }

    public bool Close(TNode node)
    {
        return CloseNode(node, IterationData);
    }
}