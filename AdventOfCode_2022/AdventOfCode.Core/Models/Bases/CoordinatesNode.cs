﻿namespace AdventOfCode.Core.Models.Bases;

public abstract class CoordinatesNode : NodeBase
{
    public Coordinates Position { get; set; }

    public override bool Equals(object? obj) => ((CoordinatesNode)obj).Position.Equals(Position);
    public override int GetHashCode() => Position.GetHashCode();
}
