using AdventOfCode_2022.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode_2022;

internal static class Day_6 {
    public static void Solve() {
        string text = Helpers.FileCleanReadText(6);

        for (int part_count = 4; part_count < 15; part_count += 10) {
            string marker = text[..part_count];
            int meter = 0;

            while (marker.ToCharArray().Distinct().Count() < part_count) {
                meter++;
                marker = text.Substring(meter, part_count);
            }

            Console.WriteLine($"Distinct: {part_count} - Index: {text.IndexOf(marker) + part_count}");
        }
    }
}
