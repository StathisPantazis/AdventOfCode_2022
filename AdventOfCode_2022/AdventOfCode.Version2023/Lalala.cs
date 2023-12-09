using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2023;

internal class Lalala
{
    public void Run()
    {
        var grid = GetGrid(GridType.Cartesian, AsymmetrySide.ManyRows);
        var coord = grid.GetCoordinates(3, 3);

        var start = grid.GetCoordinates(1, 1);
        grid[start] = new Point("S");
        var awayFrom = grid.GetCoordinates(0, 0);
        grid[awayFrom] = new Point("X");
        grid.Print();

        while (start.MoveOpposite(awayFrom))
        {
            grid[start] = new Point(".");
        }

        grid.Print();
        Console.WriteLine("Cartesian");

        //var indexed = GetGrid(GridType.Indexed, AsymmetrySide.BigSquare);
        //var indCoord = indexed.GetCoordinates(3, 3);
        ////indCoord = indCoord.U;
        //indexed[indCoord] = new Point(".");

        //indexed.Print(false);
        //Console.WriteLine("Indexed");
    }

    public static Grid<Point> GetGrid(GridType gridType, AsymmetrySide asymmetrySide, bool rowsSameNumber = false, bool columnsSameNumber = false)
    {
        if (rowsSameNumber && columnsSameNumber)
        {
            throw new InvalidOperationException();
        }

        var rows = asymmetrySide switch
        {
            AsymmetrySide.MoreRows or AsymmetrySide.ManyColumns => 4,
            AsymmetrySide.ManyRows or AsymmetrySide.BigSquare => 7,
            _ => 3
        };

        var columns = asymmetrySide switch
        {
            AsymmetrySide.MoreColumns or AsymmetrySide.ManyRows => 4,
            AsymmetrySide.ManyColumns or AsymmetrySide.BigSquare => 7,
            _ => 3
        };

        var rowColumnNumber = rowsSameNumber ? 1 : 0;
        var counter = 0;

        string getCounter()
        {
            if (rowsSameNumber)
            {
                if (counter == columns)
                {
                    rowColumnNumber++;
                    counter = 0;
                }

                counter++;

                return rowColumnNumber.ToString();
            }
            else if (columnsSameNumber)
            {
                if (counter == columns)
                {
                    rowColumnNumber = 0;
                    counter = 0;
                }

                rowColumnNumber++;
                counter++;

                return rowColumnNumber.ToString();
            }

            counter++;
            counter = counter > 9 ? 0 : counter;
            return counter.ToString();
        }

        return gridType switch
        {
            GridType.Indexed => new IndexedGrid<Point>(ListBuilder.ForI(rows).Select(x => ListBuilder.ForI(columns).Select(y => new Point(getCounter())).ToList())),
            GridType.Cartesian => new CartesianGrid<Point>(ListBuilder.ForI(rows).Select(x => ListBuilder.ForI(columns).Select(y => new Point(getCounter())).ToList())),
            _ => throw new NotImplementedException()
        };
    }

    public class Point : GridInput<string>
    {
        public Point()
        {
            EmptyValue = ".";
            ValueToFillWithEmptyName = nameof(Text);
        }

        public Point(string text) : this()
        {
            Text = text;
        }

        public string Text { get; set; }
        public override string ToString() => Text;
    }

    public enum AsymmetrySide
    {
        None,
        MoreRows,
        MoreColumns,
        ManyRows,
        ManyColumns,
        BigSquare,
    }
}
