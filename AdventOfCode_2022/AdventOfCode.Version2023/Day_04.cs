using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2023.Day_04;

namespace AdventOfCode.Version2023;

public class Day_04 : AoCBaseDay<int, int, List<Card>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var text = Helpers.FileReadText(FileDescription(this, resourceType))
            .Replace("Card ", "")
            .Replace(": ", "|")
            .Replace(" | ", "|");

        var cards = Helpers.TextCleanReadLines(text)
            .Select(x => x.Split('|') is string[] arr ? new Card()
            {
                Id = int.Parse(arr[0]),
                Winning = arr[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y.ToString())).ToList(),
                Own = arr[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y.ToString())).ToList(),
            } : new())
            .ToList();

        return Solution(cards);
    }

    protected override int Part1(List<Card> cards)
    {
        return cards.Sum(x => (int)Math.Pow(2, x.Wins - 1));
    }

    protected override int Part2(List<Card> cards)
    {
        var id_count = cards.Select(x => x.Id).ToDictionary(x => x, x => 1);
        var ids = cards.Select(x => x.Id).ToList();

        for (var i = 0; i < ids.Count; i++)
        {
            var id = ids[i];
            var idCards = id_count[id];

            var card = cards.FirstOrDefault(x => x.Id == id);
            var count = id_count[id];

            var nextId = i + 2;

            if (nextId == ids.Count)
            {
                break;
            }

            for (var j = 0; j < count; j++)
            {
                nextId = i + 2;

                for (var z = 0; z < card.Wins; z++)
                {
                    id_count[nextId] += 1;
                    nextId++;
                }
            }
        }

        return id_count.Sum(x => x.Value);
    }

    public class Card
    {
        private int? _wins;

        public int Id { get; set; }
        public List<int> Winning { get; set; } = new();
        public List<int> Own { get; set; } = new();
        public int Wins
        {
            get
            {
                _wins ??= Own.Count(Winning.Contains);
                return _wins.Value;
            }
        }

        public override string ToString() => $"{Id}: {string.Join(" ", Winning)} | {string.Join(" ", Own)}";
    }
}