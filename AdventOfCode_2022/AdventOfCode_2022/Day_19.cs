using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_19 {

    public static void Solve() {
        string text = Helpers.File_CleanReadText(19)
            .Replace("Blueprint ", "").Replace(": Each ore robot costs ", ",").Replace(" ore. Each clay robot costs ", ",").Replace(" ore. Each obsidian robot costs ", ",")
            .Replace(" ore and ", ",").Replace(" clay. Each geode robot costs ", ",").Replace(" obsidian.", "");

        List<Blueprint> blueprints = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split(','))
            .Select(x => new Blueprint(int.Parse(x[0].ToString()), int.Parse(x[1].ToString()), int.Parse(x[2].ToString()), int.Parse(x[3].ToString()), int.Parse(x[4].ToString()), int.Parse(x[5].ToString()), int.Parse(x[6].ToString())))
            .ToList();

        Dictionary<Mineral, int> money = new() {
            { Mineral.Ore, 0 },
            { Mineral.Clay, 0 },
            { Mineral.Obsidian, 0 },
            { Mineral.Geode, 0 }
        };

        List<Robot> robots = new() { new Robot(Mineral.Ore) };
        List<Robot> theRobots = new();
        Blueprint blueprint = blueprints[0];
        RobotFactory factory = new(blueprint);
        IterationData iterationData = new();

        List<Node> getNeighbours(Node node) {
            List<Node> nodes = new();
            if (factory.CanProduce(Mineral.Geode, node.Money)) {
                nodes.Add(node.GetNextNode(Decision.BuyGeode));
                return nodes;
            }

            if (factory.CanProduce(Mineral.Obsidian, node.Money)) {
                nodes.Add(node.GetNextNode(Decision.BuyObsidian));
            }
            if (factory.CanProduce(Mineral.Clay, node.Money)) {
                nodes.Add(node.GetNextNode(Decision.BuyClay));
            }
            if (factory.CanProduce(Mineral.Ore, node.Money)) {
                nodes.Add(node.GetNextNode(Decision.BuyOre));
            }
            nodes.Add(node.GetNextNode(Decision.NoBuy));
            return nodes;
        };
        void visitNode(Node node) {
            Robot newRobot = null;

            if (node.Decision is Decision.BuyGeode) {
                newRobot = factory.Produce(Mineral.Geode, node.Money);
            }
            else if (node.Decision is Decision.BuyObsidian) {
                newRobot = factory.Produce(Mineral.Obsidian, node.Money);
            }
            else if (node.Decision is Decision.BuyClay) {
                newRobot = factory.Produce(Mineral.Clay, node.Money);
            }
            else if (node.Decision is Decision.BuyOre) {
                newRobot = factory.Produce(Mineral.Ore, node.Money);
            }

            node.Robots.ForEach(r => r.Mine(node.Money));

            if (newRobot is not null) {
                node.Robots.Add(newRobot);
            }
        }
        void closeNodeAction(Node node, IterationData data) {
            if (node.Money[Mineral.Geode] > data.GeodeMax) {
                data.GeodeMax = node.Money[Mineral.Geode];
            }
        }
        bool shouldContinue(Node node) => node.Minute < 24;

        DFS<Node, IterationData> dfs = new(iterationData, getNeighbours, visitNode, closeNodeAction, shouldContinue);
        Node start = new(1, robots, money, Decision.NoBuy, string.Empty);

        while (true) {
            dfs.Visit(start);

            while (start is not null) {
                start = dfs.SearchForward();
            }

            if (dfs.SearchBackward() is Node previous) {
                //dfs.VisitedNodes.RemoveAll(x => x.StartsWith(previous.Key[..^1]) && x.Length > previous.Key.Length);
                start = previous;

                if (dfs.Stack.Count == 0) {
                    break;
                }
            }
            else {
                break;
            }
        }

        Console.WriteLine();
    }

    public class IterationData {
        public int GeodeMax { get; set; }
    }

    public class Node : INode {
        public Node(int minute, List<Robot> robots, Dictionary<Mineral, int> money, Decision decision, string chain) {
            Minute = minute;
            Robots = new(robots);
            Money = new(money);
            Decision = decision;
            Key = $"{chain}{(int)decision}";
        }

        public string Key { get; set; }
        public int Minute { get; set; }
        public List<Robot> Robots { get; set; }
        public Dictionary<Mineral, int> RobotDict { get; set; }
        public Dictionary<Mineral, int> Money { get; set; }
        public Decision Decision { get; set; }
        public Node GetNextNode(Decision decision) => new(Minute + 1, Robots, Money, decision, Key);
        public override string ToString() => $"{Minute}' : {Decision}";
        public override bool Equals(object? obj) => ((Node)obj).Minute == Minute && ((Node)obj).Decision == Decision;
        public override int GetHashCode() => string.Format($"{Minute}{Decision}").GetHashCode();
    }

    public class Memo {
        public int Minute { get; set; }
        public Dictionary<Mineral, int> Robots { get; set; }
        public Dictionary<Mineral, int> Money { get; set; }
        public int MaxValue { get; set; }

        public override bool Equals(object? obj) {
            Memo other = (Memo)obj;
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
        public override int GetHashCode() => string.Format($"{Minute}{Robots[Mineral.Ore]}{Money[Mineral.Ore]}").GetHashCode();
    }

    public class Robot {
        public Robot(Mineral robotType) {
            Type = robotType;
        }

        public Mineral Type { get; set; }

        public void Mine(Dictionary<Mineral, int> money) {
            if (Type is Mineral.Ore) {
                money[Mineral.Ore] += 1;
            }
            else if (Type is Mineral.Clay) {
                money[Mineral.Clay] += 1;
            }
            else if (Type is Mineral.Obsidian) {
                money[Mineral.Obsidian] += 1;
            }
            else if (Type is Mineral.Geode) {
                money[Mineral.Geode] += 1;
            }
        }

        public override string ToString() => Type.ToString();
    }

    public class RobotFactory {
        public RobotFactory(Blueprint blueprint) {
            Blueprint = blueprint;
        }

        public Blueprint Blueprint { get; set; }

        public bool CanProduce(Mineral robotType, Dictionary<Mineral, int> money) {
            return robotType switch {
                Mineral.Ore => money[Mineral.Ore] >= Blueprint.OreRobotCost_Ore,
                Mineral.Clay => money[Mineral.Ore] >= Blueprint.ClayRobotCost_Ore,
                Mineral.Obsidian => money[Mineral.Ore] >= Blueprint.ObsidianRobotCost_Ore && money[Mineral.Clay] >= Blueprint.ObsidianRobotCost_Clay,
                Mineral.Geode => money[Mineral.Ore] >= Blueprint.GeodeRobotOre_Ore && money[Mineral.Obsidian] >= Blueprint.GeodeRobotOre_Obsidian,
            };
        }

        public Robot Produce(Mineral robotType, Dictionary<Mineral, int> money) {
            if (robotType is Mineral.Ore) {
                money[Mineral.Ore] -= Blueprint.OreRobotCost_Ore;
                return new Robot(Mineral.Ore);
            }
            else if (robotType is Mineral.Clay) {
                money[Mineral.Ore] -= Blueprint.ClayRobotCost_Ore;
                return new Robot(Mineral.Clay);
            }
            else if (robotType is Mineral.Obsidian) {
                money[Mineral.Ore] -= Blueprint.ObsidianRobotCost_Ore;
                money[Mineral.Clay] -= Blueprint.ObsidianRobotCost_Clay;
                return new Robot(Mineral.Obsidian);
            }
            else if (robotType is Mineral.Geode) {
                money[Mineral.Ore] -= Blueprint.GeodeRobotOre_Ore;
                money[Mineral.Obsidian] -= Blueprint.GeodeRobotOre_Obsidian;
                return new Robot(Mineral.Geode);
            }
            return null;
        }
    }

    public class Blueprint {
        public Blueprint(int index, int oreRobotCost_Ore, int clayRobotCost_Ore, int obsidianRobotCost_Ore, int obsidianRobotCost_Clay, int geodeRobotOre_Ore, int geodeRobotOre_Obsidian) {
            Index = index;
            OreRobotCost_Ore = oreRobotCost_Ore;
            ClayRobotCost_Ore = clayRobotCost_Ore;
            ObsidianRobotCost_Ore = obsidianRobotCost_Ore;
            ObsidianRobotCost_Clay = obsidianRobotCost_Clay;
            GeodeRobotOre_Ore = geodeRobotOre_Ore;
            GeodeRobotOre_Obsidian = geodeRobotOre_Obsidian;
        }
        public int Index { get; set; }
        public int OreRobotCost_Ore { get; set; }
        public int ClayRobotCost_Ore { get; set; }
        public int ObsidianRobotCost_Ore { get; set; }
        public int ObsidianRobotCost_Clay { get; set; }
        public int GeodeRobotOre_Ore { get; set; }
        public int GeodeRobotOre_Obsidian { get; set; }
    }

    public enum Mineral { Ore, Clay, Obsidian, Geode };

    public enum Decision { BuyOre, BuyClay, BuyObsidian, BuyGeode, NoBuy };
}
