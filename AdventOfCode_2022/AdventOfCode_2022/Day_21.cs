using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_21 {
    public static void Solve() {
        string[] input = Helpers.Text_CleanReadLines(Helpers.File_ReadText(21).Replace(": ", ",").Replace(" ", ","));

        Console.WriteLine($"Part_1: {Part_1(input)}");
        Console.WriteLine($"Part_2: {Part_2(input)}");
    }

    public static long Part_2(string[] input) {
        List<Monkey> monkeys = GetMonkeys(input);
        Monkey root = monkeys.First(x => x.Name == "root");
        monkeys.RemoveAt(0);
        Monkey result1 = root.Monkeys[0];
        Monkey result2 = root.Monkeys[1];

        Monkey humn = monkeys.First(x => x.Name == "humn");
        humn.Number = 1;
        long numTest = 0;
        Dictionary<Monkey, long> initState = monkeys.Where(x => x.Number > 0).ToDictionary(x => x, x => x.Number);
        long plus = 100000000000;

        while (result1.Number == 0 || result1.Number != result2.Number) {
            long d = result1.Number - result2.Number;
            if (d < 0) {
                numTest -= plus;
                plus /= 10;
            }
            numTest += plus;
            monkeys.ForEach(x => x.Calculated = false);

            // Reset
            foreach
                (Monkey key in initState.Keys) {
                key.Number = initState[key];
                key.Calculated = true;
            }

            humn.Number = numTest;

            // Search
            while (monkeys.Any(x => !x.Calculated)) {
                foreach (Monkey monkey in monkeys.Where(x => !x.Calculated)) {
                    if (monkey.Monkeys[0].Calculated && monkey.Monkeys[1].Calculated) {
                        if (monkey.Monkeys[0].Number == 0 && monkey.Monkeys[1].Number == 0) {
                            monkeys.ForEach(x => x.Calculated = true);
                            break;
                        }
                        monkey.CalculateNumber();
                    } 
                }
            }
        }

        return humn.Number;
    }

    public static long Part_1(string[] input) {
        List<Monkey> monkeys = GetMonkeys(input);

        while (monkeys.Any(x => x.Number == 0)) {
            foreach (Monkey monkey in monkeys.Where(x => x.Monkeys.Count > 0 && x.Monkeys.All(y => y.Number != 0))) {
                monkey.CalculateNumber();
            }
        }

        return monkeys.First(x => x.Name == "root").Number;
    }

    public static List<Monkey> GetMonkeys(string[] input) {
        List<Monkey> monkeys = input.Select(x => x.Split(',') is string[] arr ?
            (arr.Length == 2 ? new Monkey(arr[0], long.Parse(arr[1])) : new Monkey(arr[0], arr[1], arr[2], arr[3])) : null).ToList();

        foreach (Monkey monkey in monkeys) {
            monkey.Names.ForEach(n => monkey.Monkeys.Add(monkeys.First(x => x.Name == n)));
        }

        return monkeys;
    }

    public class Monkey {
        public Monkey(string name, long number) {
            Name = name;
            Number = number;
            Calculated = true;
        }

        public Monkey(string name, string name1, string function, string name2) {
            Name = name;
            FunctionSymbol = function;
            Function = function is "+" ? Function.Add : function is "-" ? Function.Sub : function is "*" ? Function.Mul : Function.Div;
            Names = new List<string>() { name1, name2 };
        }

        public string Name { get; set; }

        public Function Function { get; set; }

        public string FunctionSymbol { get; set; }

        public long Number { get; set; }

        public bool Calculated { get; set; }

        public List<string> Names { get; set; } = new();

        public List<Monkey> Monkeys { get; set; } = new();

        public void CalculateNumber() {
            long num1 = Monkeys[0].Number;
            long num2 = Monkeys[1].Number;

            Number = Function switch {
                Function.Add => num1 + num2,
                Function.Sub => num1 - num2,
                Function.Mul => num1 * num2,
                Function.Div => num1 / num2,
            };

            Calculated = true;
        }

        public override string ToString() => $"{Name}: {Number}{(Names.Count == 0 ? "" : $"   :||: {Names[0]} {FunctionSymbol} {Names[1]}")}";
    }

    public enum Function { Add, Sub, Mul, Div };
}
