using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using AdventOfCode.Version2022.Models;

namespace AdventOfCode.Version2022;

public class Day_10 : AoCBaseDay<int, string, List<(int cycle, int sum)>>
{
    public static readonly string _addx = "A";
    public static readonly string _noop = "N";

    public override AoCSolution<int, string> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_CleanReadText(10, 2022, resourceType).Replace("addx", _addx).Replace("noop", _noop);
        List<(string instr, int num)> input = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split(' ') is string[] ar ? (ar[0].ToString(), ar.Length > 1 ? int.Parse(ar[1].ToString()) : 0) : default)
            .ToList();

        // Create cycles
        int sum = 1, cycle = 1;
        List<(int cycle, int sum)> cycles = new();

        for (var i = 0; i < input.Count; i++)
        {
            (var instr, var num) = input[i];

            if (instr == _addx)
            {
                cycle++;
                cycles.Add((cycle, sum));
            }
            sum += instr == _noop ? 0 : num;
            cycle++;
            cycles.Add((cycle, sum));
        }

        return new AoCSolution<int, string>(Part1(cycles), Part2(cycles));
    }

    protected override int Part1(List<(int cycle, int sum)> cycles)
    {
        var signals = new int[] { 20, 60, 100, 140, 180, 220 };
        var signalStrength = 0;
        signals.ForEachDo(s => signalStrength += cycles.FirstOrDefault(x => x.cycle == s) is (int cycle, int sum) ? sum * cycle : 0);
        return signalStrength;
    }

    protected override string Part2(List<(int cycle, int sum)> cycles)
    {
        LegacyGrid<string> crt = new(Enumerable.Range(0, 6).Select(i => new string('.', 40)), singleCharacters: true);
        LegacyCoordinates pos = new(crt);
        string spriteTemplate = new('.', 40);
        var sprite = spriteTemplate;

        while (pos.TraverseGrid())
        {
            crt[pos] = sprite[pos.Y].ToString();
            sprite = !pos.IsAtEnd ? string.Concat(string.Join("", spriteTemplate.Take(cycles[pos.StepsTraversed].sum - 1)), "###", spriteTemplate[..Math.Max(0, 38 - cycles[pos.StepsTraversed].sum)]) : string.Empty;
        }

        return crt.ToString();
    }
}
