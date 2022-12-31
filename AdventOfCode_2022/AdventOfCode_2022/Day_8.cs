using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Models;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_8 {
    public static void Solve() {
        LegacyGrid<int> trees = new(Helpers.File_CleanReadLines(8), singleCharacters: true);

        Console.WriteLine(Part_1(trees));
        Console.WriteLine(Part_2(trees));
    }

    public static int Part_2(LegacyGrid<int> trees) {
        LegacyCoordinates pos = new(trees.Height, trees.Width, true);
        int value = 0;

        int treeValue(int tree, List<int> trees) {
            int val = 0;
            trees.ForEachDoBreak(x => val++, x => tree <= x);
            return val;
        }

        while (pos.TraverseGrid()) {
            if (!pos.IsOnBorder) {
                int tree = trees[pos];
                (int r, int l, int t, int b) = (treeValue(tree, trees.RowSliceRight(pos)),
                                                treeValue(tree, trees.RowSliceLeft(pos)),
                                                treeValue(tree, trees.ColumnSliceUp(pos)),
                                                treeValue(tree, trees.ColumnSliceDown(pos)));

                value = Math.Max(r * l * t * b, value);
            }
        }

        return value;
    }

    public static int Part_1(LegacyGrid<int> trees) {
        LegacyCoordinates pos = new(trees.Height, trees.Width, true);
        int visibleTrees = 0;

        while (pos.TraverseGrid()) {
            if (!pos.IsOnBorder) {
                int tree = trees[pos];

                visibleTrees += trees.RowSliceRight(pos).Max() < tree
                    || trees.RowSliceLeft(pos).Max() < tree
                    || trees.ColumnSliceUp(pos).Max() < tree
                    || trees.ColumnSliceDown(pos).Max() < tree
                    ? 1 : 0;
            }
        }

        return visibleTrees + (trees.Height * 2) + ((trees.Height - 2) * 2);
    }
}
