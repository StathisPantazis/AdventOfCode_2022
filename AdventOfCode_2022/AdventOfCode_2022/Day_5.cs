using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_5 {
    public static void Solve() {
        string text = Helpers.FileCleanReadText(5);
        text = text
            .Replace("    [", " ").Replace("]    ", " ").Replace("    ", " ").Replace("[", "").Replace("]", "")
            .Replace("move ", "").Replace(" from ", " ").Replace(" to ", " ");
        string[] lines = Helpers.TextCleanReadLines(text);

        int instructionIndex = lines.ToList().IndexOf(lines.First(y => y.StartsWith(" 1"))) + 2;
        int total = int.Parse(lines[instructionIndex - 2].Trim().Last().ToString());
        int[][] instructions = lines.Skip(instructionIndex).Select(x => x.Split(' ').Select(y => int.Parse(y)).ToArray()).ToArray();

        for (int part = 1; part < 3; part++) {
            List<List<string>> stacks = Enumerable.Range(0, total)
                .Select(i => lines.Take(instructionIndex - 2).Select(x => x.Split(' ')[i]).Where(x => !string.IsNullOrEmpty(x)).ToList())
                .ToList();

            foreach (int[] instr in instructions) {
                List<string> from = stacks[instr[1] - 1];
                List<string> move = part == 1 ? from.Take(instr[0]).ToList() : from.Take(instr[0]).Reverse().ToList();

                for (int i = 0; i < move.Count; i++) {
                    stacks[instr[2] - 1].Insert(0, move[i]);
                }

                stacks[instr[1] - 1] = from.Skip(instr[0]).ToList();
            }

            Console.WriteLine(string.Join("", stacks.Select(x => x.FirstOrDefault())));
        }
    }
}

