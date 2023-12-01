using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_13 : AoCBaseDay<int, int, string[]>
{
    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var lines = Helpers.File_CleanReadLines(FileDescription(this, resourceType)).Where(x => !string.IsNullOrEmpty(x)).ToArray();

        var part1 = Part1(lines);
        var part2 = Part2(lines);

        return new AoCSolution<int, int>(part1, part2);
    }

    protected override int Part1(string[] lines)
    {
        var correctIndexes = new List<int>();
        var pairIndex = 0;

        for (var i = 0; i < lines.Length; i++)
        {
            pairIndex++;

            var leftPart = new Part(lines[i]);
            var rightPart = new Part(lines[i + 1]);
            i++;

            if (Compare(leftPart, rightPart))
            {
                correctIndexes.Add(pairIndex);
            }
        }

        return correctIndexes.Sum();
    }

    protected override int Part2(string[] lines)
    {
        return 0;
    }

    private static bool Compare(Part leftPart, Part rightPart)
    {
        var left = leftPart.GetTextToExamine();
        var right = rightPart.GetTextToExamine();

        if (left == "")
        {
            return true;
        }
        else if (right == "")
        {
            return false;
        }
        else if (left.StartsWith('[') && !right.StartsWith('['))
        {
            rightPart.TurnExaminedNumberToList();
        }
        else if (!left.StartsWith('[') && right.StartsWith('['))
        {
            leftPart.TurnExaminedNumberToList();
        }
        else if (left.StartsWith('['))
        {
            if (Compare(new Part(left), new Part(right)))
            {
                leftPart.CleanExaminedText();
                rightPart.CleanExaminedText();
            }
            else
            {
                return false;
            }
        }
        else if (int.Parse(left) > int.Parse(right))
        {
            return false;
        }
        else if (int.Parse(left) < int.Parse(right))
        {
            return true;
        }
        else if (int.Parse(left) == int.Parse(right))
        {
            leftPart.CleanExaminedText();
            rightPart.CleanExaminedText();
            return Compare(leftPart, rightPart);
        }

        return Compare(leftPart, rightPart);
    }

    private record Part
    {
        private string _textToExamine = "";

        public Part(string text)
        {
            Text = text[1..^1];
        }

        public string Text { get; set; }

        public void CleanExaminedText()
        {
            Text = Text.Length == 1
                ? ""
                : Text[_textToExamine.Length..];

            if (Text.StartsWith(','))
            {
                Text = Text[1..];
            }
        }

        public string GetTextToExamine()
        {
            if (Text.Length == 0)
            {
                _textToExamine = "";
            }
            else if (Text[0] != '[')
            {
                _textToExamine = Text[0].ToString();
            }
            else
            {
                var openingBracketsCount = Text[..Text.IndexOf(']')].Count(x => x == '[');
                var closingBracketsCount = 0;

                for (var i = 0; i < Text.Length; i++)
                {
                    if (Text[i] == ']')
                    {
                        closingBracketsCount++;
                    }

                    if (closingBracketsCount == openingBracketsCount)
                    {
                        _textToExamine = Text[..(i + 1)];
                        break;
                    }
                }
            }

            return _textToExamine;
        }

        public void TurnExaminedNumberToList()
        {
            _textToExamine = $"[{Text[0]}]";
            Text = $"{_textToExamine}{Text[1..]}";
        }

        public override string ToString() => Text;
    }
}
