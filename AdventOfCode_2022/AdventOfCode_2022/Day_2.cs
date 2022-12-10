using AdventOfCode_2022.Utils;

namespace AdventOfCode_2022;

internal static class Day_2 {
    private static readonly Dictionary<int, Dictionary<string, int>> dictPartPoints = new() {
        { 1, new Dictionary<string, int>() {
            { "X", 1 },
            { "Y", 2 },
            { "Z", 3 },
        }},
        { 2, new Dictionary<string, int>() {
            { "X", 0 },
            { "Y", 3 },
            { "Z", 6 },
        } }
    };

    private static Dictionary<RPS, int> rpsDict = new() {
            { RPS.Rock, 1 },
            { RPS.Paper, 2 },
            { RPS.Scizzor, 3 },
        };

    public static void Solve() {
        string[] games = Helpers.File_CleanReadLines(2);
        Console.WriteLine(Points_Part1(games));
        Console.WriteLine(Points_Part2(games));
    }

    public static int Points_Part2(string[] games) {
        int points = 0;
        foreach (string game in games) {
            RPS enemy = ParseRps(game[0].ToString());
            string result = game.Last().ToString();

            RPS selfRPS = result == "Y" ? enemy :
                enemy switch {
                    RPS.Rock when result == "Z" => RPS.Paper,
                    RPS.Rock => RPS.Scizzor,
                    RPS.Paper when result == "Z" => RPS.Scizzor,
                    RPS.Paper => RPS.Rock,
                    RPS.Scizzor when result == "Z" => RPS.Rock,
                    _ => RPS.Paper
                };

            points += dictPartPoints[2][result] + rpsDict[selfRPS];
        }

        return points;
    }

    public static int Points_Part1(string[] games) {
        int points = 0;
        foreach (string game in games) {
            RPS enemy = ParseRps(game[0].ToString());
            RPS self = ParseRps(game.Last().ToString());

            string result = self == enemy ? "D" :
                self switch {
                    RPS.Rock when enemy is RPS.Scizzor => "W",
                    RPS.Paper when enemy is RPS.Rock => "W",
                    RPS.Scizzor when enemy is RPS.Paper => "W",
                    _ => "L"
                };

            points += dictPartPoints[1][result] + rpsDict[self];
        }

        return points;
    }

    private static RPS ParseRps(string letter) {
        return letter is "A" or "X" ? RPS.Rock : letter is "B" or "Y" ? RPS.Paper : RPS.Scizzor;
    }
}

public enum RPS {
    Rock,
    Paper,
    Scizzor,
}