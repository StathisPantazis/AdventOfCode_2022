using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2022.Day_11;

namespace AdventOfCode.Version2022;

public class Day_11 : AoCBaseDay<int, ulong, List<Monkey>>
{
    public override AoCSolution<int, ulong> Solve(AoCResourceType resourceType)
    {
        var monkeys = Helpers.FileReadText(FileDescription(this, resourceType))
            .Replace("old * old", "^ 0")
            .Split("Monkey", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new Monkey(x))
            .ToList();

        return Solution(monkeys);
    }

    protected override int Part1(List<Monkey> monkeys)
    {
        ListExtensions.ForNTimesDo(20, () =>
        {
            monkeys.ForEach(monkey =>
                new List<ulong>(monkey.Items).ForEach(item =>
                {
                    var worry = monkey.CalculateWorry(item) / 3;
                    var throwTo = worry % monkey.DivisibleBy == 0 ? monkey.MonkeyIfTrue : monkey.MonkeyIfFalse;
                    monkeys.FirstOrDefault(x => x.Index == throwTo)!.Items.Add(monkey.Items.PopChange(worry));
                }));
        });

        return monkeys.Max(x => x.TimesInspected) * monkeys.NMax(2, x => x.TimesInspected);
    }

    protected override ulong Part2(List<Monkey> monkeys)
    {
        ulong commonMultiple = 1;
        monkeys.ForEachDo(x => commonMultiple *= x.DivisibleBy);

        ListExtensions.ForNTimesDo(10000, (int i) =>
        {
            monkeys.ForEach(monkey =>
                new List<ulong>(monkey.Items).ForEach(item =>
                {
                    var worry = monkey.CalculateWorry(item) % commonMultiple;
                    var throwTo = worry % monkey.DivisibleBy == 0 ? monkey.MonkeyIfTrue : monkey.MonkeyIfFalse;
                    monkeys.FirstOrDefault(x => x.Index == throwTo)!.Items.Add(monkey.Items.PopChange(worry));
                }));
        });

        return (ulong)monkeys.Max(x => x.TimesInspected) * (ulong)monkeys.NMax(2, x => x.TimesInspected);
    }

    public class Monkey
    {
        public Monkey(string text)
        {
            var instructions = Helpers.TextCleanReadLines(text);

            Index = int.Parse(instructions[0].Replace(":", ""));
            Items = instructions[1].Replace("Starting items: ", "").Split(", ").Select(x => ulong.Parse(x)).ToList();
            Operation = (instructions[2].Split(' ')[^2], ulong.Parse(instructions[2].Split(' ').Last()));
            DivisibleBy = ulong.Parse(instructions[3].Split(' ').Last());
            MonkeyIfTrue = int.Parse(instructions[4].Split(' ').Last());
            MonkeyIfFalse = int.Parse(instructions[5].Split(' ').Last());
        }

        public int Index { get; set; }

        public List<ulong> Items { get; set; }

        public (string symbol, ulong value) Operation { get; set; }

        public ulong DivisibleBy { get; set; }

        public int MonkeyIfTrue { get; set; }

        public int MonkeyIfFalse { get; set; }

        public int TimesInspected { get; set; }

        public ulong CalculateWorry(ulong item)
        {
            TimesInspected++;

            return Operation.symbol switch
            {
                "+" => item + Operation.value,
                "-" => item - Operation.value,
                "*" => item * Operation.value,
                "/" => item / Operation.value,
                "^" => item * item,
                _ => throw new Exception("What sort of sorcery is this")
            };
        }

        public override string ToString() => $"Monkey {Index}: {string.Join(", ", Items)}";
    }
}
