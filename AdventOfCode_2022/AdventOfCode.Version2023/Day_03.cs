﻿using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using Point = AdventOfCode.Version2023.Day_03.Point;

namespace AdventOfCode.Version2023;

public class Day_03 : AoCBaseDay<int, int, IndexedGrid<Point>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(s => s.Select(c => new Point(c)))
            .ToList();

        var grid = new IndexedGrid<Point>(lines);

        return Solution(grid);
    }

    protected override int Part1(IndexedGrid<Point> grid)
    {
        var numbers = new List<Number>();
        Number number = null;
        var coord = grid.GetCoordinates(true);

        while (coord.TraverseGrid())
        {
            var point = grid[coord];

            if (point.IsNumber)
            {
                number ??= new Number();
                number.Text += point.Character;

                if (!number.IsPartNumber)
                {
                    var neighbours = coord.GetAllNeighbours(filterCondition: x => grid[x].IsSymbol);

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

    protected override int Part2(IndexedGrid<Point> grid)
    {
        var gearRatios = 0;
        var coord = grid.GetCoordinates(true);

        while (coord.TraverseGrid())
        {
            var point = grid[coord];

            if (point.Character == "*")
            {
                var neighbours = coord.GetAllNeighbours(filterCondition: x => grid[x].IsNumber);
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