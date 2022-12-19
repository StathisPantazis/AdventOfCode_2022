using AdventOfCode_2022.Extensions;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_17 {

    public static void Solve() {
        string text = Helpers.File_ReadText(17);

        List<Direction> directions = new();
        text.ForEachDo(c => directions.Add(c == '<' ? Direction.L : Direction.R));

        _ = LetTheRocksHitDaFloor(directions, out Dictionary<int, List<DirectionMarker>> dirIndexes);

        // Find an index where the rock pattern is repeated
        int dictKey = 0;
        int iterationsUntilRepetition = 0;

        foreach ((int key, List<DirectionMarker> value) in dirIndexes) {
            if (value.Count > 20 && value.Select(x => x.CaveString).Distinct().Count() == 1) {
                int diff = dirIndexes[key][1].IterationIndex - dirIndexes[key][0].IterationIndex;
                for (int i = 2; i < dirIndexes[key].Count; i++) {
                    if (dirIndexes[key][i].IterationIndex - dirIndexes[key][i - 1].IterationIndex != diff) {
                        diff = -1;
                        break;
                    }
                }

                if (diff != -1) {
                    dictKey = key;
                    iterationsUntilRepetition = diff;
                    break;
                }
            }
        }

        long[] parts = new long[] { 2022, 1000000000000 };

        for (int i = 0; i < parts.Length; i++) {
            // Count total rocks within pattern
            // Count rocks before the pattern begins
            // Count remaining rocks after pattern

            DirectionMarker marker = dirIndexes[dictKey][0];
            long totalRepetitionRocks = parts[i] / iterationsUntilRepetition * (dirIndexes[dictKey][0].SecondCaveHeight - dirIndexes[dictKey][0].FirstCaveHeight);
            long remainingRocksNeeded = parts[i] % iterationsUntilRepetition;
            long remainingRocks = (long)LetTheRocksHitDaFloor(directions, out _, remainingRocksNeeded, dictKey);
            long caveHeight = totalRepetitionRocks + remainingRocks + marker.CaveHeightBeforeMarker;
            Console.WriteLine($"Part_{i + 1}: {caveHeight}");
        }
    }

    public static long? LetTheRocksHitDaFloor(List<Direction> directions, out Dictionary<int, List<DirectionMarker>> dirIndexes, long runTimesAfterDirIndex = 0, int? dirIndex = null) {
        NextTracker tracker = new(directions);
        Grid<string> cave = Grid<string>.CreateEmptyGrid();

        dirIndexes = new Dictionary<int, List<DirectionMarker>>();
        for (int i = 0; i < directions.Count; i++) {
            dirIndexes.Add(i, new List<DirectionMarker>());
        }

        long rocksAfterRepetitionMark = 0;
        int markCaveHeight = 0;
        bool markMet = false;

        for (int i = 0; i < 40000; i++) {

            if (dirIndex is null) {
                string caveString = string.Join("\n", cave.Rows.Take(10).Select(x => string.Join(string.Empty, x)));
                dirIndexes[tracker.DirectionIndex].Add(new DirectionMarker(caveString, i));

                if (dirIndexes[tracker.DirectionIndex].Count == 1) {
                    dirIndexes[tracker.DirectionIndex].First().FirstCaveHeight = cave.Height;
                }
                else if (dirIndexes[tracker.DirectionIndex].Count == 2) {
                    dirIndexes[tracker.DirectionIndex].First().SecondCaveHeight = cave.Height;
                    dirIndexes[tracker.DirectionIndex].First().CaveHeightBeforeMarker = dirIndexes[tracker.DirectionIndex].First().FirstCaveHeight - dirIndexes[tracker.DirectionIndex].First().CaveHeightForRepetition;
                }
            }
            else {
                if (markMet) {
                    rocksAfterRepetitionMark++;

                    if (runTimesAfterDirIndex == rocksAfterRepetitionMark) {
                        return cave.Height - markCaveHeight;
                    }
                }
                else if (tracker.DirectionIndex == (int)dirIndex) {
                    markMet = true;
                    markCaveHeight = cave.Height;
                }
            }

            int caveHeight = cave.Height;
            Rock rock = tracker.GetNextRock();

            for (int j = rock.Height; j > 0; j--) {
                cave.Rows.Insert(0, new List<string>(rock.Shape[j - 1]));
            }

            ListExtensions.ForNTimesDo(4, () => {
                rock.Blow(tracker.GetNextDirection(), cave);
            });

            while (Fall(cave, rock, caveHeight)) {
                rock.Blow(tracker.GetNextDirection(), cave);
            }

            ListExtensions.ForNTimesDo(cave.Rows.Count < 50 ? cave.Rows.Count : 50, (int i) => cave.Rows[i] = cave.Rows[i].Select(x => x == "@" ? "#" : x).ToList());
        }

        return null;
    }

    public static bool Fall(Grid<string> cave, Rock rock, int caveHeight) {
        int lastRockIndex = GetLastRockIndex(cave);

        if (cave.Height == 1) {
            return false;
        }

        List<string> nextRow = cave.Row(lastRockIndex + 1);

        // Check fall
        List<string> rockBottom = rock.Bottom;
        int width = rockBottom.Count;

        for (int i = 0; i < width; i++) {
            if (nextRow[i] == "#" && rockBottom[i] == "@") {
                return false;
            }

            for (int j = 0; j < rock.Height - 1; j++) {
                if (rock.Shape[j][i] == "@" && rock.Shape[j + 1][i] == "#") {
                    return false;
                }
            }
        }

        // Fall
        List<string> newBottom = new();

        for (int i = 0; i < width; i++) {
            newBottom.Add(rock.Bottom[i] == "@" ? "@" : nextRow[i]);
        }

        // Move rumbles
        for (int i = 0; i < rock.Height - 1; i++) {
            for (int j = 0; j < rock.Width; j++) {
                rock.Shape[i][j] = rock.Shape[i][j] == "@" ? "@" : rock.Shape[i + 1][j] == "#" ? "#" : ".";
            }
        }

        rock.Shape.RemoveAt(rock.Height - 1);
        rock.Shape.Add(newBottom);

        bool cropCave = false;
        if (caveHeight < cave.Height) {
            cave.RemoveRow(lastRockIndex + 1);
            cropCave = true;
        }

        ReplaceRockInCave(rock, cave, cropCave);
        return true;
    }

    private static void ReplaceRockInCave(Rock rock, Grid<string> cave, bool cropCave = true) {
        int lastRockIndex = GetLastRockIndex(cave);

        if (cropCave) {
            for (int i = 0; i < rock.Height; i++) {
                cave.Rows[lastRockIndex - i] = new List<string>(rock.Shape[rock.Height - 1 - i]);
            }
        }
        else {
            for (int i = 0; i < rock.Height; i++) {
                cave.Rows[lastRockIndex - i + 1] = new List<string>(rock.Shape[rock.Height - 1 - i]);
            }

            List<string> firstRockRow = cave.Row(lastRockIndex - rock.Height + 1);
            for (int i = 0; i < rock.Width; i++) {
                firstRockRow[i] = firstRockRow[i] == "@" ? "." : firstRockRow[i];
            }
        }
    }

    private static int GetLastRockIndex(Grid<string> cave) {
        int lastRockIndex = 0;
        bool found = false;

        for (int i = 0; i < cave.Rows.Count; i++) {
            List<string> row = cave.Row(i);

            if (!found && row.Contains("@")) {
                found = true;
            }

            if (found && !cave.Row(i).Contains("@")) {
                lastRockIndex = i - 1;
                break;
            }
        }

        return lastRockIndex;
    }

    public class NextTracker {
        private readonly List<Direction> _directions;

        public NextTracker(List<Direction> directions) {
            _directions = directions;
        }

        public int Order { get; set; } = 1;
        public int DirectionIndex { get; set; } = 0;

        public Direction GetNextDirection() {
            Direction dir = _directions[DirectionIndex];
            DirectionIndex = DirectionIndex == _directions.Count - 1 ? 0 : DirectionIndex + 1;
            return dir;
        }

        public Rock GetNextRock() {
            Rock rocks = GetRock(Order);
            Order = Order == 5 ? 1 : Order + 1;
            return rocks;
        }

        private static Rock GetRock(int order) {
            return order switch {
                1 => new Rock(new List<List<string>>() {
                    new List<string>() { ".", ".", "@", "@", "@", "@", "." },
                }),
                2 => new Rock(new List<List<string>>() {
                    new List<string>() { ".", ".", ".", "@", ".", ".", "." },
                    new List<string>() { ".", ".", "@", "@", "@", ".", "." },
                    new List<string>() { ".", ".", ".", "@", ".", ".", "." },
                }),
                3 => new Rock(new List<List<string>>() {
                    new List<string>() { ".", ".", ".", ".", "@", ".", "." },
                    new List<string>() { ".", ".", ".", ".", "@", ".", "." },
                    new List<string>() { ".", ".", "@", "@", "@", ".", "." },
                }),
                4 => new Rock(new List<List<string>>() {
                    new List<string>() { ".", ".", "@", ".", ".", ".", "." },
                    new List<string>() { ".", ".", "@", ".", ".", ".", "." },
                    new List<string>() { ".", ".", "@", ".", ".", ".", "." },
                    new List<string>() { ".", ".", "@", ".", ".", ".", "." },
                }),
                5 => new Rock(new List<List<string>>() {
                    new List<string>() { ".", ".", "@", "@", ".", ".", "." },
                    new List<string>() { ".", ".", "@", "@", ".", ".", "." },
                }),
            };
        }
    }

    public class Rock {
        public Rock(List<List<string>> shape) {
            Shape = shape;
            Height = shape.Count;
            Width = shape[0].Count;
        }

        public List<List<string>> Shape { get; init; }
        public int Height { get; init; }
        public int Width { get; init; }
        public List<string> Bottom => Shape.Last();

        public bool Blow(Direction dir, Grid<string> cave) {
            // Check
            if (dir is Direction.R) {
                foreach (List<string> row in Shape) {
                    int lastInd = row.LastIndexOf("@");
                    if (lastInd == row.Count - 1 || row[lastInd + 1] == "#") {
                        return false;
                    }
                }
            }
            else if (dir is Direction.L) {
                foreach (List<string> row in Shape) {
                    int firstInd = row.IndexOf("@");
                    if (firstInd == 0 || row[firstInd - 1] == "#") {
                        return false;
                    }
                }
            }

            // Move
            if (dir is Direction.R) {
                foreach (List<string> row in Shape) {
                    for (int i = row.Count - 1; i > 0; i--) {
                        if (row[i] == "." && row[i - 1] == "@") {
                            row[i - 1] = ".";
                            row[i] = "@";
                        }
                    }
                }
            }
            else if (dir is Direction.L) {
                foreach (List<string> row in Shape) {
                    for (int i = 0; i < row.Count - 1; i++) {
                        if (row[i] == "." && row[i + 1] == "@") {
                            row[i] = "@";
                            row[i + 1] = ".";
                        }
                    }
                }
            }

            ReplaceRockInCave(this, cave);

            return true;
        }
    }

    public class DirectionMarker {
        public DirectionMarker(string caveString, int iterationIndex) {
            CaveString = caveString;
            IterationIndex = iterationIndex;
        }

        public string CaveString { get; init; }

        public int CaveHeightBeforeMarker { get; set; }

        public int FirstCaveHeight { get; set; }

        public int SecondCaveHeight { get; set; }

        public int CaveHeightForRepetition => SecondCaveHeight - FirstCaveHeight;

        public int IterationIndex { get; init; }


        public override string ToString() => IterationIndex.ToString();
    }

    public enum TurnType { Fall, Blow };
}
