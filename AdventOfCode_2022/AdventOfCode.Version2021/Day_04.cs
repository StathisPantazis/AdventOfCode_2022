using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2021.Day_04;

namespace AdventOfCode.Version2021;

public class Day_04 : AoCBaseDay<int, int, (int[] draws, IndexedGrid<BingoNumber>[] boards)>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        var draws = lines[0].Split(',').Select(int.Parse).ToArray();
        lines = lines.Skip(1).ToArray();

        var boards = Enumerable.Range(0, (lines.Length) / 5)
            .Select(i => lines.Skip(i * 5).Take(5))
            .Select(x => x.Select(y => y.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(z => new BingoNumber(int.Parse(z)))))
            .Select(x => new IndexedGrid<BingoNumber>(x))
            .ToArray();

        return Solution((draws, boards));
    }

    protected override int Part1((int[] draws, IndexedGrid<BingoNumber>[] boards) p)
    {
        var drawnNumbers = new List<int>();
        IndexedGrid<BingoNumber> firstBoard = null;

        foreach (var num in p.draws)
        {
            if (drawnNumbers.Contains(num))
            {
                continue;
            }

            drawnNumbers.Add(num);

            foreach (var board in p.boards)
            {
                foreach (var row in board.Rows)
                {
                    var hitNumbers = row.Where(x => x.Number == num);

                    foreach (var hit in hitNumbers)
                    {
                        hit.Hit = true;
                    }
                }

                if (firstBoard == null && (board.Rows.Any(row => row.All(x => x.Hit)) || board.Columns.Any(col => col.All(x => x.Hit))))
                {
                    firstBoard = board;
                }
            }

            if (firstBoard != null)
            {
                break;
            }
        }

        var unmarkedNumbersSum = firstBoard.Rows.SelectMany(x => x.Where(y => !y.Hit).Select(y => y.Number)).Sum();
        return unmarkedNumbersSum * drawnNumbers.Last();
    }

    protected override int Part2((int[] draws, IndexedGrid<BingoNumber>[] boards) p)
    {
        var drawnNumbers = new List<int>();
        var finishedBoards = new List<string>();

        foreach (var num in p.draws)
        {
            if (drawnNumbers.Contains(num))
            {
                continue;
            }

            drawnNumbers.Add(num);

            foreach (var board in p.boards)
            {
                if (finishedBoards.Contains(board.Id))
                {
                    continue;
                }

                foreach (var row in board.Rows)
                {
                    var hitNumbers = row.Where(x => x.Number == num);

                    foreach (var hit in hitNumbers)
                    {
                        hit.Hit = true;
                    }
                }

                if (board.Rows.Any(row => row.All(x => x.Hit)) || board.Columns.Any(col => col.All(x => x.Hit)))
                {
                    finishedBoards.Add(board.Id);
                }
            }

            if (finishedBoards.Count == p.boards.Length)
            {
                break;
            }
        }

        var lastBoard = p.boards.Single(x => finishedBoards.Last() == x.Id);
        var unmarkedNumbersSum = lastBoard.Rows.SelectMany(x => x.Where(y => !y.Hit).Select(y => y.Number)).Sum();
        return unmarkedNumbersSum * drawnNumbers.Last();
    }

    public class BingoNumber
    {
        public BingoNumber(int number)
        {
            Number = number;
        }

        public int Number { get; init; }
        public bool Hit { get; set; }

        public override string ToString() => $"{Number}({(Hit ? "X" : "O")})";
    }
}