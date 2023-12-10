using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2023.Day_10;

namespace AdventOfCode.Version2023;

public class Day_10 : AoCBaseDay<int, int, IndexedGrid<Node>>
{
    private BFS<Node> _bfs;

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var grid = new IndexedGrid<Node>(Helpers.File_CleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Select(y => new Node(y.ToString())).ToList())
            .ToList());

        var directions = new List<Direction> { Direction.U, Direction.D, Direction.L, Direction.R };
        var steps = 0;

        List<Node> getNeighbours(Node previousNode)
        {
            steps++;
            var neighbours = new List<Node>();

            foreach (var direction in directions)
            {
                if (previousNode.Position.GetFromDirection(direction) is Coordinates nextPos && grid[nextPos] is Node nextNode
                    && DirectionToIsAllowed(direction, previousNode, nextNode)
                    && !(nextNode.IsStart && previousNode.Step < 4))
                {
                    nextNode.DirectionThatWasReached = direction;
                    neighbours.Add(nextNode);
                }
            }

            previousNode.IsBorder = true;
            return neighbours;
        };

        bool shouldCloseNode(Node node) => node.IsStart && steps > 4;
        void visitNode(Node node) => node.Step = steps;

        _bfs = new(getNeighbours, shouldCloseNode, visitNode);

        return Solution(grid);
    }

    protected override int Part1(IndexedGrid<Node> grid)
    {
        var start = grid.GetAllPoints().Single(x => x.Text == "S").Position;

        var searchStart = new List<Direction> { Direction.U, Direction.D, Direction.L, Direction.R }
            .Where(dir => start.GetFromDirection(dir) is Coordinates nextPos && DirectionToIsAllowed(dir, grid[start], grid[nextPos]))
            .Select(start.GetFromDirection)
            .First();

        _bfs.Search(grid[searchStart], false);
        return (_bfs.PathLength + 1) / 2;
    }

    protected override int Part2(IndexedGrid<Node> grid)
    {
        grid.GetAllPoints().Where(x => !x.IsBorder).ForEachDo(x => x.Text = " ");
        var inwardDirection = grid.GetAllPoints().FirstOrDefault(x => x.Text == "|").DirectionThatWasReached is Direction.U ? Direction.R : Direction.L;

        return grid.GetAllPoints()
            .Where(x => !x.IsBorder && !x.IsStart)
            .Where(x => !x.Position.IsOnBorder)
            .Where(x => grid.RowSliceRight(x.Position).FirstOrDefault(x => x.IsBorder) is Node node && node.LeftSideIsInsideOfBorder(inwardDirection))
            .Count();
    }

    public class Node : CoordinatesNode
    {
        public Node(string text)
        {
            Text = text;
            InitialTile = text;
            IsStart = text == "S";
        }

        public string InitialTile { get; }
        public bool IsStart { get; }
        public string Text { get; set; }
        public int Step { get; set; }
        public Direction DirectionThatWasReached { get; set; }
        public bool IsBorder { get; set; }

        public bool LeftSideIsInsideOfBorder(Direction inwardDirection)
        {
            var forLeftInward = Text switch
            {
                "F" => DirectionThatWasReached is Direction.U,
                "L" => DirectionThatWasReached is Direction.L,
                "|" => DirectionThatWasReached is Direction.U,
            };

            return (inwardDirection is Direction.L && forLeftInward) || (inwardDirection is Direction.R && !forLeftInward);
        }

        public override string ToString() => Text;
    }

    private static bool DirectionToIsAllowed(Direction direction, Node previousNode, Node nextNode)
    {
        return direction switch
        {
            Direction.U when previousNode.Text is "|" or "L" or "J" or "S" && nextNode.Text is "|" or "F" or "7" or "S" => true,
            Direction.D when previousNode.Text is "|" or "F" or "7" or "S" && nextNode.Text is "|" or "L" or "J" or "S" => true,
            Direction.L when previousNode.Text is "-" or "7" or "J" or "S" && nextNode.Text is "-" or "F" or "L" or "S" => true,
            Direction.R when previousNode.Text is "-" or "F" or "L" or "S" && nextNode.Text is "-" or "7" or "J" or "S" => true,
            _ => false
        };
    }
}
