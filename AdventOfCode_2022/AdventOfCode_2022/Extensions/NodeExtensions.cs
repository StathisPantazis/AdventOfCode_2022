using AdventOfCode_2022.Models.Interfaces;

namespace AdventOfCode_2022.Extensions;

public static class NodeExtensions
{
    public static void AddChild(this INode node, INode child) => node.Children.Add(child);
    public static void SetParent(this INode node, INode parent) => node.Parent = parent;
}
