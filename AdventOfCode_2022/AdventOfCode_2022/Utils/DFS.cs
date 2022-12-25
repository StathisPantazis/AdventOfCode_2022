using AdventOfCode_2022.Extensions;

namespace AdventOfCode_2022.Utils;

public class DFS<TNode, TData>
    where TNode : class, INode
    where TData : class {

    private Func<TNode, List<TNode>> GetNeighbours { get; set; }
    private Func<TNode, bool> ShouldContinue { get; set; }
    private Action<TNode> VisitNode { get; set; }
    private Action<TNode, TData> CloseNode { get; set; }

    public DFS(
        TData iterationData,
        Func<TNode, List<TNode>> getNeighboursFunc,
        Action<TNode> visitNodeAction,
        Action<TNode, TData> closeNodeAction,
        Func<TNode, bool> shouldContinueFunc) {
        IterationData = iterationData;
        GetNeighbours = getNeighboursFunc;
        ShouldContinue = shouldContinueFunc;
        CloseNode = closeNodeAction;
        VisitNode = visitNodeAction;
    }

    public List<Stack<TNode>> ValidPaths { get; set; } = new();

    public List<TNode> VisitedNodes { get; set; } = new();

    public Stack<TNode> Stack { get; set; } = new();

    private TNode CurrentNode { get; set; }

    private TData IterationData { get; set; }

    private int CurrentDepth { get; set; }

    public bool SearchTree(TNode start) {
        TNode next = start;
        Visit(start);

        while (next is not null) {
            next = SearchForward();
        }

        if (SearchBackward() is TNode previous) {
            // Delete all above-visited paths
            //VisitedNodes.RemoveAll(x => x.StartsWith(previous.Key[..^1]) && x.Length > previous.Key.Length);
            return SearchTree(previous);
        }

        return true;
    }

    public void Visit(TNode node) {
        CurrentNode = node;
        VisitNode(CurrentNode);
        VisitedNodes.Add(CurrentNode);
    }

    public void Close() {
        CloseNode(CurrentNode, IterationData);
        CurrentNode = null;
    }

    public TNode SearchForward() {
        if (ShouldContinue(CurrentNode)) {
            List<TNode> nextNodes = GetNeighbours(CurrentNode).Except(VisitedNodes).Except(Stack).ToList();

            if (!nextNodes.Any()) {
                return null;
            }

            CurrentDepth++;
            TNode next = nextNodes.Pop();
            Visit(next);

            // Dont add nodes for second-to-last node
            if (ShouldContinue(next)) {
                nextNodes.ForEach(n => Stack.Push(n));
            }
            return next;
        }
        return null;
    }

    public TNode SearchBackward() {
        Close();
        return Stack.Any() ? Stack.Pop() : null;
    }
}