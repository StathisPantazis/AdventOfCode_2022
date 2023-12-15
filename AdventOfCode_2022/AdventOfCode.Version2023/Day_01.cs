using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_01 : AoCBaseDay<int, int, string>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.FileCleanReadText(FileDescription(this, resourceType));
        return Solution(text);
    }

    protected override int Part1(string text)
    {
        return SumDigits(Helpers.TextCleanReadLines(text));
    }

    protected override int Part2(string text)
    {
        // If you are looking for shame look elsewhere
        var textReplaced = text
            .Replace("eightwo", "82")
            .Replace("eighthree", "83")
            .Replace("eighthreeight", "838")
            .Replace("eighten", "810")
            .Replace("nineight", "98")
            .Replace("zerone", "01")
            .Replace("oneight", "18")
            .Replace("threeight", "38")
            .Replace("twone", "21")
            .Replace("one", "1")
            .Replace("zero", "0")
            .Replace("two", "2")
            .Replace("three", "3")
            .Replace("four", "4")
            .Replace("five", "5")
            .Replace("six", "6")
            .Replace("seven", "7")
            .Replace("eight", "8")
            .Replace("nine", "9")
            .Replace("ten", "10");

        return SumDigits(Helpers.TextCleanReadLines(textReplaced));
    }

    private static int SumDigits(string[] lines)
    {
        return lines.Select(x => string.Join("", x.Where(y => char.IsDigit(y))))
            .Select(x => int.Parse(string.Join("", x.Length == 1 ? $"{x}{x}" : $"{x.First()}{x.Last()}")))
            .Sum();
    }
}
