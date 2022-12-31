using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Models;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_9 {
    public static readonly int _borders = 1000;

    public static void Solve() {
        string instructionsFlipped = Helpers.File_CleanReadText(9).Replace("U", "_").Replace("D", "U").Replace("_", "D");
        List<(Direction dir, int steps)> instructions = Helpers.Text_CleanReadLines(instructionsFlipped)
            .Select(x => (Helpers.GetDirection(x.Split(" ")[0].ToString()), int.Parse(x.Split(" ")[1].ToString())))
            .ToList();

        Console.WriteLine($"Part_1: {Part_1(instructions)}");
        Console.WriteLine($"Part_2: {Part_2(instructions)}");
    }

    public static int Part_2(List<(Direction dir, int steps)> instructions) {
        LegacyGrid<string> bridge = GetGrid();
        int startX = 300, startY = 300;

        LegacyCoordinates pos_H = new(_borders, _borders, startX, startY);
        List<LegacyCoordinates> tail = ListExtensions.ForNTimesFill(9, () => new LegacyCoordinates(_borders, _borders, startX, startY));

        instructions.ForEachDo(instr => ListExtensions.ForNTimesDo(instr.steps, () => {
            pos_H.Move(instr.dir);
            tail.First().MoveTowards(pos_H);
            ListExtensions.ForNTimesDo(tail.Count - 1, (int t) => tail[t + 1].MoveTowards(tail[t]));
            bridge[tail.Last()] = "#";
        }));

        return bridge.ToString("").Count(x => x == '#');
    }

    public static int Part_1(List<(Direction dir, int steps)> instructions) {
        LegacyGrid<string> bridge = GetGrid();
        int startX = 200, startY = 10;
        LegacyCoordinates pos_H = new(_borders, _borders, startX, startY), pos_T = pos_H.Copy();
        bridge[pos_T] = "#";

        instructions.ForEachDo(instr => ListExtensions.ForNTimesDo(instr.steps, () => {
            pos_H.Move(instr.dir);
            pos_T.MoveTowards(pos_H);
            bridge[pos_T] = "#";
        }));

        return bridge.ToString().Count(x => x == '#');
    }

    private static LegacyGrid<string> GetGrid() => new(Helpers.GetGriddedStringList(_borders, '.'), singleCharacters: true);
}
