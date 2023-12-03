using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using Point = AdventOfCode.Version2023.Day_3.Point;

namespace AdventOfCode.Version2023;

public class Day_3 : AoCBaseDay<int, int, Grid<Point>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .Select(s => s.Select(c => new Point(c)))
            .ToList();

        var grid = new Grid<Point>(lines);

        return Solution(grid);
    }

    protected override int Part1(Grid<Point> grid)
    {
        var numbers = new List<Number>();
        Number number = null;
        var coord = new Coordinates(grid, true);

        while (coord.TraverseGrid())
        {
            var point = grid[coord];

            if (point.IsNumber)
            {
                number ??= new Number();
                number.Text += point.Character;

                if (!number.IsPartNumber)
                {
                    var neighbours = coord.GetAllNeighbours(x => grid[x].IsSymbol);

                    foreach (var neighbour in neighbours)
                    {
                        number.IsPartNumber = true;
                        numbers.Add(number);
                        break;
                    }
                }

                point.Number = number;
            }
            else
            {
                number = null;
            }
        }

        return numbers.Sum(x => x.Num);
    }

    protected override int Part2(Grid<Point> grid)
    {
        var gearRatios = 0;
        var coord = new Coordinates(grid, true);

        while (coord.TraverseGrid())
        {
            var point = grid[coord];

            if (point.Character == "*")
            {
                var neighbours = coord.GetAllNeighbours(x => grid[x].IsNumber);
                var adjacentNumbers = new List<Number>();

                foreach (var neighbour in neighbours)
                {
                    adjacentNumbers.Add(grid[neighbour].Number);
                }

                adjacentNumbers = adjacentNumbers.DistinctBy(x => x.Id).ToList();

                if (adjacentNumbers.Count == 2)
                {
                    gearRatios += adjacentNumbers[0].Num * adjacentNumbers[1].Num;
                }
            }
        }

        return gearRatios;
    }

    public class Point
    {
        public Point(char character)
        {
            Character = character.ToString();
            IsNumber = char.IsDigit(character);
            IsSymbol = !IsNumber && character != '.';
        }

        public string Character { get; set; }
        public bool IsSymbol { get; set; }
        public bool IsNumber { get; set; }
        public Number Number { get; set; }

        public override string ToString() => Character;
    }

    public class Number
    {
        public Number()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }
        public string Text { get; set; }
        public bool IsPartNumber { get; set; }
        public int Num => int.Parse(Text);
    }
}