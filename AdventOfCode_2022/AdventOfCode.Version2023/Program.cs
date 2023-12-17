﻿using AdventOfCode.Core.Models;
using AdventOfCode.Version2023;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
Console.ForegroundColor = ConsoleColor.Cyan;

Stopwatch sw = new();
sw.Start();
var day = new Day_16();
day.Solve(AoCResourceType.Solution);
Console.WriteLine($"{sw.ElapsedMilliseconds}ms");