using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Tests.Shared;

internal static class GridBuilder
{
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
            AsymmetrySide.HugeSquare => 15,
            _ => 3
        };

        var columns = asymmetrySide switch
        {
            AsymmetrySide.MoreColumns or AsymmetrySide.ManyRows => 4,
            AsymmetrySide.ManyColumns or AsymmetrySide.BigSquare => 7,
            AsymmetrySide.HugeSquare => 15,
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
}
