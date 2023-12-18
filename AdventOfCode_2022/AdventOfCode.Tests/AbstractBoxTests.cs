using AdventOfCode.Core;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Enums;
using FluentAssertions;

namespace AdventOfCode.Tests;

public class AbstractBoxTests
{
    [Theory]
    [InlineData(AbstractBoxVisualization.FloodFill)]
    [InlineData(AbstractBoxVisualization.Padding)]
    [InlineData(AbstractBoxVisualization.TrueVoid)]
    [InlineData(AbstractBoxVisualization.Padding, AbstractBoxVisualization.TrueVoid)]
    [InlineData(AbstractBoxVisualization.Padding, AbstractBoxVisualization.FloodFill)]
    [InlineData(AbstractBoxVisualization.FloodFill, AbstractBoxVisualization.TrueVoid)]
    [InlineData(AbstractBoxVisualization.Padding, AbstractBoxVisualization.FloodFill, AbstractBoxVisualization.TrueVoid)]
    public void Visualizations_should_word(params AbstractBoxVisualization[] visualizations)
    {
        var box = MockBox();

        var str = box.Visualize(visualizations);

        if (visualizations.SequenceEqual([AbstractBoxVisualization.FloodFill]))
        {
            var stringToMatch = "┌─────┐.┌─┐\n│█████│.│█│\n│█████│.│█│\n│█████└─┘█│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n└─────────┘";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.TrueVoid]))
        {
            var stringToMatch = "┌─────┐ ┌─┐\n│     │ │ │\n│     │ │ │\n│     └─┘ │\n│         │\n│         │\n│         │\n│         │\n│         │\n│         │\n└─────────┘";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.Padding]))
        {
            var stringToMatch = ".................\n.................\n.................\n...┌─────┐.┌─┐...\n...│.....│.│.│...\n...│.....│.│.│...\n...│.....└─┘.│...\n...│.........│...\n...│.........│...\n...│.........│...\n...│.........│...\n...│.........│...\n...│.........│...\n...└─────────┘...\n.................\n.................\n.................";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.Padding, AbstractBoxVisualization.TrueVoid]))
        {
            var stringToMatch = "                 \n                 \n                 \n   ┌─────┐ ┌─┐   \n   │     │ │ │   \n   │     │ │ │   \n   │     └─┘ │   \n   │         │   \n   │         │   \n   │         │   \n   │         │   \n   │         │   \n   │         │   \n   └─────────┘   \n                 \n                 \n                 ";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.Padding, AbstractBoxVisualization.FloodFill]))
        {
            var stringToMatch = ".................\n.................\n.................\n...┌─────┐.┌─┐...\n...│█████│.│█│...\n...│█████│.│█│...\n...│█████└─┘█│...\n...│█████████│...\n...│█████████│...\n...│█████████│...\n...│█████████│...\n...│█████████│...\n...│█████████│...\n...└─────────┘...\n.................\n.................\n.................";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.FloodFill, AbstractBoxVisualization.TrueVoid]))
        {
            var stringToMatch = "┌─────┐ ┌─┐\n│█████│ │█│\n│█████│ │█│\n│█████└─┘█│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n│█████████│\n└─────────┘";
            str.Should().Be(stringToMatch);
        }
        else if (visualizations.SequenceEqual([AbstractBoxVisualization.Padding, AbstractBoxVisualization.FloodFill, AbstractBoxVisualization.TrueVoid]))
        {
            var stringToMatch = "                 \n                 \n                 \n   ┌─────┐ ┌─┐   \n   │█████│ │█│   \n   │█████│ │█│   \n   │█████└─┘█│   \n   │█████████│   \n   │█████████│   \n   │█████████│   \n   │█████████│   \n   │█████████│   \n   │█████████│   \n   └─────────┘   \n                 \n                 \n                 ";
            str.Should().Be(stringToMatch);
        }
    }

    private AbstractBox MockBox()
    {
        return new AbstractBox()
        {
            Edges =
            [
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 0, 0),
                    End = new IndexedCoordinates(1000, 1000, 6, 0),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 6, 0),
                    End = new IndexedCoordinates(1000, 1000, 6, 3),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 6, 3),
                    End = new IndexedCoordinates(1000, 1000, 8, 3),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 8, 3),
                    End = new IndexedCoordinates(1000, 1000, 8, 0),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 8, 0),
                    End = new IndexedCoordinates(1000, 1000, 10, 0),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 10, 0),
                    End = new IndexedCoordinates(1000, 1000, 10, 10),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 10, 10),
                    End = new IndexedCoordinates(1000, 1000, 0, 10),
                },
                new AbstractBoxEdge()
                {
                    Start = new IndexedCoordinates(1000, 1000, 0, 10),
                    End = new IndexedCoordinates(1000, 1000, 0, 0),
                },
            ]
        };
    }
}
