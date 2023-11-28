using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models.Bases;

public abstract class NodeBase : INode
{
    public INode Parent { get; set; }
    public List<INode> Children { get; set; } = new();
    public bool Closed { get; set; }
}
