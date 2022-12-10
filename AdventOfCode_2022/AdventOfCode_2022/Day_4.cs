using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_4 {
    public static void Solve() {
        string text = Helpers.File_CleanReadText(4).Replace(",", "-");
        List<int[]> pairs = Helpers.Text_CleanReadLines(text)
            .Select(x => x.Split('-').Select(y => int.Parse(y.ToString())).ToArray())
            .ToList();

        int count_pt1 = pairs.Count(x => (x[0] <= x[2] && x[1] >= x[3]) || (x[2] <= x[0] && x[3] >= x[1]));
        int count_pt2 = pairs.Count(x => (x[0] <= x[2] && x[1] >= x[2]) || (x[2] <= x[0] && x[3] >= x[0]));
    }
}
