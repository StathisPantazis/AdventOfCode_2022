namespace AdventOfCode.Core.Models.Interfaces;

public interface INode
{
    INode Parent { get; set; }
    List<INode> Children { get; set; }
    bool Closed { get; set; }
}
