namespace AdventOfCode_2022.Utils;

internal static class Helpers {
    public static string[] FileCleanReadLines(int day, string extraPath = "") {
        IEnumerable<string> lines = File.ReadLines($@"C:\Source\AdventOfCode_2022\AdventOfCode_2022\AdventOfCode_2022\Resources\day_{day}{extraPath}.txt");
        lines = lines.Select(x => x.Replace("\r", string.Empty));
        return lines.ToArray();
    }

    public static string[] TextCleanReadLines(string text) {
        return text.Replace("\r", string.Empty).Split("\n").ToArray();
    }

    public static string FileCleanReadText(int day, string extraPath = "") {
        return File.ReadAllText($@"C:\Source\AdventOfCode_2022\AdventOfCode_2022\AdventOfCode_2022\Resources\day_{day}{extraPath}.txt").Replace("\r", string.Empty);
    }
}
