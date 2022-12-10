using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_10 {
    public static readonly string _addx = "A";
    public static readonly string _noop = "N";

    public static void Solve() {
        string text = Helpers.File_CleanReadText(10).Replace("addx", _addx).Replace("noop", _noop);
        List<(string instr, int num)> input = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split(' ') is string[] ar ? (ar[0].ToString(), ar.Length > 1 ? int.Parse(ar[1].ToString()) : 0) : default)
            .ToList();

        // Create cycles
        int sum = 1, cycle = 1;
        List<(int cycle, int sum)> cycles = new();

        for (int i = 0; i < input.Count; i++) {
            (string instr, int num) = input[i];

            if (instr == _addx) {
                cycle++;
                cycles.Add((cycle, sum));
            }
            sum += instr == _noop ? 0 : num;
            cycle++;
            cycles.Add((cycle, sum));
        }

        Console.WriteLine($"Part_1: {Part_1(cycles)}");
        Console.WriteLine($"Part_2:\n{Part_2(cycles)}");
    }

    public static string Part_2(List<(int cycle, int sum)> cycles) {
        Grid<string> crt = new(Enumerable.Range(0, 6).Select(i => new string('.', 40)), singleCharacters: true);
        string spriteTemplate = new('.', 40);
        string sprite = spriteTemplate;

        for (int i = 0; i < cycles.Count; i++) {
            int row = (int)Math.Floor((double)(i / 40));
            int col = i % 40;
            crt[row, col] = sprite[col].ToString();

            if (i != cycles.Count - 1 && cycles[i].sum == cycles[i + 1].sum) {
                sprite = string.Concat(string.Join("", spriteTemplate.Take(cycles[i].sum - 1)), "###", spriteTemplate[..Math.Max(0, 38 - cycles[i].sum)]);
            }
        }

        return crt.ToString();
    }

    public static int Part_1(List<(int cycle, int sum)> cycles) {
        int[] signals = new int[] { 20, 60, 100, 140, 180, 220 };
        int signalStrength = 0;
        signals.ForEachDo(s => signalStrength += cycles.FirstOrDefault(x => x.cycle == s) is (int cycle, int sum) ? sum * cycle : 0);
        return signalStrength;
    }
}
