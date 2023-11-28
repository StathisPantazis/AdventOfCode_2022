using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;
using AdventOfCode.Core.Utils;

namespace AdventOfCode.Version2022;

public class Day_2 : AoCBaseDay<int, int, string[]>
{
    private static readonly Dictionary<int, Dictionary<string, int>> _dictPartPoints = new() {
        { 1, new Dictionary<string, int>() {
            { "L", 0 },
            { "D", 3 },
            { "W", 6 },
        }},
        { 2, new Dictionary<string, int>() {
            { "X", 0 },
            { "Y", 3 },
            { "Z", 6 },
        }}
    };

    private static readonly Dictionary<RPS, int> _rpsDict = new() {
        { RPS.Rock, 1 },
        { RPS.Paper, 2 },
        { RPS.Scizzor, 3 },
    };

    public override AoCSolution<int, int> Solve(AoCResourceType resourceType)
    {
        var games = Helpers.File_CleanReadLines(2, 2022, resourceType);
        return new AoCSolution<int, int>(Part1(games), Part2(games));
    }

    protected override int Part1(string[] games)
    {
        var points = 0;
        foreach (var game in games)
        {
            var enemy = ParseRps(game[0].ToString());
            var self = ParseRps(game.Last().ToString());

            var result = self == enemy ? "D" :
                self switch
                {
                    RPS.Rock when enemy is RPS.Scizzor => "W",
                    RPS.Paper when enemy is RPS.Rock => "W",
                    RPS.Scizzor when enemy is RPS.Paper => "W",
                    _ => "L"
                };

            points += _dictPartPoints[1][result] + _rpsDict[self];
        }

        return points;
    }

    protected override int Part2(string[] games)
    {
        var points = 0;
        foreach (var game in games)
        {
            var enemy = ParseRps(game[0].ToString());
            var result = game.Last().ToString();

            var selfRPS = result == "Y" ? enemy :
                enemy switch
                {
                    RPS.Rock when result == "Z" => RPS.Paper,
                    RPS.Rock => RPS.Scizzor,
                    RPS.Paper when result == "Z" => RPS.Scizzor,
                    RPS.Paper => RPS.Rock,
                    RPS.Scizzor when result == "Z" => RPS.Rock,
                    _ => RPS.Paper
                };

            points += _dictPartPoints[2][result] + _rpsDict[selfRPS];
        }

        return points;
    }

    private static RPS ParseRps(string letter)
    {
        return letter is "A" or "X" ? RPS.Rock : letter is "B" or "Y" ? RPS.Paper : RPS.Scizzor;
    }

    public enum RPS
    {
        Rock,
        Paper,
        Scizzor,
    }
}