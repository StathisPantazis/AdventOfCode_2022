using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using AdventOfCode.Version2022.Models;

namespace AdventOfCode.Version2022;

public class Day_9 : AoCBaseDay<int, int, List<(Direction dir, int steps)>>
{
    private static readonly int _borders = 1000;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var instructionsFlipped = Helpers.File_CleanReadText(9, 2022, resourceType)
            .Replace("U", "_")
            .Replace("D", "U")
            .Replace("_", "D");

        var instructions = Helpers.Text_CleanReadLines(instructionsFlipped)
            .Select(x => (Helpers.GetDirection(x.Split(" ")[0].ToString()), int.Parse(x.Split(" ")[1].ToString())))
            .ToList();

        return new AoCSolution<int, int>(Part1(instructions), Part2(instructions));
    }

    protected override int Part1(List<(Direction dir, int steps)> instructions)
    {
        var bridge = GetGrid();
        int startX = 200, startY = 10;
        LegacyCoordinates pos_H = new(_borders, _borders, startX, startY), pos_T = pos_H.Copy();
        bridge[pos_T] = "#";

        instructions.ForEachDo(instr => ListExtensions.ForNTimesDo(instr.steps, () =>
        {
            pos_H.Move(instr.dir);
            pos_T.MoveTowards(pos_H);
            bridge[pos_T] = "#";
        }));

        return bridge.ToString().Count(x => x == '#');
    }

    protected override int Part2(List<(Direction dir, int steps)> instructions)
    {
        var bridge = GetGrid();
        int startX = 300, startY = 300;

        LegacyCoordinates pos_H = new(_borders, _borders, startX, startY);
        var tail = ListExtensions.ForNTimesFill(9, () => new LegacyCoordinates(_borders, _borders, startX, startY));

        instructions.ForEachDo(instr => ListExtensions.ForNTimesDo(instr.steps, () =>
        {
            pos_H.Move(instr.dir);
            tail.First().MoveTowards(pos_H);
            ListExtensions.ForNTimesDo(tail.Count - 1, (int t) => tail[t + 1].MoveTowards(tail[t]));
            bridge[tail.Last()] = "#";
        }));

        return bridge.ToString("").Count(x => x == '#');
    }

    private static LegacyGrid<string> GetGrid() => new(Helpers.GetGriddedStringList(_borders, '.'), singleCharacters: true);
}
