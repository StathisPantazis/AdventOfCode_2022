using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;
using static AdventOfCode.Version2023.Day_02;

namespace AdventOfCode.Version2023;

public class Day_02 : AoCBaseDay<int, int, Game[]>
{
    private const string _red = "red";
    private const string _blue = "blue";
    private const string _green = "green";

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var texta = Helpers.FileCleanReadText(FileDescription(this, resourceType));

        var text = Helpers.FileCleanReadText(FileDescription(this, resourceType))
            .Replace("Game ", string.Empty)
            .Replace(": ", ":")
            .Replace(", ", ",")
            .Replace("; ", ";");

        var games = Helpers.TextCleanReadLines(text)
            .Select(x => x.Split(':') is string[] arr ? new Game(int.Parse(arr[0]), arr[1]) : new Game(0, string.Empty))
            .ToArray();

        return Solution(games);
    }

    protected override int Part1(Game[] games)
    {
        foreach (var game in games)
        {
            var sets = game.Draws.Split(';');

            foreach (var set in sets)
            {
                var colors = set.Split(',');

                foreach (var color in colors)
                {
                    var draw = color.Split(' ');
                    var number = int.Parse(draw[0]);

                    game.IsPossible = draw[1] switch
                    {
                        _red when number <= 12 => true,
                        _green when number <= 13 => true,
                        _blue when number <= 14 => true,
                        _ => false
                    };

                    if (!game.IsPossible)
                    {
                        break;
                    }
                }

                if (!game.IsPossible)
                {
                    break;
                }
            }
        }

        return games
            .Where(x => x.IsPossible)
            .Sum(x => x.Id);
    }

    protected override int Part2(Game[] games)
    {
        foreach (var game in games)
        {
            var sets = game.Draws.Split(';');

            foreach (var set in sets)
            {
                var colors = set.Split(',');

                foreach (var color in colors)
                {
                    var draw = color.Split(' ');
                    var number = int.Parse(draw[0]);

                    if (game.ColorCount[draw[1]] < number)
                    {
                        game.ColorCount[draw[1]] = number;
                    }
                }
            }
        }

        return games.Sum(x => x.Power);
    }

    public class Game
    {
        public Game(int id, string draws)
        {
            Id = id;
            Draws = draws;
        }

        public int Id { get; set; }
        public string Draws { get; set; }
        public Dictionary<string, int> ColorCount { get; set; } = new()
        {
            { _blue, 0 },
            { _red, 0 },
            { _green, 0 },
        };
        public bool IsPossible { get; set; }
        public int Power => ColorCount[_red] * ColorCount[_green] * ColorCount[_blue];

        public override string ToString() => $"{Id}] {Draws}";
    }
}