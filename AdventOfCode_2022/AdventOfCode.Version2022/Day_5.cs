using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_5 : AoCBaseDay<string, string, string[]>
{
    public override AoCSolution<string, string> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(5, 2022, resourceType)
            .Replace("    [", " ").Replace("]    ", " ").Replace("    ", " ").Replace("[", "").Replace("]", "")
            .Replace("move ", "").Replace(" from ", " ").Replace(" to ", " ");

        var lines = Helpers.Text_CleanReadLines(text);

        return new AoCSolution<string, string>(Part1(lines), Part2(lines));
    }

    protected override string Part1(string[] args)
    {
        return ShareSolution(1, args);
    }

    protected override string Part2(string[] args)
    {
        return ShareSolution(2, args);
    }

    private static string ShareSolution(int part, string[] lines)
    {
        var instructionIndex = lines.ToList().IndexOf(lines.First(y => y.StartsWith(" 1"))) + 2;
        var total = int.Parse(lines[instructionIndex - 2].Trim().Last().ToString());
        var instructions = lines.Skip(instructionIndex).Select(x => x.Split(' ').Select(y => int.Parse(y)).ToArray()).ToArray();

        var stacks = Enumerable.Range(0, total)
            .Select(i => lines.Take(instructionIndex - 2).Select(x => x.Split(' ')[i]).Where(x => !string.IsNullOrEmpty(x)).ToList())
            .ToList();

        foreach (var instr in instructions)
        {
            var from = stacks[instr[1] - 1];
            var move = part == 1 ? from.Take(instr[0]).ToList() : from.Take(instr[0]).Reverse().ToList();

            for (var i = 0; i < move.Count; i++)
            {
                stacks[instr[2] - 1].Insert(0, move[i]);
            }

            stacks[instr[1] - 1] = from.Skip(instr[0]).ToList();
        }

        return string.Join("", stacks.Select(x => x.FirstOrDefault()));
    }
}

