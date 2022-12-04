namespace AdventOfCode_2022;

internal static class Day_1 {
    public static void Solve() {
        string text = File.ReadAllText(@"C:\Source\AdventOfCode_2022\AdventOfCode_2022\AdventOfCode_2022\Resources\day_1.txt");
    
        IEnumerable<string> cals = text.Split("\n\r\n").Select(x => x.Replace("\r", ""));
        IEnumerable<int> calsTotals = cals.Select(x => x.Split('\n').Select(y => int.Parse(y)).Sum());
        int[] calsTotals2 = calsTotals.OrderByDescending(x => x).ToArray();

        Console.WriteLine(calsTotals2[0] + calsTotals2[1] + calsTotals2[2]);
    }
}
