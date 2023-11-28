using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_17 : AoCBaseDay<long, long, long>
{
    private Dictionary<int, List<DirectionMarker>> _dirIndexes;
    private List<Direction> _directions = new();
    private int _dictKey = 0;
    private int _iterationsUntilRepetition = 0;

    public override AoCSolution<long, long> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.File_ReadText(17, 2022, resourceType);

        text.ForEachDo(c => _directions.Add(c == '<' ? Direction.L : Direction.R));

        _ = LetTheRocksHitDaFloor(_directions, out _dirIndexes);

        // Find an index where the rock pattern is repeated
        foreach ((var key, var value) in _dirIndexes)
        {
            if (value.Count > 20 && value.Select(x => x.CaveString).Distinct().Count() == 1)
            {
                var diff = _dirIndexes[key][1].IterationIndex - _dirIndexes[key][0].IterationIndex;
                for (var i = 2; i < _dirIndexes[key].Count; i++)
                {
                    if (_dirIndexes[key][i].IterationIndex - _dirIndexes[key][i - 1].IterationIndex != diff)
                    {
                        diff = -1;
                        break;
                    }
                }

                if (diff != -1)
                {
                    _dictKey = key;
                    _iterationsUntilRepetition = diff;
                    break;
                }
            }
        }

        return new AoCSolution<long, long>(Part1(2022), Part2(1000000000000));
    }

    protected override long Part1(long args)
    {
        return SharePartSolution(args);
    }

    protected override long Part2(long args)
    {
        return SharePartSolution(args);
    }

    private long SharePartSolution(long drops)
    {
        // Count total rocks within pattern
        // Count rocks before the pattern begins
        // Count remaining rocks after pattern

        var marker = _dirIndexes[_dictKey][0];
        var totalRepetitionRocks = drops / _iterationsUntilRepetition * (_dirIndexes[_dictKey][0].SecondCaveHeight - _dirIndexes[_dictKey][0].FirstCaveHeight);
        var remainingRocksNeeded = drops % _iterationsUntilRepetition;
        var remainingRocks = (long)LetTheRocksHitDaFloor(_directions, out _, remainingRocksNeeded, _dictKey);
        var caveHeight = totalRepetitionRocks + remainingRocks + marker.CaveHeightBeforeMarker;

        return caveHeight;
    }

    private static long? LetTheRocksHitDaFloor(List<Direction> directions, out Dictionary<int, List<DirectionMarker>> dirIndexes, long runTimesAfterDirIndex = 0, int? dirIndex = null)
    {
        NextTracker tracker = new(directions);
        var cave = Grid<string>.CreateEmptyGrid();

        dirIndexes = new Dictionary<int, List<DirectionMarker>>();
        for (var i = 0; i < directions.Count; i++)
        {
            dirIndexes.Add(i, new List<DirectionMarker>());
        }

        long rocksAfterRepetitionMark = 0;
        var markCaveHeight = 0;
        var markMet = false;

        for (var i = 0; i < 40000; i++)
        {

            if (dirIndex is null)
            {
                var caveString = string.Join("\n", cave.Rows.Take(10).Select(x => string.Join(string.Empty, x)));
                dirIndexes[tracker.DirectionIndex].Add(new DirectionMarker(caveString, i));

                if (dirIndexes[tracker.DirectionIndex].Count == 1)
                {
                    dirIndexes[tracker.DirectionIndex].First().FirstCaveHeight = cave.Height;
                }
                else if (dirIndexes[tracker.DirectionIndex].Count == 2)
                {
                    dirIndexes[tracker.DirectionIndex].First().SecondCaveHeight = cave.Height;
                    dirIndexes[tracker.DirectionIndex].First().CaveHeightBeforeMarker = dirIndexes[tracker.DirectionIndex].First().FirstCaveHeight - dirIndexes[tracker.DirectionIndex].First().CaveHeightForRepetition;
                }
            }
            else
            {
                if (markMet)
                {
                    rocksAfterRepetitionMark++;

                    if (runTimesAfterDirIndex == rocksAfterRepetitionMark)
                    {
                        return cave.Height - markCaveHeight;
                    }
                }
                else if (tracker.DirectionIndex == (int)dirIndex)
                {
                    markMet = true;
                    markCaveHeight = cave.Height;
                }
            }

            var caveHeight = cave.Height;
            var rock = tracker.GetNextRock();

            for (var j = rock.Height; j > 0; j--)
            {
                cave.Rows.Insert(0, new List<string>(rock.Shape[j - 1]));
            }

            ListExtensions.ForNTimesDo(4, () =>
            {
                rock.Blow(tracker.GetNextDirection(), cave);
            });

            while (Fall(cave, rock, caveHeight))
            {
                rock.Blow(tracker.GetNextDirection(), cave);
            }

            ListExtensions.ForNTimesDo(cave.Rows.Count < 50 ? cave.Rows.Count : 50, (i) => cave.Rows[i] = cave.Rows[i].Select(x => x == "@" ? "#" : x).ToList());
        }

        return null;
    }

    private static bool Fall(Grid<string> cave, Rock rock, int caveHeight)
    {
        var lastRockIndex = GetLastRockIndex(cave);

        if (cave.Height == 1)
        {
            return false;
        }

        var nextRow = cave.Row(lastRockIndex + 1);

        // Check fall
        var rockBottom = rock.Bottom;
        var width = rockBottom.Count;

        for (var i = 0; i < width; i++)
        {
            if (nextRow[i] == "#" && rockBottom[i] == "@")
            {
                return false;
            }

            for (var j = 0; j < rock.Height - 1; j++)
            {
                if (rock.Shape[j][i] == "@" && rock.Shape[j + 1][i] == "#")
                {
                    return false;
                }
            }
        }

        // Fall
        List<string> newBottom = new();

        for (var i = 0; i < width; i++)
        {
            newBottom.Add(rock.Bottom[i] == "@" ? "@" : nextRow[i]);
        }

        // Move rumbles
        for (var i = 0; i < rock.Height - 1; i++)
        {
            for (var j = 0; j < rock.Width; j++)
            {
                rock.Shape[i][j] = rock.Shape[i][j] == "@" ? "@" : rock.Shape[i + 1][j] == "#" ? "#" : ".";
            }
        }

        rock.Shape.RemoveAt(rock.Height - 1);
        rock.Shape.Add(newBottom);

        var cropCave = false;
        if (caveHeight < cave.Height)
        {
            cave.RemoveRow(lastRockIndex + 1);
            cropCave = true;
        }

        ReplaceRockInCave(rock, cave, cropCave);
        return true;
    }

    private static void ReplaceRockInCave(Rock rock, Grid<string> cave, bool cropCave = true)
    {
        var lastRockIndex = GetLastRockIndex(cave);

        if (cropCave)
        {
            for (var i = 0; i < rock.Height; i++)
            {
                cave.Rows[lastRockIndex - i] = new List<string>(rock.Shape[rock.Height - 1 - i]);
            }
        }
        else
        {
            for (var i = 0; i < rock.Height; i++)
            {
                cave.Rows[lastRockIndex - i + 1] = new List<string>(rock.Shape[rock.Height - 1 - i]);
            }

            var firstRockRow = cave.Row(lastRockIndex - rock.Height + 1);
            for (var i = 0; i < rock.Width; i++)
            {
                firstRockRow[i] = firstRockRow[i] == "@" ? "." : firstRockRow[i];
            }
        }
    }

    private static int GetLastRockIndex(Grid<string> cave)
    {
        var lastRockIndex = 0;
        var found = false;

        for (var i = 0; i < cave.Rows.Count; i++)
        {
            var row = cave.Row(i);

            if (!found && row.Contains("@"))
            {
                found = true;
            }

            if (found && !cave.Row(i).Contains("@"))
            {
                lastRockIndex = i - 1;
                break;
            }
        }

        return lastRockIndex;
    }

    private class NextTracker
    {
        private readonly List<Direction> _directions;

        public NextTracker(List<Direction> directions)
        {
            _directions = directions;
        }

        public int Order { get; set; } = 1;
        public int DirectionIndex { get; set; } = 0;

        public Direction GetNextDirection()
        {
            var dir = _directions[DirectionIndex];
            DirectionIndex = DirectionIndex == _directions.Count - 1 ? 0 : DirectionIndex + 1;
            return dir;
        }

        public Rock GetNextRock()
        {
            var rocks = GetRock(Order);
            Order = Order == 5 ? 1 : Order + 1;
            return rocks;
        }

        private static Rock GetRock(int order)
        {
            return order switch
            {
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

    private class Rock
    {
        public Rock(List<List<string>> shape)
        {
            Shape = shape;
            Height = shape.Count;
            Width = shape[0].Count;
        }

        public List<List<string>> Shape { get; init; }
        public int Height { get; init; }
        public int Width { get; init; }
        public List<string> Bottom => Shape.Last();

        public bool Blow(Direction dir, Grid<string> cave)
        {
            // Check
            if (dir is Direction.R)
            {
                foreach (var row in Shape)
                {
                    var lastInd = row.LastIndexOf("@");
                    if (lastInd == row.Count - 1 || row[lastInd + 1] == "#")
                    {
                        return false;
                    }
                }
            }
            else if (dir is Direction.L)
            {
                foreach (var row in Shape)
                {
                    var firstInd = row.IndexOf("@");
                    if (firstInd == 0 || row[firstInd - 1] == "#")
                    {
                        return false;
                    }
                }
            }

            // Move
            if (dir is Direction.R)
            {
                foreach (var row in Shape)
                {
                    for (var i = row.Count - 1; i > 0; i--)
                    {
                        if (row[i] == "." && row[i - 1] == "@")
                        {
                            row[i - 1] = ".";
                            row[i] = "@";
                        }
                    }
                }
            }
            else if (dir is Direction.L)
            {
                foreach (var row in Shape)
                {
                    for (var i = 0; i < row.Count - 1; i++)
                    {
                        if (row[i] == "." && row[i + 1] == "@")
                        {
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

    private class DirectionMarker
    {
        public DirectionMarker(string caveString, int iterationIndex)
        {
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

    private enum TurnType { Fall, Blow };
}
