﻿using AdventOfCode.Core.Models;
using AdventOfCode.Version2023;
using System.Text;

Console.OutputEncoding = Encoding.GetEncoding("UTF-8");
Console.ForegroundColor = ConsoleColor.Cyan;

var day = new Day_16();
day.Solve(AoCResourceType.Solution);