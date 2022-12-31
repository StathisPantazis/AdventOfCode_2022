using AdventOfCode_2022.Models;

namespace AdventOfCode_2022.Utils;

internal static class Helpers {
    public static string GetPath(int day, string extraPath = "") => 
        @$"{Path.Combine(Path.GetFullPath(@"..\..\..\"), "Resources", $"day_{day}{extraPath}.txt")}";

    public static string[] File_CleanReadLines(int day, string extraPath = "") {
        IEnumerable<string> lines = File.ReadLines(GetPath(day, extraPath));
        lines = lines.Select(x => x.Replace("\r", string.Empty));
        return lines.ToArray();
    }

    public static string[] Text_CleanReadLines(string text) {
        return text.Replace("\r", string.Empty).Split("\n").ToArray();
    }

    public static string File_CleanReadText(int day, string extraPath = "") {
        return File_ReadText(day, extraPath).Replace("\r", string.Empty);
    }

    public static string File_ReadText(int day, string extraPath = "") {
        return File.ReadAllText(GetPath(day, extraPath));
    }

    public static Direction GetDirection(string str) {
        return str.ToLower() switch {
            "u" => Direction.U,
            "d" => Direction.D,
            "r" => Direction.R,
            "l" => Direction.L,
            _ => throw new Exception("Wrong direction")
        };
    }

    public static List<string> GetGriddedStringList(int rows, int columns, char character) {
        return Enumerable.Range(0, rows).Select(i => new string(character, columns)).ToList();
    }

    public static List<string> GetGriddedStringList(int borders, char character) {
        return Enumerable.Range(0, borders).Select(i => new string(character, borders)).ToList();
    }
}
