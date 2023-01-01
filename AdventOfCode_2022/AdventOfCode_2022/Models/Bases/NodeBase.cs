using AdventOfCode_2022.Models.Interfaces;

namespace AdventOfCode_2022.Models.Bases;

public abstract class NodeBase : INode
{
    public INode Parent { get; set; }
    public List<INode> Children { get; set; } = new();
    public bool Closed { get; set; }
}
