using AdventOfCode.Core.Models;

namespace AdventOfCode.Core.Utils;

public static class Helpers
{
    public static string[] File_CleanReadLines(FileDescription fileDescription)
    {
        var lines = File.ReadLines(GetPath(fileDescription));
        lines = lines.Select(x => x.Replace("\r", string.Empty));
        return lines.ToArray();
    }

    public static string[] Text_CleanReadLines(string text)
    {
        return text.Replace("\r", string.Empty).Split("\n").ToArray();
    }

    public static string File_CleanReadText(FileDescription fileDescription)
    {
        return File_ReadText(fileDescription).Replace("\r", string.Empty);
    }

    public static string File_ReadText(FileDescription fileDescription)
    {
        return File.ReadAllText(GetPath(fileDescription));
    }

    public static Direction GetDirection(string str)
    {
        return str.ToLower() switch
        {
            "u" => Direction.U,
            "d" => Direction.D,
            "r" => Direction.R,
            "l" => Direction.L,
            _ => throw new Exception("Wrong direction")
        };
    }

    public static List<string> GetGriddedStringList(int rows, int columns, char character)
    {
        return Enumerable.Range(0, rows).Select(i => new string(character, columns)).ToList();
    }

    public static List<string> GetGriddedStringList(int borders, char character)
    {
        return Enumerable.Range(0, borders).Select(i => new string(character, borders)).ToList();
    }

    private static string GetPath(FileDescription fileDescription)
    {
        var resource = fileDescription.ResourceType is AoCResourceType.Example ? "_0" : "_1";
        var fullPath = @$"{Path.Combine(Path.GetFullPath(@"..\..\..\"), "Resources", $"day_{fileDescription.AoCDay}{resource}.txt")}";

        if (fullPath.Contains("Tests"))
        {
            fullPath = fullPath.Replace("Tests", $"Version{fileDescription.AoCYear}");
        }

        return fullPath;
    }
}
