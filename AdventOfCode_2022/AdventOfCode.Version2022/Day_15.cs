using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_15 : AoCBaseDay<int, int, List<((int x, int y) sensor, (int x, int y) beacon)>>
{
    private static readonly string[] _blocks = new string[] { "S", "B", "#" };
    private static readonly Direction[] _rotateDirection = new Direction[] { Direction.UR, Direction.DR, Direction.DL, Direction.UL };

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        List<((int x, int y) sensor, (int x, int y) beacon)> sensor_beacons = Helpers.File_CleanReadText(15, 2022, resourceType)
            .Replace("Sensor at x=", "")
            .Replace(" y=", "")
            .Replace("Sensor at x=", "")
            .Replace(": closest beacon is at x=", ",")
            .Split('\n')
            .Select(x => ((int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1])), (int.Parse(x.Split(',')[2]), int.Parse(x.Split(',')[3]))))
            .ToList();

        return new AoCSolution<int, int>(Part1(sensor_beacons), Part2(sensor_beacons));
    }

    protected override int Part1(List<((int x, int y) sensor, (int x, int y) beacon)> sensor_beacons)
    {
        var minX = sensor_beacons.Min(x => Math.Min(x.sensor.x, x.beacon.x)); ;
        var minY = sensor_beacons.Min(x => Math.Min(x.sensor.y, x.beacon.y)); ;
        var maxX = sensor_beacons.Max(x => Math.Max(x.sensor.x, x.beacon.x));
        var maxY = sensor_beacons.Max(x => Math.Max(x.sensor.y, x.beacon.y));

        var addedX = minX < 0 ? -minX : 0;
        var addedY = minY < 0 ? -minY : 0;
        var tunnels = Grid<string>.CreateGrid(maxY + addedY + 1, maxX + addedX + 1, '.');

        var sensors = sensor_beacons.Select(x => new Coordinates(tunnels, x.sensor.x + addedX, x.sensor.y + addedY)).ToList();
        var beacons = sensor_beacons.Select(x => new Coordinates(tunnels, x.beacon.x + addedX, x.beacon.y + addedY)).ToList();

        sensors.ForEach(sensor => tunnels[sensor] = "S");
        beacons.ForEach(beacon => tunnels[beacon] = "B");

        var _fillDirections = Enum.GetValues<Direction>().ToArray();

        foreach (var sensor in sensors)
        {
            Console.WriteLine(sensor);
            var distance = 0;
            var found = false;

            while (!found)
            {
                distance++;
                var check = sensor.GetFromDirectionWithDistance(Direction.L, distance);
                if (check.IsInsideOfBorder)
                {
                    tunnels[check] = "#";
                }

                foreach (var dir in _rotateDirection)
                {
                    while (check.Move(dir, allowOutside: true))
                    {
                        if (check.IsInsideOfBorder)
                        {
                            tunnels[check] = tunnels[check] == "B" ? "B" : "+";

                            var value = tunnels[check];
                            if (value == "B")
                            {
                                found = true;
                            }
                            else if (!_blocks.Contains(value))
                            {
                                tunnels[check] = "#";
                            }
                        }

                        if (sensor.X == check.X || sensor.Y == check.Y)
                        {
                            break;
                        }
                    }
                }
            }
        }

        var lala = tunnels.ToString();
        Console.WriteLine();

        return 0;
    }

    protected override int Part2(List<((int x, int y) sensor, (int x, int y) beacon)> args)
    {
        return 0;
    }
}
