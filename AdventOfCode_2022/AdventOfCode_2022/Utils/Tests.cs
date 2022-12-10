namespace AdventOfCode_2022.Utils;

internal static class Tests {
    public static void TestTraverse() {
        for (int i = 0; i < 2; i++) {
            bool startFromNegative = i == 0;
            Coordinates pos = new(10, 10, startFromNegative);
            Console.WriteLine($"Grid: {pos.X_Border + 1} x {pos.Y_Border + 1} - StartFromNegative = {startFromNegative}");
            Console.WriteLine(pos);

            while (pos.TraverseGrid()) {
                Console.WriteLine(pos);
            }

            Console.WriteLine();
        }
    }

    public static void TestTraverseReverse() {
        Coordinates pos = new(10, 10);
        pos.GoToEnd();
        Console.WriteLine($"Grid: {pos.X_Border + 1} x {pos.Y_Border + 1}");
        Console.WriteLine(pos);

        while (pos.TraverseGridReverse()) {
            Console.WriteLine(pos);
        }
    }

    public static void TestDirectionsTowards() {
        for (int i = 0; i < 2; i++) {
            bool allowOverlap = i != 0;

            Coordinates from = new(10, 10), to = from.Copy();

            to.GoToEnd();

            Console.WriteLine($"Grid: {to.X_Border + 1} x {to.Y_Border + 1} - AllowOverlap = {allowOverlap}");
            Console.WriteLine(from);

            while (from.MoveTowards(to, allowOverlap)) {
                Console.WriteLine(from);
            }

            Console.WriteLine();
        }
    }
}
