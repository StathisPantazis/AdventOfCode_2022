using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_21 : AoCBaseDay<long, long, string[]>
{
    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.FileReadText(FileDescription(this, resourceType))
            .Replace(": ", ",")
            .Replace(" ", ",");

        var input = Helpers.TextCleanReadLines(text);

        return Solution(input);
    }

    protected override long Part1(string[] input)
    {
        var monkeys = GetMonkeys(input);

        while (monkeys.Any(x => x.Number == 0))
        {
            foreach (var monkey in monkeys.Where(x => x.Monkeys.Count > 0 && x.Monkeys.All(y => y.Number != 0)))
            {
                monkey.CalculateNumber();
            }
        }

        return monkeys.First(x => x.Name == "root").Number;
    }

    protected override long Part2(string[] input)
    {
        var monkeys = GetMonkeys(input);
        var root = monkeys.First(x => x.Name == "root");
        monkeys.RemoveAt(0);
        var result1 = root.Monkeys[0];
        var result2 = root.Monkeys[1];

        var humn = monkeys.First(x => x.Name == "humn");
        humn.Number = 1;
        long numTest = 0;
        var initState = monkeys.Where(x => x.Number > 0).ToDictionary(x => x, x => x.Number);
        var plus = 100000000000;

        while (result1.Number == 0 || result1.Number != result2.Number)
        {
            var d = result1.Number - result2.Number;
            if (d < 0)
            {
                numTest -= plus;
                plus /= 10;
            }
            numTest += plus;
            monkeys.ForEach(x => x.Calculated = false);

            // Reset
            foreach
                (var key in initState.Keys)
            {
                key.Number = initState[key];
                key.Calculated = true;
            }

            humn.Number = numTest;

            // Search
            while (monkeys.Any(x => !x.Calculated))
            {
                foreach (var monkey in monkeys.Where(x => !x.Calculated))
                {
                    if (monkey.Monkeys[0].Calculated && monkey.Monkeys[1].Calculated)
                    {
                        if (monkey.Monkeys[0].Number == 0 && monkey.Monkeys[1].Number == 0)
                        {
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

    private static List<Monkey> GetMonkeys(string[] input)
    {
        var monkeys = input.Select(x => x.Split(',') is string[] arr ?
            arr.Length == 2 ? new Monkey(arr[0], long.Parse(arr[1])) : new Monkey(arr[0], arr[1], arr[2], arr[3]) : null).ToList();

        foreach (var monkey in monkeys)
        {
            monkey.Names.ForEach(n => monkey.Monkeys.Add(monkeys.First(x => x.Name == n)));
        }

        return monkeys;
    }

    private class Monkey
    {
        public Monkey(string name, long number)
        {
            Name = name;
            Number = number;
            Calculated = true;
        }

        public Monkey(string name, string name1, string function, string name2)
        {
            Name = name;
            FunctionSymbol = function;
            Function = function is "+" ? Function.Add : function is "-" ? Function.Sub : function is "*" ? Function.Mul : Function.Div;
            Names = [name1, name2];
        }

        public string Name { get; set; }

        public Function Function { get; set; }

        public string FunctionSymbol { get; set; }

        public long Number { get; set; }

        public bool Calculated { get; set; }

        public List<string> Names { get; set; } = new();

        public List<Monkey> Monkeys { get; set; } = new();

        public void CalculateNumber()
        {
            var num1 = Monkeys[0].Number;
            var num2 = Monkeys[1].Number;

            Number = Function switch
            {
                Function.Add => num1 + num2,
                Function.Sub => num1 - num2,
                Function.Mul => num1 * num2,
                Function.Div => num1 / num2,
            };

            Calculated = true;
        }

        public override string ToString() => $"{Name}: {Number}{(Names.Count == 0 ? "" : $"   :||: {Names[0]} {FunctionSymbol} {Names[1]}")}";
    }

    private enum Function { Add, Sub, Mul, Div };
}
