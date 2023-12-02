using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2021;

public class Day_3 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.File_CleanReadLines(FileDescription(this));
        return Solution(lines);
    }

    protected override int Part1(string[] lines)
    {
        var gama = string.Empty;
        var epsilon = string.Empty;

        for (var i = 0; i < lines[0].ToString().Length; i++)
        {
            var zeroes = lines.Count(x => x[i] == '0');
            var ones = lines.Count(x => x[i] == '1');

            gama += zeroes > ones ? "0" : "1";
            epsilon += zeroes < ones ? "0" : "1";
        }

        return gama.FromBinaryToInt() * epsilon.FromBinaryToInt();
    }

    protected override int Part2(string[] lines)
    {
        var oxygen = lines.ToList();
        var c02 = lines.ToList();

        for (var i = 0; i < lines[0].ToString().Length; i++)
        {
            var zeroes = oxygen.Count(x => x[i] == '0');
            var ones = oxygen.Count(x => x[i] == '1');

            if (oxygen.Count > 1)
            {
                oxygen.RemoveAll(x => x[i] == (zeroes > ones ? '1' : '0'));
            }

            zeroes = c02.Count(x => x[i] == '0');
            ones = c02.Count(x => x[i] == '1');

            if (c02.Count > 1)
            {
                c02.RemoveAll(x => x[i] == (zeroes > ones ? '0' : '1'));
            }
        }

        return oxygen[0].FromBinaryToInt() * c02[0].FromBinaryToInt();
    }
}