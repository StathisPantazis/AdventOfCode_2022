namespace AdventOfCode.Core.Tests;

public abstract class Lala
{
    protected abstract bool Yes { get; set; }
}

public readonly struct FastCoordinates(int x, int y)
{
    public ushort X { get; } = (ushort)x;
    public ushort Y { get; } = (ushort)y;

    public bool La5 => X == 4;

    public FastCoordinates Copy() => new(X, Y);
}

public class FastCoordinatesClass(int x, int y)
{
    public ushort X { get; } = (ushort)x;
    public ushort Y { get; } = (ushort)y;

    public bool La5 => X == 4;

    public FastCoordinatesClass Copy() => new(X, Y);
}

public class FastPoint(int x, int y, string text)
{
    public FastCoordinates FastCoordinates = new(x, y);

    public string Text = text;

    public override string ToString() => $"{Text} - ({FastCoordinates.X} , {FastCoordinates.Y})";
}