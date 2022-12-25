using AdventOfCode_2022.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using static AdventOfCode_2022.Day_13;

namespace AdventOfCode_2022;

internal static class Day_13 {
    internal static void Solve() {
        string[] lines = Helpers.File_CleanReadLines(13).Where(x => !string.IsNullOrEmpty(x)).ToArray();

        List<int> indexes = new();
        int pairCount = 0;

        for (int i = 0; i < lines.Length - 1; i += 2) {
            bool isCorrect = true;
            pairCount += 1;
            string textLeft = lines[i][1..^1], textRight = lines[i + 1][1..^1];
            TextCursor cursorLeft = new(lines[i][1..^1], 0);
            TextCursor cursorRight = new(lines[i + 1][1..^1], 0);

            Element left = ReadNextElement(textLeft, 0);
            Element right = ReadNextElement(textRight, 0);

            //if (left.Type is ElementType.Ended) {
            //    break;
            //}
            //else if (right.Type is ElementType.Ended) {
            //    isCorrect = false;
            //    break;
            //}

            //if (!RecursiveCheck(textLeft, textRight, left, right)) {
            //    isCorrect = false;
            //    break;
            //}

            if (isCorrect) {
                indexes.Add(pairCount);
            }
        }

        var lala = "";
        Console.WriteLine();
    }

    //internal static bool RecursiveCheck(string textLeft, string textRight, Element left, Element right) {
    //    if (left.Type is ElementType.Number && right.Type is ElementType.Number) {
    //        if (int.Parse(left.Text) > int.Parse(right.Text)) {
    //            return false;
    //        }
    //    }
    //    else if (left.Type != right.Type) {
    //        left = left.Type is ElementType.Number ? left : ReadNextElement($"[[{left.Text}]]", 0);
    //        right = right.Type is ElementType.Number ? right : ReadNextElement($"[[{right.Text}]]", 0);
    //        return RecursiveCheck(left, right);
    //    }
    //    else if (left.Type is ElementType.List && right.Type is ElementType.List) {
    //        left = ReadNextElement(left.Text, 0);
    //        right = ReadNextElement(right.Text, 0);
            
    //        if (RecursiveCheck(left, right)) {

    //        }
    //    }

    //    return true;
    //}

    internal static Element ReadNextElement(string text, int startIndex) {
        //if (startIndex == text.Length - 1) {
        //    return new Element(text, ElementType.Ended);
        //}

        startIndex += text.StartsWith(",") ? 1 : 0;
        text = text[startIndex..];

        if (text[0] == '[' && text[1] == '[') {
            int openingBrackets = 1;
            int closingBrackets = 0;
            int endIndex = 0;

            for (int i = 1; i < text.Length; i++) {
                openingBrackets += text[i] == '[' ? 1 : 0;
                closingBrackets += text[i] == ']' ? 1 : 0;

                if (openingBrackets == closingBrackets) {
                    endIndex = i;
                    break;
                }
            }

            return new Element($"{text[..endIndex]}]");
        }
        else if (text[0] == '[') {
            return new Element(int.Parse(text[1].ToString()));
        }
        else {
            return new Element(int.Parse(text[0].ToString()));
        }
    }

    internal class Element {
        public Element(object content) {
            Content = content;
        }

        public object Content { get; set; }

        //public override string ToString() {
        //    return Type switch {
        //        ElementType.Number => $"{Text}",
        //        ElementType.List => $"{Text}",
        //        ElementType.Ended => "ENDED"
        //    };
        //}
    }

    internal class TextCursor {
        public TextCursor(string text, int position) {
            Text = text;
            Position = position;
        }

        public string Text { get; set; }
        public int Position { get; set; }
    }
}

internal enum ElementType {
    Number,
    List,
    Ended,
}
