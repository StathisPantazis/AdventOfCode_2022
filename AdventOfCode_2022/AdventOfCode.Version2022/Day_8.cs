using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using AdventOfCode.Version2022.Models;

namespace AdventOfCode.Version2022;

public class Day_8 : AoCBaseDay<int, int, LegacyGrid<int>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.File_CleanReadLines(8, 2022, resourceType);
        var trees = new LegacyGrid<int>(lines, singleCharacters: true);

        return new AoCSolution<int, int>(Part1(trees), Part2(trees));
    }

    protected override int Part1(LegacyGrid<int> trees)
    {
        var pos = new LegacyCoordinates(trees.Height, trees.Width, true);
        var visibleTrees = 0;

        while (pos.TraverseGrid())
        {
            if (!pos.IsOnBorder)
            {
                var tree = trees[pos];

                visibleTrees += trees.RowSliceRight(pos).Max() < tree
                    || trees.RowSliceLeft(pos).Max() < tree
                    || trees.ColumnSliceUp(pos).Max() < tree
                    || trees.ColumnSliceDown(pos).Max() < tree
                    ? 1 : 0;
            }
        }

        return visibleTrees + (trees.Height * 2) + ((trees.Height - 2) * 2);
    }

    protected override int Part2(LegacyGrid<int> trees)
    {
        var pos = new LegacyCoordinates(trees.Height, trees.Width, true);
        var value = 0;

        int treeValue(int tree, List<int> trees)
        {
            var val = 0;
            trees.ForEachDoBreak(x => val++, x => tree <= x);
            return val;
        }

        while (pos.TraverseGrid())
        {
            if (!pos.IsOnBorder)
            {
                var tree = trees[pos];
                (var r, var l, var t, var b) = (treeValue(tree, trees.RowSliceRight(pos)),
                                                treeValue(tree, trees.RowSliceLeft(pos)),
                                                treeValue(tree, trees.ColumnSliceUp(pos)),
                                                treeValue(tree, trees.ColumnSliceDown(pos)));

                value = Math.Max(r * l * t * b, value);
            }
        }

        return value;
    }
}
