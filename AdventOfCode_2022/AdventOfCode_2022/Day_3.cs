using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_3 {
    public static void Solve() {
        string[] comps = Helpers.FileCleanReadLines(3);
        List<char> types = Enumerable.Range('a', 26).ToList().Union(Enumerable.Range('A', 26)).Select(x => (char)x).ToList();
        Console.WriteLine(Part_1(comps, types));
        Console.WriteLine(Part_2(comps, types));
    }

    private static int Part_2(string[] comps, List<char> types) {
        int sum = 0;

        for (int i = 0; i < comps.Length; i += 3) {
            List<char> pt1 = comps[i].Select(x => x).ToList();
            List<char> pt2 = comps[i + 1].Select(x => x).ToList();
            List<char> pt3 = comps[i + 2].Select(x => x).ToList();

            char same = pt1.Intersect(pt2).Intersect(pt3).First();
            sum += types.IndexOf(same) + 1;
        }

        return sum;
    }

    private static int Part_1(string[] comps, List<char> types) {
        int sum = 0;

        foreach (string comp in comps) {
            string pt1 = comp[..(comp.Length / 2)];
            string pt2 = comp[(comp.Length / 2)..];
            char dif = pt1.FirstOrDefault(pt2.Contains);
            sum += types.IndexOf(dif) + 1;
        }

        return sum;
    }
}
