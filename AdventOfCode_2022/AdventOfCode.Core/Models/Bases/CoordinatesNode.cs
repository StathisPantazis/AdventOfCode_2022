namespace AdventOfCode.Core.Models.Bases;

public abstract class CoordinatedBase
{
    public Coordinates Position { get; set; }

    public override bool Equals(object? obj) => ((CoordinatedBase)obj).Position.Equals(Position);
    public override int GetHashCode() => Position.GetHashCode();
}

public abstract class CoordinatesNode : NodeBase
{
    public CoordinatesNode() { }

    public CoordinatesNode(Coordinates position)
    {
        Position = position;
    }

    public Coordinates Position { get; set; }

    public override bool Equals(object? obj) => ((CoordinatesNode)obj).Position.Equals(Position);
    public override int GetHashCode() => Position.GetHashCode();
}
