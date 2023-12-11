using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.Extensions;

public static class NodeExtensions
{
    public static void AddChild(this NodeBase node, NodeBase child) => node.Children.Add(child);
    public static void SetParent(this NodeBase node, NodeBase parent) => node.Parent = parent;
}
