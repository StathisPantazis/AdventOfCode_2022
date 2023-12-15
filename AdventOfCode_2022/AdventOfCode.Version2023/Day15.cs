using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_15 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var hashes = Helpers.File_CleanReadText(FileDescription(this, resourceType)).Split(',').ToArray();
        return Solution(hashes);
    }

    protected override int Part1(string[] hashes)
    {
        return hashes.Sum(GetHashAscii);
    }

    protected override int Part2(string[] hashes)
    {
        var boxes = ListBuilder.ForI(256).Select(x => new List<(string str, int num)>()).ToList();

        foreach (var hash in hashes)
        {
            (var str, var num) = (hash.CropUntil('-').CropUntil('='), hash.SplitMinLengthCheck("=", 2) is string[] arr ? int.Parse(arr[1]) : -1);
            var ascii = GetHashAscii(str);

            var box = boxes[ascii];

            if (box.Any(x => x.str == str))
            {
                var index = box.IndexOf(box.First(x => x.str == str));
                box.RemoveAt(index);

                if (num > -1)
                {
                    box.Insert(index, (str, num));
                }
            }
            else if (!hash.Contains('-'))
            {
                boxes[ascii].Add((str, num));
            }
        }

        return boxes.Select((box, i) => box.Select((_, j) => (i + 1) * (j + 1) * box[j].num).Sum()).Sum();
    }

    private static int GetHashAscii(string hash)
    {
        var value = 0;

        foreach (var letter in hash)
        {
            value += letter.ToAscii();
            value *= 17;
            value %= 256;
        }

        return value;
    }
}