using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_13 : AoCBaseDay<int, int, List<IndexedGrid<string>>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var patterns = Helpers.File_ReadText(FileDescription(this, resourceType))
            .Split("\r\n\r\n")
            .Select(x => new IndexedGrid<string>(Helpers.Text_CleanReadLines(x).Select(y => y.Select(y => y.ToString()))))
            .ToList();

        return Solution(patterns);
    }

    protected override int Part1(List<IndexedGrid<string>> patterns)
    {
        var sum = 0;

        foreach (var pattern in patterns)
        {
            if (!IteratePatternPart1(pattern, GridSide.Row, ref sum))
            {
                _ = IteratePatternPart1(pattern, GridSide.Column, ref sum);
            }
        }

        return sum;
    }

    protected override int Part2(List<IndexedGrid<string>> patterns)
    {
        var sum = 0;

        foreach (var pattern in patterns)
        {
            if (!IteratePatternPart2(pattern, GridSide.Row, ref sum))
            {
                _ = IteratePatternPart2(pattern, GridSide.Column, ref sum);
            }
        }

        return sum;
    }

    private static bool IteratePatternPart1(IndexedGrid<string> pattern, GridSide gridSide, ref int sum)
    {
        var total = gridSide is GridSide.Row ? pattern.Height : pattern.Width;

        for (var i = 0; i < total - 1; i++)
        {
            var isBorder = pattern.AnyIndexesAreOnBorder(gridSide, i, i + 1);
            var index1 = i;
            var index2 = i + 1;

            while (pattern.RowOrColumn(gridSide, index1).SequenceEqual(pattern.RowOrColumn(gridSide, index2)))
            {
                if (isBorder || pattern.AnyIndexesAreOnBorder(gridSide, index1, index2))
                {
                    sum += gridSide is GridSide.Row ? (i + 1) * 100 : i + 1;
                    return true;
                }

                index1--;
                index2++;
            }
        }

        return false;
    }

    private static bool IteratePatternPart2(IndexedGrid<string> pattern, GridSide gridSide, ref int sum)
    {
        var total = gridSide is GridSide.Row ? pattern.Height : pattern.Width;

        for (var i = 0; i < total - 1; i++)
        {
            var isBorder = pattern.AnyIndexesAreOnBorder(gridSide, i, i + 1);
            var index1 = i;
            var index2 = i + 1;
            var justSmudged = SmudgeCheck(pattern, index1, index2, gridSide);
            var smudged = justSmudged;

            while (justSmudged || pattern.RowOrColumn(gridSide, index1).SequenceEqual(pattern.RowOrColumn(gridSide, index2)))
            {
                justSmudged = false;

                if (isBorder || pattern.AnyIndexesAreOnBorder(gridSide, index1, index2))
                {
                    if (smudged)
                    {
                        sum += gridSide is GridSide.Row ? (i + 1) * 100 : i + 1;
                        return true;
                    }
                    else
                    {
                        break;
                    }
                }

                index1--;
                index2++;

                if (!smudged)
                {
                    smudged = SmudgeCheck(pattern, index1, index2, gridSide);
                    justSmudged = smudged;
                }
            }
        }

        return false;
    }

    private static bool SmudgeCheck(IndexedGrid<string> pattern, int index1, int index2, GridSide gridSide)
    {
        var rowUp = pattern.RowOrColumn(gridSide, index1).ToList();
        var rowDown = pattern.RowOrColumn(gridSide, index2);

        for (var i = 0; i < rowUp.Count; i++)
        {
            rowUp[i] = rowUp[i] == "#" ? "." : "#";

            if (rowUp.SequenceEqual(rowDown))
            {
                return true;
            }

            rowUp[i] = rowUp[i] == "#" ? "." : "#";
        }

        return false;
    }
}