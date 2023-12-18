namespace AdventOfCode.Core.Models.Bases;

public abstract class CoordinatedBase : ICoordinated
{
    public CoordinatedBase() { }

    public CoordinatedBase(Coordinates position)
    {
        Position = position;
    }

    public Coordinates Position { get; set; }

    public override bool Equals(object? obj) => ((CoordinatedBase)obj).Position.Equals(Position);
    public override int GetHashCode() => Position.GetHashCode();
}

public abstract class CoordinatesNode : NodeBase, ICoordinated
{
    public CoordinatesNode() { }

    public CoordinatesNode(Coordinates position)
    {
        Position = position;
    }

    public Coordinates Position { get; set; }

    public override bool Equals(object? obj) => ((CoordinatesNode)obj).Position.Equals(Position);
    public override int GetHashCode() => Position.GetHashCode();
    public override string ToString() => Position.ToString();
}

public interface ICoordinated
{
    Coordinates Position { get; set; }
}