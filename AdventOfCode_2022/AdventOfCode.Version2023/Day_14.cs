using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public class Day_14 : AoCBaseDay<int, int, AoCResourceType>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        return Solution(resourceType);
    }

    protected override int Part1(AoCResourceType resourceType)
    {
        var grid = GetGrid(resourceType);
        TiltGrid(grid);
        grid.RebuildCoordinates();

        return grid.GetAllPoints(x => x.Text == "O").Sum(x => x.Position.Y + 1);
    }

    protected override int Part2(AoCResourceType resourceType)
    {
        var grid = GetGrid(resourceType);
        var memo = new Dictionary<string, int>();
        var repeatPoint = 0;
        var iterations = 1000000000;

        for (var i = 0; i < iterations; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                TiltGrid(grid);
                grid.Rotate();
            }

            var key = grid.ToString();
            if (memo.ContainsKey(key))
            {
                repeatPoint = memo.Keys.ToList().IndexOf(key);
                memo.Keys.Take(repeatPoint).ToList().ForEach(k => memo.Remove(k));
                break;
            }

            var sum = grid.GetAllPoints(x => x.Text == "O").Sum(x => x.Position.Y + 1);
            memo.Add(key, sum);
        }

        var index = ((iterations - repeatPoint) % memo.Count) - 1;
        return memo[memo.Keys.ToList()[index]];
    }

    private static void TiltGrid(CartesianGrid<Rock> grid)
    {
        var rocks = grid.GetAllPoints(x => x.Text == "O");

        foreach (var rock in rocks)
        {
            (var x, var y) = (rock.Position.X, rock.Position.Y);

            while (rock.Position.Move(Direction.U, (x) => grid[x].Text != "."))
            {
            }

            grid[x, y].Text = ".";
            grid[rock.Position].Text = "O";
        }
    }

    private CartesianGrid<Rock> GetGrid(AoCResourceType resourceType)
        => new(Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Select(y => new Rock(y.ToString())).ToList())
            .ToList());

    public class Rock(string text) : CoordinatesNode
    {
        public string Text { get; set; } = text;

        public override string ToString() => Text;
    }
}
