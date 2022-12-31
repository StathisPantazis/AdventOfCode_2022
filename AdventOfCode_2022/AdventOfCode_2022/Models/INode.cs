namespace AdventOfCode_2022.Models;

public interface INode
{
    void AddChild(INode child);
    void SetParent(INode parent);
}
