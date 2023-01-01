namespace AdventOfCode_2022.Models.Interfaces;

public interface INode
{
    INode Parent { get; set; }
    List<INode> Children { get; set; }
    bool Closed { get; set; }
}
