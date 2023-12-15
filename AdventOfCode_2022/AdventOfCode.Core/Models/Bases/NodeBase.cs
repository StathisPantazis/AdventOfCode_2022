namespace AdventOfCode.Core.Models.Bases;

public abstract class NodeBase
{
    public NodeBase Parent { get; set; }
    public List<NodeBase> Children { get; set; } = [];
    public bool Closed { get; set; }
}
