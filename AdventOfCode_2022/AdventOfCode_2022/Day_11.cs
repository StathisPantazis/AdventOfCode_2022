using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Utils;
using System.Numerics;

namespace AdventOfCode_2022;

internal static class Day_11 {
    private static List<Monkey> GetMonkeys() => Helpers.File_ReadText(11).Replace("old * old", "^ 0")
            .Split("Monkey", StringSplitOptions.RemoveEmptyEntries).Select(x => new Monkey(x)).ToList();

    public static void Solve() {
        Console.WriteLine($"Part1: {Part_1()}");
        Console.WriteLine($"Part2: {Part_2()}");
    }

    public static ulong Part_2() {
        List<Monkey> monkeys = GetMonkeys();

        ulong commonMultiple = 1;
        monkeys.ForEachDo(x => commonMultiple *= x.DivisibleBy);

        ListExtensions.ForNTimesDo(10000, (int i) => {
            var lasala = monkeys.Select(x => x.TimesInspected).ToList();
            if (i is 20 or 21) {
                var lala = "";
            }
            monkeys.ForEach(monkey =>
                new List<ulong>(monkey.Items).ForEach(item => {
                    ulong worry = monkey.CalculateWorry(item) % commonMultiple;
                    int throwTo = worry % monkey.DivisibleBy == 0 ? monkey.MonkeyIfTrue : monkey.MonkeyIfFalse;
                    monkeys.FirstOrDefault(x => x.Index == throwTo).Items.Add(monkey.Items.PopChange(worry));
                }));
        });

        return (ulong)monkeys.Max(x => x.TimesInspected) * (ulong)monkeys.NMax(2, x => x.TimesInspected);
    }

    public static int Part_1() {
        List<Monkey> monkeys = GetMonkeys();

        ListExtensions.ForNTimesDo(20, () => {
            monkeys.ForEach(monkey =>
                new List<ulong>(monkey.Items).ForEach(item => {
                    ulong worry = monkey.CalculateWorry(item) / 3;
                    int throwTo = worry % monkey.DivisibleBy == 0 ? monkey.MonkeyIfTrue : monkey.MonkeyIfFalse;
                    monkeys.FirstOrDefault(x => x.Index == throwTo).Items.Add(monkey.Items.PopChange(worry));
                }));
        });

        return monkeys.Max(x => x.TimesInspected) * monkeys.NMax(2, x => x.TimesInspected);
    }

    public class Monkey {
        public Monkey(string text) {
            string[] instructions = Helpers.Text_CleanReadLines(text);

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

        public ulong CalculateWorry(ulong item) {
            TimesInspected++;

            return Operation.symbol switch {
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
