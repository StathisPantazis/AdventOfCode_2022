using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Models.Enums;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Core;

public class AbstractBox
{
    public List<AbstractBoxEdge> Edges { get; set; }

    public string Visualize(params AbstractBoxVisualization[] visualizationSettings)
    {
        var gridText = new List<string> { "█", "┐", "┘", "┌", "└", "─", "│" };
        var fill = "█";
        var voidStr = ".";
        var excessivePadding = 20;
        var padding = visualizationSettings.Length == 0 || visualizationSettings.Contains(AbstractBoxVisualization.Padding) ? 3 : 0;

        var grid = new IndexedGrid<AbstractBoxPoint>(ListBuilder.ForI(excessivePadding * 2)
            .Select(row => ListBuilder.ForI(excessivePadding * 2).Select(col => new AbstractBoxPoint(voidStr)).ToList()).ToList());

        // Walk borders
        for (var i = 0; i < Edges.Count; i++)
        {
            // Note connecting edges
            var edge = Edges[i];

            if (i == 0)
            {
                edge.PreviousEdge = Edges[^1];
                edge.NextEdge = Edges[i + 1];
            }
            else if (i == Edges.Count - 1)
            {
                edge.PreviousEdge = Edges[i - 1];
                edge.NextEdge = Edges[0];
            }
            else
            {
                edge.PreviousEdge = Edges[i - 1];
                edge.NextEdge = Edges[i + 1];
            }

            // Walk border and ascii it
            var from = grid.GetCoordinates(edge.Start.X + excessivePadding, edge.Start.Y + excessivePadding);
            var to = grid.GetCoordinates(edge.End.X + excessivePadding, edge.End.Y + excessivePadding);

            var steps = 0;

            var isHorizontal = edge.Start.Y == edge.End.Y;
            _ = isHorizontal
                ? from.TryGetMoveAscii(from.R, out var link)
                : from.TryGetMoveAscii(from.U, out link);

            while (from.MoveTowards(to, allowOverlap: true))
            {
                steps++;

                if (steps == 1)
                {
                    edge.AfterStartPosition = from.Copy(from.X - excessivePadding, from.Y - excessivePadding);
                }

                if (!from.Equals(to))
                {
                    edge.BeforeEndPosition = from.Copy(from.X - excessivePadding, from.Y - excessivePadding);
                }

                grid[from].Text = link;
            }

            var edgeStr = voidStr;
            if (i != 0 && !edge.Start.TryGetEdgeAscii(edge.PreviousEdge.BeforeEndPosition, edge.AfterStartPosition, out edgeStr))
            {
                throw new ArgumentException();
            }

            grid[edge.Start.X + excessivePadding, edge.Start.Y + excessivePadding].Text = edgeStr;
        }

        // Remaining starting edge
        if (!Edges[0].Start.TryGetEdgeAscii(Edges[0].PreviousEdge.BeforeEndPosition, Edges[0].AfterStartPosition, out var startEdge))
        {
            throw new ArgumentException();
        }
        grid[Edges[0].Start.X + excessivePadding, Edges[0].Start.Y + excessivePadding].Text = startEdge;

        // Remove excessive padding
        grid.RemoveRows(0, grid.FirstIndexOfRow(x => gridText.Contains(x.Text)) - padding);
        grid.RemoveColumns(0, grid.FirstIndexOfColumn(x => gridText.Contains(x.Text)) - padding);

        var lastRow = grid.LastIndexOfRow(x => gridText.Contains(x.Text)) + padding;
        grid.RemoveRows(lastRow, grid.Height - lastRow);

        var lastColumn = grid.LastIndexOfColumn(x => gridText.Contains(x.Text)) + padding;
        grid.RemoveColumns(lastColumn, grid.Width - lastColumn);

        grid.RebuildCoordinates();

        // Flood fill
        if (visualizationSettings.Contains(AbstractBoxVisualization.FloodFill))
        {
            Coordinates floodFillStart = null;

            for (var y = 0; y < grid.Height; y++)
            {
                if (grid.GetPoint(x => x.Position.Y == y && gridText.Contains(x.Text)) is AbstractBoxPoint point && !gridText.Contains(grid[point.Position.R].Text))
                {
                    floodFillStart = point.Position.R;
                    break;
                }
            }

            grid.FloodFill(floodFillStart, (x) => x.Text = fill, (x) => gridText.Contains(x.Text));
        }

        // Void outside
        if (visualizationSettings.Contains(AbstractBoxVisualization.TrueVoid))
        {
            grid.GetAllPoints(x => x.Text == voidStr).ForEachDo(x => x.Text = " ");
        }

        return grid.ToString();
    }
}
