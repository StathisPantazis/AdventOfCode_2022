using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2023.Day_07;

namespace AdventOfCode.Version2023;

public class Day_07 : AoCBaseDay<int, int, List<Hand>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var hands = Helpers.FileCleanReadLines(FileDescription(this, resourceType))
            .Select(x => x.Split(' ') is string[] arr ? new Hand(arr[0], arr[1]) : new())
            .ToList();

        return Solution(hands);
    }

    protected override int Part1(List<Hand> hands)
    {
        var values = new char[] { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        var cardValues = GetCardValues(values);

        foreach (var hand in hands)
        {
            var countPerCard = hand.Cards.Select(x => (x, hand.Cards.Count(y => y == x))).Distinct().ToList();
            var counts = countPerCard.Select(x => x.Item2).OrderByDescending(x => x).ToList();
            hand.Value = GetHandValue(cardValues, hand);
            hand.Result = GetHandResult(counts);
        }

        var results = hands.OrderBy(x => x.Result).ThenBy(x => x.Value).ToList();
        return ListBuilder.ForI(results.Count).Sum(i => results[i].Bid * (i + 1));
    }

    protected override int Part2(List<Hand> hands)
    {
        var values = new char[] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
        var cardValues = GetCardValues(values);

        List<(char card, int count)> getCountPerCard(Hand hand) => hand.Cards.Select(x => (x, hand.Cards.Count(y => y == x))).Distinct().OrderByDescending(x => x.Item2).ThenByDescending(x => cardValues[x.x]).ToList();
        List<int> getCounts(List<(char card, int count)> countPerCard) => countPerCard.Select(x => x.count).OrderByDescending(x => x).ToList();

        foreach (var hand in hands)
        {
            var countPerCard = getCountPerCard(hand);
            var counts = getCounts(countPerCard);
            hand.Value = GetHandValue(cardValues, hand);

            if (hand.Cards.Contains('J') && counts.Count > 1)
            {
                hand.JacksCount = hand.Cards.Count(x => x == 'J');
                var bestCard = countPerCard.Where(x => x.card != 'J').First().card;
                hand.Cards = hand.Cards.Replace('J', bestCard);

                countPerCard = getCountPerCard(hand);
                counts = getCounts(countPerCard);
            }

            hand.Result = GetHandResult(counts);
        }

        var results = hands.OrderBy(x => x.Result).ThenBy(x => x.Value).ThenByDescending(x => x.JacksCount).ToList();
        return ListBuilder.ForI(results.Count).Sum(i => results[i].Bid * (i + 1));
    }

    private static Dictionary<char, string> GetCardValues(char[] cards)
    {
        var letters = ListBuilder.StringRange("a", "m");
        return ListBuilder.ForI(cards.Length).ToDictionary(i => cards[i], i => letters[i]);
    }

    private static string GetHandValue(Dictionary<char, string> cardValues, Hand hand) => string.Join("", ListBuilder.ForI(5).Select(i => cardValues[hand.Cards[i]]));

    private static Code GetHandResult(List<int> cardCounts)
    {
        return cardCounts.Count switch
        {
            1 => Code.FiveOfAKind,
            2 when cardCounts.SequenceEqual(new List<int> { 4, 1 }) => Code.FourOfAKind,
            2 when cardCounts.SequenceEqual(new List<int> { 3, 2 }) => Code.FullHouse,
            3 when cardCounts.SequenceEqual(new List<int> { 3, 1, 1 }) => Code.ThreeOfAKind,
            3 when cardCounts.SequenceEqual(new List<int> { 2, 2, 1 }) => Code.TwoPair,
            4 when cardCounts.SequenceEqual(new List<int> { 2, 1, 1, 1 }) => Code.OnePair,
            5 when cardCounts.SequenceEqual(new List<int> { 1, 1, 1, 1, 1 }) => Code.HighCard,
            _ => throw new InvalidOperationException()
        };
    }

    public class Hand
    {
        public Hand() { }

        public Hand(string cards, string bid)
        {
            Cards = cards;
            Bid = int.Parse(bid);
        }

        public string Cards { get; set; }
        public int Bid { get; set; }
        public Code Result { get; set; }
        public string Value { get; set; }
        public int JacksCount { get; set; }

        public override string ToString() => $"{Cards} {Bid}  [{Value}] ({JacksCount})";
    }

    public enum Code
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }
}