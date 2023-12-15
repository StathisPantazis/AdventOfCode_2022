using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2022.Day_19;

namespace AdventOfCode.Version2022;

public class Day_19 : AoCBaseDay<int, int, List<Blueprint>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.FileCleanReadText(FileDescription(this, resourceType))
            .Replace("Blueprint ", "").Replace(": Each ore robot costs ", ",").Replace(" ore. Each clay robot costs ", ",").Replace(" ore. Each obsidian robot costs ", ",")
            .Replace(" ore and ", ",").Replace(" clay. Each geode robot costs ", ",").Replace(" obsidian.", "");

        var blueprints = Helpers.TextCleanReadLines(text)
            .Select(x => x.Split(','))
            .Select(x => new Blueprint(int.Parse(x[0].ToString()), int.Parse(x[1].ToString()), int.Parse(x[2].ToString()), int.Parse(x[3].ToString()), int.Parse(x[4].ToString()), int.Parse(x[5].ToString()), int.Parse(x[6].ToString())))
            .ToList();

        return Solution(blueprints);
    }

    protected override int Part1(List<Blueprint> args)
    {
        var quality = 0;

        for (var i = 0; i < args.Count; i++)
        {
            var blueprint = args[i];
            quality += GetMaxGeodes(blueprint, 24) * blueprint.Index;
        }

        return quality;
    }

    protected override int Part2(List<Blueprint> args)
    {
        List<int> results = new();

        for (var i = 0; i < 3; i++)
        {
            var blueprint = args[i];
            results.Add(GetMaxGeodes(blueprint, 32));
        }

        return results[0] * results[1] * results[2];
    }

    private static int GetMaxGeodes(Blueprint blueprint, int lastMinute)
    {
        var minute_maxGeodes = Enumerable.Range(17, 16).ToDictionary(x => x, x => 0);

        Dictionary<Mineral, int> money = new() {
            { Mineral.Ore, 1 },
            { Mineral.Clay, 0 },
            { Mineral.Obsidian, 0 },
            { Mineral.Geode, 0 }
        };

        Dictionary<Mineral, int> robots = new() {
            { Mineral.Ore, 1 },
            { Mineral.Clay, 0 },
            { Mineral.Obsidian, 0 },
            { Mineral.Geode, 0 }
        };

        RobotFactory factory = new(blueprint);

        List<Node> getNeighbours(Node node)
        {
            var decisions = new List<Decision>();
            var nodes = new List<Node>();
            if (factory.CanProduce(Mineral.Geode, node.Money))
            {
                nodes.Add(node.GetNextNode(Decision.BuyGeode, factory));
                return nodes;
            }

            if (factory.CanProduce(Mineral.Obsidian, node.Money))
            {
                nodes.Add(node.GetNextNode(Decision.BuyObsidian, factory));
                decisions.Add(Decision.BuyObsidian);
            }
            if (factory.CanProduce(Mineral.Clay, node.Money))
            {
                nodes.Add(node.GetNextNode(Decision.BuyClay, factory));
                decisions.Add(Decision.BuyClay);
            }
            if (factory.CanProduce(Mineral.Ore, node.Money))
            {
                nodes.Add(node.GetNextNode(Decision.BuyOre, factory));
                decisions.Add(Decision.BuyOre);
            }

            var noBuyNode = node.GetNextNode(Decision.NoBuy, factory);
            if (nodes.Count == 0)
            {
                nodes.Add(noBuyNode);
            }
            else
            {
                var noBuyAfterNoBuy = noBuyNode.GetNextNode(Decision.NoBuy, factory);
                var newDecisions = new List<Decision>();

                if (factory.CanProduce(Mineral.Obsidian, noBuyAfterNoBuy.Money))
                {
                    newDecisions.Add(Decision.BuyObsidian);
                }
                if (factory.CanProduce(Mineral.Clay, noBuyAfterNoBuy.Money))
                {
                    newDecisions.Add(Decision.BuyClay);
                }
                if (factory.CanProduce(Mineral.Ore, noBuyAfterNoBuy.Money))
                {
                    newDecisions.Add(Decision.BuyOre);
                }

                if (nodes.Count == 0 || newDecisions.Count > decisions.Count)
                {
                    nodes.Add(noBuyNode);
                }
            }

            return nodes;
        };
        bool shouldCloseNode(Node node) => node.Minute == lastMinute;
        void closeParent(Node node) => node.MaxGeodes = node.Money[Mineral.Geode];
        void closeChildren(Node parent) => parent.MaxGeodes = parent.Children.Max(x => ((Node)x).MaxGeodes);
        void visitNode(Node node)
        {
            if (minute_maxGeodes.TryGetValue(node.Minute, out var geodeMax) && geodeMax < node.Robots[Mineral.Geode])
            {
                minute_maxGeodes[node.Minute] = node.Robots[Mineral.Geode];
            }
        }
        bool prune(Node node) => node.ConsecutiveNoBuys > 4
            || minute_maxGeodes.TryGetValue(node.Minute, out var geodeMax) && geodeMax > node.Robots[Mineral.Geode] + 1
            || node.Robots[Mineral.Ore] > blueprint.MaxOreRobots
            || node.Robots[Mineral.Clay] > blueprint.MaxClayRobots
            || node.Robots[Mineral.Obsidian] > blueprint.MaxObsidianRobots;

        DFS<Node> dfs = new(getNeighbours, shouldCloseNode, visitNode, closeParent, closeChildren, prune);
        Node start = new(1, robots, money);
        dfs.Search(start);
        return dfs.Visited.Max(x => x.MaxGeodes);
    }

    private class Node(int minute, Dictionary<Mineral, int> robots, Dictionary<Mineral, int> money) : NodeBase
    {
        public int Minute { get; set; } = minute;
        public Dictionary<Mineral, int> Robots { get; set; } = robots;
        public Dictionary<Mineral, int> Money { get; set; } = money;
        public int MaxGeodes { get; set; }
        public int ConsecutiveNoBuys { get; set; }

        public Node GetNextNode(Decision decision, RobotFactory factory)
        {
            Node node = new(Minute + 1, new(Robots), new(Money))
            {
                ConsecutiveNoBuys = decision is Decision.NoBuy ? ConsecutiveNoBuys + 1 : 0
            };

            node.Money[Mineral.Geode] += node.Robots[Mineral.Geode];
            node.Money[Mineral.Obsidian] += node.Robots[Mineral.Obsidian];
            node.Money[Mineral.Clay] += node.Robots[Mineral.Clay];
            node.Money[Mineral.Ore] += node.Robots[Mineral.Ore];

            if (decision is Decision.BuyGeode)
            {
                factory.Produce(Mineral.Geode, node.Money);
                node.Robots[Mineral.Geode] += 1;
            }
            else if (decision is Decision.BuyObsidian)
            {
                factory.Produce(Mineral.Obsidian, node.Money);
                node.Robots[Mineral.Obsidian] += 1;
            }
            else if (decision is Decision.BuyClay)
            {
                factory.Produce(Mineral.Clay, node.Money);
                node.Robots[Mineral.Clay] += 1;
            }
            else if (decision is Decision.BuyOre)
            {
                factory.Produce(Mineral.Ore, node.Money);
                node.Robots[Mineral.Ore] += 1;
            }

            return node;
        }

        public override bool Equals(object? obj)
        {
            var other = (Node)obj;
            return other.Minute == Minute
                && other.Robots[Mineral.Ore] == Robots[Mineral.Ore]
                && other.Robots[Mineral.Clay] == Robots[Mineral.Clay]
                && other.Robots[Mineral.Obsidian] == Robots[Mineral.Obsidian]
                && other.Robots[Mineral.Geode] == Robots[Mineral.Geode]
                && other.Money[Mineral.Ore] == Money[Mineral.Ore]
                && other.Money[Mineral.Clay] == Money[Mineral.Clay]
                && other.Money[Mineral.Obsidian] == Money[Mineral.Obsidian]
                && other.Money[Mineral.Geode] == Money[Mineral.Geode];
        }
        public override int GetHashCode() => $"{Minute}{Robots[Mineral.Ore]}{Robots[Mineral.Clay]}{Robots[Mineral.Obsidian]}{Robots[Mineral.Geode]}{Money[Mineral.Ore]}{Money[Mineral.Clay]}{Money[Mineral.Obsidian]}{Money[Mineral.Geode]}".GetHashCode();
        public override string ToString() => $"{Minute}";
    }

    private class RobotFactory(Blueprint blueprint)
    {
        public Blueprint Blueprint { get; set; } = blueprint;

        public bool CanProduce(Mineral robotType, Dictionary<Mineral, int> money)
        {
            return robotType switch
            {
                Mineral.Ore => money[Mineral.Ore] >= Blueprint.OreRobotCost_Ore,
                Mineral.Clay => money[Mineral.Ore] >= Blueprint.ClayRobotCost_Ore,
                Mineral.Obsidian => money[Mineral.Ore] >= Blueprint.ObsidianRobotCost_Ore && money[Mineral.Clay] >= Blueprint.ObsidianRobotCost_Clay,
                Mineral.Geode => money[Mineral.Ore] >= Blueprint.GeodeRobotCost_Ore && money[Mineral.Obsidian] >= Blueprint.GeodeRobotCost_Obsidian,
            };
        }

        public void Produce(Mineral robotType, Dictionary<Mineral, int> money)
        {
            if (robotType is Mineral.Ore)
            {
                money[Mineral.Ore] -= Blueprint.OreRobotCost_Ore;
            }
            else if (robotType is Mineral.Clay)
            {
                money[Mineral.Ore] -= Blueprint.ClayRobotCost_Ore;
            }
            else if (robotType is Mineral.Obsidian)
            {
                money[Mineral.Ore] -= Blueprint.ObsidianRobotCost_Ore;
                money[Mineral.Clay] -= Blueprint.ObsidianRobotCost_Clay;
            }
            else if (robotType is Mineral.Geode)
            {
                money[Mineral.Ore] -= Blueprint.GeodeRobotCost_Ore;
                money[Mineral.Obsidian] -= Blueprint.GeodeRobotCost_Obsidian;
            }
        }
    }

    public class Blueprint
    {
        public Blueprint(int index, int oreRobotCost_Ore, int clayRobotCost_Ore, int obsidianRobotCost_Ore, int obsidianRobotCost_Clay, int geodeRobotOre_Ore, int geodeRobotOre_Obsidian)
        {
            Index = index;
            OreRobotCost_Ore = oreRobotCost_Ore;
            ClayRobotCost_Ore = clayRobotCost_Ore;
            ObsidianRobotCost_Ore = obsidianRobotCost_Ore;
            ObsidianRobotCost_Clay = obsidianRobotCost_Clay;
            GeodeRobotCost_Ore = geodeRobotOre_Ore;
            GeodeRobotCost_Obsidian = geodeRobotOre_Obsidian;
            MaxOreRobots = new int[] { OreRobotCost_Ore, ClayRobotCost_Ore, ObsidianRobotCost_Ore, GeodeRobotCost_Ore }.Max();
            MaxClayRobots = obsidianRobotCost_Clay;
            MaxObsidianRobots = GeodeRobotCost_Obsidian;
        }
        public int Index { get; set; }
        public int OreRobotCost_Ore { get; set; }
        public int ClayRobotCost_Ore { get; set; }
        public int ObsidianRobotCost_Ore { get; set; }
        public int ObsidianRobotCost_Clay { get; set; }
        public int GeodeRobotCost_Ore { get; set; }
        public int GeodeRobotCost_Obsidian { get; set; }
        public int MaxOreRobots { get; set; }
        public int MaxClayRobots { get; set; }
        public int MaxObsidianRobots { get; set; }
    }

    private enum Mineral { Ore, Clay, Obsidian, Geode };

    private enum Decision { BuyOre, BuyClay, BuyObsidian, BuyGeode, NoBuy };
}
