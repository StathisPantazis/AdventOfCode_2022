using AdventOfCode_2022.Utils;
using Force.DeepCloner;
using static AdventOfCode_2022.Day_19;

namespace AdventOfCode_2022;

internal static class Test {
    public static void Main() {
        IterationData data = new();

        Node a = new("A", 1);
        Node b = new("B", 2);
        Node c = new("C", 3);
        Node d = new("D", 4);
        Node e = new("E", 5);
        Node f = new("F", 6);
        Node g = new("G", 7);

        a.Neighbours.Add(b);
        a.Neighbours.Add(c);
        b.Neighbours.Add(d);
        b.Neighbours.Add(f);
        c.Neighbours.Add(a);
        c.Neighbours.Add(d);
        d.Neighbours.Add(b);
        d.Neighbours.Add(c);
        d.Neighbours.Add(e);
        e.Neighbours.Add(d);
        e.Neighbours.Add(g);
        f.Neighbours.Add(b);
        g.Neighbours.Add(e);

        List<Node> getNeighbours(Node node) => node.Neighbours;
        void visitNode(Node node) { }
        void closeNode(Node node, IterationData data) { }
        bool shouldContinue(Node node) => true;

        DFS<Node, IterationData> dfs = new(data, getNeighbours, visitNode, closeNode, shouldContinue);
        Node start = a.DeepClone();

        while (true) {
            dfs.Visit(start);

            while (start is not null) {
                start = dfs.SearchForward();
            }

            if (dfs.SearchBackward() is Node previous) {
                //dfs.VisitedNodes.RemoveAll(x => );
                start = previous;

                if (dfs.Stack.Count == 0) {
                    break;
                }
            }
            else {
                break;
            }
        }

    }

    public class Node : INode {
        public Node(string name, int value) {
            Name = name;
            Cost = value;
        }

        public string Key { get; set; }

        public int Cost { get; set; }

        public string Name { get; set; }

        public List<Node> Neighbours { get; set; } = new();

        public override bool Equals(object? obj) => ((Node)obj).Name == Name;
        public override int GetHashCode() => string.Format(Name).GetHashCode();
        public override string ToString() => Name;
    }
    
    public class Memo { 
        
    }

    public class IterationData {
        public int ShortestPath { get; set; }
    }
}
