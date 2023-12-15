using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2021.Day_08;

namespace AdventOfCode.Version2021;

public class Day_08 : AoCBaseDay<int, int, List<Display>>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var displays = Helpers.FileCleanReadText(FileDescription(this, resourceType))
            .Replace(" | ", "|")
            .Split("\n")
            .Select(x => x.Split('|') is string[] arr ? new Display(arr[0], arr[1]) : null)
            .ToList();

        var numbers = ListBuilder.ForI(10).ToDictionary(i => i, x =>
        {
            return x switch
            {
                0 => [Segment.Top, Segment.TopRight, Segment.TopLeft, Segment.BottomRight, Segment.BottomLeft, Segment.Bottom],
                1 => [Segment.TopRight, Segment.BottomRight],
                2 => [Segment.Top, Segment.TopRight, Segment.Middle, Segment.BottomLeft, Segment.Bottom],
                3 => [Segment.Top, Segment.TopRight, Segment.Middle, Segment.BottomRight, Segment.Bottom],
                4 => [Segment.TopRight, Segment.TopLeft, Segment.Middle, Segment.BottomRight],
                5 => [Segment.Top, Segment.TopLeft, Segment.Middle, Segment.BottomRight, Segment.Bottom],
                6 => [Segment.Top, Segment.TopLeft, Segment.Middle, Segment.BottomRight, Segment.BottomLeft, Segment.Bottom],
                7 => [Segment.Top, Segment.TopRight, Segment.BottomRight],
                8 => [Segment.Top, Segment.TopRight, Segment.TopLeft, Segment.Middle, Segment.BottomRight, Segment.BottomLeft, Segment.Bottom],
                9 => new List<Segment>() { Segment.Top, Segment.TopRight, Segment.TopLeft, Segment.Middle, Segment.BottomRight, Segment.Bottom },
            };
        });

        foreach (var key in numbers.Keys)
        {
            numbers[key] = numbers[key].OrderBy(x => x).ToList();
        }

        foreach (var display in displays)
        {
            var allLetters = ListBuilder.CharRange('a', 'g');
            var segments = allLetters.ToDictionary(x => x, x => (Segment?)null);

            var one_1 = display.GetAllLetters(2);
            var seven_7 = display.GetAllLetters(3);

            var top = seven_7.Except(one_1).Single();
            segments[top] = Segment.Top;

            var zero_six_nine = display.GetAllLetters(6);
            var topRight = zero_six_nine.Where(x => zero_six_nine.Count(y => y == x) == 2).Intersect(one_1).First();
            segments[topRight] = Segment.TopRight;

            var bottomRight = one_1.Single(x => x != topRight);
            segments[bottomRight] = Segment.BottomRight;

            var two_three_five_whole = display.GetAllNumbers(5);
            var five_5 = two_three_five_whole.Where(x => !x.Contains(topRight)).First();
            var five_5_letters = five_5.Select(x => x).ToList();

            var bottomLeft = allLetters.Except(five_5).Except(one_1).First();
            segments[bottomLeft] = Segment.BottomLeft;

            var two_2 = two_three_five_whole.Except(five_5).Where(x => !x.Contains(one_1[0]) || !x.Contains(one_1[1])).First();
            var two_2_letters = two_2.Select(x => x).ToList();
            var topLeft = allLetters.Except(two_2_letters).Except(one_1).First();
            segments[topLeft] = Segment.TopLeft;

            var four_4 = display.GetAllLetters(4);
            var middle = four_4.Except(one_1).Except(topLeft).First();
            segments[middle] = Segment.Middle;

            var bottom = allLetters.Except(four_4).Except(top).Except(bottomLeft).First();
            segments[bottom] = Segment.Bottom;

            foreach (var digit in display.Output)
            {
                var digitSegments = new List<Segment>();

                foreach (var c in digit.Code)
                {
                    digitSegments.Add((Segment)segments[c]);
                }

                digitSegments = [.. digitSegments.OrderBy(x => x)];
                digit.Number = numbers.Single(x => Enumerable.SequenceEqual(digitSegments, x.Value)).Key;
            }
        }

        return Solution(displays);
    }

    protected override int Part1(List<Display> displays)
    {
        return displays.Sum(x => x.Output.Count(y => y.Number is 1 or 4 or 7 or 8));
    }

    protected override int Part2(List<Display> displays)
    {
        return displays.Sum(x => int.Parse(string.Join("", x.Output.Select(y => y.Number))));
    }

    public class Display(string signalText, string outputText)
    {
        public List<Digit> Signals { get; init; } = signalText.Split(' ').Select(x => new Digit(x)).ToList();
        public List<Digit> Output { get; init; } = outputText.Split(' ').Select(x => new Digit(x)).ToList();

        public List<char> GetAllLetters(int segmentCount)
        {
            return Signals
                .Where(x => x.Code.Length == segmentCount)
                .Select(x => x.Code)
                .Distinct()
                .SelectMany(x => x)
                .ToList();
        }

        public List<string> GetAllNumbers(int segmentCount)
        {
            return Signals
                .Where(x => x.Code.Length == segmentCount)
                .Select(x => x.Code)
                .Distinct()
                .ToList();
        }

        public override string ToString() => $"{Signals.ListToString(" ")} | {Output.ListToString(" ")}";
    }

    public class Digit(string code)
    {
        public string Code { get; init; } = code;
        public int Number { get; set; } = -1;

        public void Print(Dictionary<char, Segment?> segments)
        {
            var segment_char = new Dictionary<Segment, char>();

            foreach (var c in Code)
            {
                if (segments[c] != null)
                {
                    segment_char.Add((Segment)segments[c], c);
                }
            }

            Console.WriteLine(
                $" {(segment_char.TryGetValue(Segment.Top, out var top) ? top.Repeat(4) : "....")} \n" +
                $"{(segment_char.TryGetValue(Segment.TopLeft, out var topLeft1) ? topLeft1.Repeat(1) : ".")}    {(segment_char.TryGetValue(Segment.TopRight, out var topRight1) ? topRight1.Repeat(1) : ".")}\n" +
                $"{(segment_char.TryGetValue(Segment.TopLeft, out var topLeft2) ? topLeft2.Repeat(1) : ".")}    {(segment_char.TryGetValue(Segment.TopRight, out var topRight2) ? topRight2.Repeat(1) : ".")}\n" +
                $" {(segment_char.TryGetValue(Segment.Middle, out var middle) ? middle.Repeat(4) : "....")} \n" +
                $"{(segment_char.TryGetValue(Segment.BottomLeft, out var bottomLeft1) ? bottomLeft1.Repeat(1) : ".")}    {(segment_char.TryGetValue(Segment.BottomRight, out var bottomRight1) ? bottomRight1.Repeat(1) : ".")}\n" +
                $"{(segment_char.TryGetValue(Segment.BottomLeft, out var bottomLeft2) ? bottomLeft2.Repeat(1) : ".")}    {(segment_char.TryGetValue(Segment.BottomRight, out var bottomRight2) ? bottomRight2.Repeat(1) : ".")}\n" +
                $" {(segment_char.TryGetValue(Segment.Bottom, out var bottom) ? bottom.Repeat(4) : "....")} \n");
        }

        public override string ToString() => $"{Code}{(Number > -1 ? $"({Number})" : string.Empty)}";
    }

    public enum Segment
    {
        Top, TopLeft, TopRight, Middle, Bottom, BottomLeft, BottomRight
    }
}