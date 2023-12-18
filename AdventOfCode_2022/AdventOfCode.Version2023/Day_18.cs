using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

public partial class Day_18 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var instructions = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Replace("(#", "").Replace(")", "").Split(' ') is string[] arr ? new Instruction(Enum.Parse<Direction>(arr[0]), int.Parse(arr[1]), arr[2]) : default)
            .ToList();

        //foreach (var instr in instructions)
        //{
        //    instr.Direction = GetDirection(int.Parse(instr.RGB.Last().ToString()));
        //    instr.Steps = Convert.ToInt32(instr.RGB.CropUntil(instr.RGB.Length - 1), 16);
        //}

        return default;
    }

    private Direction GetDirection(int num)
    {
        return num switch
        {
            0 => Direction.R,
            1 => Direction.D,
            2 => Direction.L,
            3 => Direction.U,
        };
    }

    protected override int Part1(string[] args)
    {
        //    var instructions = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
        //.Select(x => x.Replace("(#", "").Replace(")", "").Split(' ') is string[] arr ? new Instruction(Enum.Parse<Direction>(arr[0]), int.Parse(arr[1]), arr[2]) : default)
        //.ToList();

        //    var grid = new IndexedGrid<Point>(ListBuilder.ForI(800).Select(row => ListBuilder.ForI(800).Select(col => new Point(".")).ToList()).ToList());

        //    // Grid borders
        //    var traversal = grid.GetCoordinates(500, 500);

        //    foreach (var instr in instructions)
        //    {
        //        var steps = 0;

        //        while (traversal.Move(instr.Direction, (x) => steps == instr.Steps))
        //        {
        //            steps++;
        //            grid[traversal].Text = "#";
        //        }
        //    }

        //    // Remove padding
        //    grid.RemoveRows(0, grid.FirstIndexOfRow(x => x.Text == "#"));
        //    grid.RemoveColumns(0, grid.FirstIndexOfColumn(x => x.Text == "#"));

        //    var lastRow = grid.LastIndexOfRow(x => x.Text == "#");
        //    grid.RemoveRows(lastRow, grid.Height - lastRow);

        //    var lastColumn = grid.LastIndexOfColumn(x => x.Text == "#");
        //    grid.RemoveColumns(lastColumn, grid.Width - lastColumn);

        //    // Fill
        //    var firstInsidePosition = grid.GetPoint(x => x.Position.Y == 1 && x.Text == "#").Position.R;

        //    grid.FloodFill(firstInsidePosition,
        //        fill: (x) => x.Text = "#",
        //        shouldStopFill: (x) => x.Text != ".");

        //    Console.WriteLine(grid.GetAllPoints(x => x.Text == "#").Count);
        return 0;
    }

    protected override int Part2(string[] args)
    {
        return 0;
    }

    public class Instruction(Direction direction, int steps, string rGB)
    {
        public Direction Direction { get; set; } = direction;
        public int Steps { get; set; } = steps;
        public string RGB { get; set; } = rGB;

        public override string ToString() => $"{Direction} {Steps} (#{RGB})";
    }

    public class Point(string text) : CoordinatedBase
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string Text { get; set; } = text;
        public bool IsInsideBorder { get; set; }

        public override string ToString() => Text;
    }
}