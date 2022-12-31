using AdventOfCode_2022.Models;
using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_15 {
    private static readonly string[] _blocks = new string[] { "S", "B", "#" };
    private static readonly Direction[] rotateDirection = new Direction[] { Direction.UR, Direction.DR, Direction.DL, Direction.UL };

    internal static void Solve() {
        List<((int x, int y) sensor, (int x, int y) beacon)> sensor_beacons = Helpers.File_CleanReadText(15).Replace("Sensor at x=", "").Replace(" y=", "").Replace("Sensor at x=", "").Replace(": closest beacon is at x=", ",")
            .Split('\n')
            .Select(x => ((int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1])), (int.Parse(x.Split(',')[2]), int.Parse(x.Split(',')[3]))))
            .ToList();

        int minX = sensor_beacons.Min(x => Math.Min(x.sensor.x, x.beacon.x)); ;
        int minY = sensor_beacons.Min(x => Math.Min(x.sensor.y, x.beacon.y)); ;
        int maxX = sensor_beacons.Max(x => Math.Max(x.sensor.x, x.beacon.x));
        int maxY = sensor_beacons.Max(x => Math.Max(x.sensor.y, x.beacon.y));

        int addedX = minX < 0 ? -minX : 0;
        int addedY = minY < 0 ? -minY : 0;
        Grid<string> tunnels = Grid<string>.CreateGrid(maxY + addedY + 1, maxX + addedX + 1, '.');

        List<Coordinates> sensors = sensor_beacons.Select(x => new Coordinates(tunnels, x.sensor.x + addedX, x.sensor.y + addedY)).ToList();
        List<Coordinates> beacons = sensor_beacons.Select(x => new Coordinates(tunnels, x.beacon.x + addedX, x.beacon.y + addedY)).ToList();

        sensors.ForEach(sensor => tunnels[sensor] = "S");
        beacons.ForEach(beacon => tunnels[beacon] = "B");

        Direction[] _fillDirections = Enum.GetValues<Direction>().ToArray();

        foreach (Coordinates sensor in sensors) {
            Console.WriteLine(sensor);
            int distance = 0;
            bool found = false;

            while (!found) {
                distance++;
                Coordinates check = sensor.GetFromDirectionWithDistance(Direction.L, distance);
                if (check.IsInsideOfBorder) {
                    tunnels[check] = "#";
                }

                foreach (Direction dir in rotateDirection) {
                    while (check.Move(dir, allowOutside: true)) {
                        if (check.IsInsideOfBorder) {
                            tunnels[check] = tunnels[check] == "B" ? "B" : "+";

                            string value = tunnels[check];
                            if (value == "B") {
                                found = true;
                            }
                            else if (!_blocks.Contains(value)) {
                                tunnels[check] = "#";
                            }
                        }

                        if (sensor.X == check.X || sensor.Y == check.Y) {
                            break;
                        }
                    }
                }
            }
        }

        var lala = tunnels.ToString();
        Console.WriteLine();
    }
}
