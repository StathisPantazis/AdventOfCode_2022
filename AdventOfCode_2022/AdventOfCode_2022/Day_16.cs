using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Utils;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode_2022;

internal static class Day_16 {
    public static void Solve() {
        string text = Helpers.File_ReadText(16)
            .Replace("valves", "valve").Replace("leads", "lead").Replace("tunnels", "tunnel")
            .Replace(" has flow rate=", ",").Replace("; tunnel lead to valve ", ",").Replace("Valve ", "").Replace(" ", "");

        List<Valve> valves = BuildValves(text);
        List<FromToPaths> combinations = BuildValveCombinations(valves);

        List<Valve> getNextNodes(Valve valve) => valve.Valves;
        void markNode(Valve valve) => valve.IsOpen = true;
        void resetNode(Valve valve) => valve.IsOpen = false;
        bool searchForwardFilter(Valve valve) => valve.Flow > 0;

        //Tracker<Valve> tracker = new(getNextNodes, markNode, resetNode, searchForwardFilter);
        //FillPathsBetter(tracker, valves, combinations);


        int maxPressure = 0;
        int maxOutputPerMinute = valves.Sum(x => x.Flow);

        foreach (Valve valve in valves) {
            valves.ForEach(x => x.IsOpen = x.Flow == 0);
            int openValves = valves.Count(x => x.IsOpen);
            int remainingValves = valves.Count - openValves;
            int minutes = 0;
            int pressure = 0;

            while (minutes < 30) {
                minutes++;

                if (remainingValves == 0) {
                    pressure += maxOutputPerMinute;
                }
                else {
                    pressure += valves.Where(x => x.IsOpen).Sum(x => x.Flow);
                }
            }

            if (pressure > maxPressure) {
                maxPressure = pressure;
            }
        }


        var lala = "";
    }

    public static void FillPathsBetter(Tracker<Valve> tracker, List<Valve> valves, List<FromToPaths> combinations) {


        foreach (Valve valve in valves) {
            if (valve.Flow == 0) {
                continue;
            }
            valves.ForEach(x => x.IsOpen = x.Flow == 0);

            tracker.Start(valve);
            tracker.ExhaustRoute();
        }
    }

    public static List<PossiblePath> FindPathsBetweenValves(List<Valve> valves, FromToPaths combination) {
        List<PossiblePath> paths = new();
        Dictionary<Valve, List<string>> visited = valves.ToDictionary(x => x, x => new List<string>());

        for (int i = 0; i < 100; i++) {
            PossiblePath path = new();
            Valve node = combination.From;
            bool end = false;

            for (int j = 0; j < 30; j++) {
                if (combination.To.Equals(node)) {
                    break;
                }
                else if (visited[node].Count == node.CountOfValves || j == 29) {
                    path = null;
                    end = node.Equals(combination.From);
                    break;
                }
                else {
                    string prevNodeName = node.Name;
                    string nextNodeName = node.ValveNames.FirstOrDefault(x => !visited[node].Any(y => x == y) && path.ValveNames.None(y => x == y));
                    visited[node].Add(nextNodeName);
                    node = valves.FirstOrDefault(x => x.Name == nextNodeName);
                    visited[node].Add(prevNodeName);
                    path.Add(node);
                }
            }

            if (path != null) {
                paths.Add(path);
            }
            else if (end) {
                break;
            }
        }

        return paths;
    }

    public static List<FromToPaths> BuildValveCombinations(List<Valve> valves) {
        List<FromToPaths> combinations = new();

        foreach (Valve from in valves) {
            foreach (Valve to in valves.Where(x => x.NotEquals(from))) {
                FromToPaths combination = new(from, to);
                combination.Paths.AddRange(FindPathsBetweenValves(valves, combination));
                combinations.Add(combination);
            }
        }

        valves.ForEach(x => x.IsOpen = x.Flow == 0);
        return combinations;
    }

    public static List<Valve> BuildValves(string text) {
        List<Valve> valves = Helpers.Text_CleanReadLines(text)
            .Select(x => new Valve(x.Split(',')[0].ToString(), int.Parse(x.Split(',')[1]), x.Split(',')[2..].ToList()))
            .ToList();

        valves = valves.OrderByDescending(x => x.Flow).ToList();

        foreach (Valve valve in valves) {
            valve.Valves = valves.Where(x => valve.ValveNames.Contains(x.Name)).ToList();
            valve.Valves = valve.Valves.OrderByDescending(x => x.Flow).ToList();
            valve.ValveNames = valve.Valves.Select(x => x.Name).ToList();
            valve.IsOpen = valve.Flow == 0;
        }

        return valves;
    }
}

public class Tracker<T> where T : class {
    private Func<T, List<T>> GetNextNodes { get; set; }
    private Action<T> MarkNodeFunc { get; set; }
    private Action<T> ResetNodeFunc { get; set; }
    private Func<T, bool> SearchForwardFilterFunc { get; set; }
    private Func<T, bool> ValidateConditionFunc { get; set; }

    public Tracker(Func<T, List<T>> getNextNodes, Func<T, bool> validateConditionFunc, Action<T> markNodeFunc = null, Action<T> resetNodeFunc = null, Func<T, bool> searchForwardFilterFunc = null) {
        GetNextNodes = getNextNodes;
        MarkNodeFunc = markNodeFunc;
        ResetNodeFunc = resetNodeFunc;
        SearchForwardFilterFunc = searchForwardFilterFunc;
        ValidateConditionFunc = validateConditionFunc;
    }

    public Route<T> CurrentRoute { get; set; }

    public List<Route<T>> ValidRoutes { get; set; } = new();

    private T CurrentNode => CurrentRoute?.CurrentNode;

    public void Start(T currentNode) {
        CurrentRoute = new Route<T>(currentNode);
        CurrentRoute.NodePath.Add(currentNode);
        MarkNodeFunc(currentNode);
    }

    public T ValidateAndCloseRoute() {
        ValidRoutes.Add(CurrentRoute);
        CurrentRoute = CurrentRoute.Copy();
        return SearchBackward();
    }

    public T CancelRoute() {
        return SearchBackward();
    }

    public void ExhaustRoute() {
        T next = CurrentNode;

        while (next is not null) {
            next = SearchForward(GetNextNodes, MarkNodeFunc);
        }

        //ValidateConditionFunc();

        while (true) {
            if (SearchBackward(ResetNodeFunc) is T checkpoint) {
                CurrentRoute.CurrentNode = checkpoint;
                ExhaustRoute();
            }
            else {
                break;
            }
        }
    }

    public T SearchForward(Func<T, List<T>> getNextNodes, Action<T> markNodeFunc = null) {
        List<T> nextNodes = getNextNodes(CurrentNode);
        if (nextNodes.FirstOrDefault(n => SearchForwardFilterFunc(n) && CurrentRoute.NodePath.Any(c => !c.Equals(n))) is not T nextNode) {
            return default;
        }

        markNodeFunc(nextNode);
        nextNodes.Remove(nextNode);
        nextNodes.ForEach(n => CurrentRoute.RemainingNodes.Add(n));
        return nextNode;
    }

    public T SearchBackward(Action<T> resetNodeFunc = null) {
        if (resetNodeFunc is not null) {
            resetNodeFunc(CurrentNode);
        }

        T node = CurrentRoute.RemainingNodes.FirstOrDefault();

        if (node is not null) {
            CurrentRoute.RemainingNodes.Remove(node);
            CurrentRoute.SearchedNodes.Add(node);
        }

        return node;
    }
}

public class Route<T> {
    public Route(T currentNode) {
        CurrentNode = currentNode;
    }

    public T CurrentNode { get; set; }

    public List<T> NodePath { get; set; } = new();

    public List<T> RemainingNodes { get; set; } = new();

    public List<T> SearchedNodes { get; set; } = new();

    public bool Resolved => RemainingNodes.Count == 0;

    public Route<T> Copy() => new(CurrentNode) { NodePath = new List<T>(NodePath), RemainingNodes = new List<T>(RemainingNodes) };
}

public class PossiblePath {
    public List<Valve> Valves { get; set; } = new();
    public List<string> ValveNames { get; set; } = new();
    public int Distance { get; set; }

    public void Add(Valve valve) {
        Valves.Add(valve);
        ValveNames.Add(valve.Name);
        Distance = Valves.Count;
    }
}

public class FromToPaths {
    public FromToPaths(Valve from, Valve to) {
        From = from;
        To = to;
    }

    public Valve From { get; set; }
    public Valve To { get; set; }
    public List<Valve> Valves { get; set; } = new();
    public List<PossiblePath> Paths { get; set; } = new();
    public PossiblePath FastestPath() => Paths.OrderBy(x => x.Distance).FirstOrDefault();
    public override string ToString() => $"From: {From.Name} - To: {To.Name} --> {Paths.Count} paths";
    public override bool Equals(object? obj) => ((FromToPaths)obj).From.Name == From.Name && ((FromToPaths)obj).To.Name == To.Name;
    public override int GetHashCode() => string.Format($"{From.Name}{To.Name}").GetHashCode();
}

public class Valve {
    public Valve(string name, int flow, List<string> valves) {
        Name = name;
        Flow = flow;
        ValveNames = valves;
        CountOfValves = ValveNames.Count;
    }

    public string Name { get; set; }
    public int Flow { get; set; }
    public bool IsOpen { get; set; }
    public List<string> ValveNames { get; set; }
    public List<Valve> Valves { get; set; } = new();
    public int CountOfValves { get; set; }
    public override string ToString() => $"{Name}: {Flow} --> {string.Join(" , ", ValveNames)}";
    public override bool Equals(object? obj) => ((Valve)obj).Name == Name;
    public override int GetHashCode() => string.Format($"{Name}").GetHashCode();
    public bool Equals(Valve other) => other.Name == Name;
    public bool NotEquals(Valve other) => !Equals(other);
}