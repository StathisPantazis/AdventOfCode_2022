using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models;
using AdventOfCode.Version2023;
using System.Text;

var lala = '?'.Repeat(2)
    .GetAllCombinations('#', 1, includeBrokenPairs: true);

Console.WriteLine(lala.ListToString());

return;
Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
Console.ForegroundColor = ConsoleColor.Cyan;

var day = new Day_12();
day.Solve(AoCResourceType.Example);