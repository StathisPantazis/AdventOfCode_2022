using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Tests.Shared;

public class Point : GridInput<string>
{
    public Point()
    {
        EmptyValue = ".";
        ValueToFillWithEmptyName = nameof(Text);
        Id = Guid.NewGuid().ToString();
    }

    public Point(string text) : this()
    {
        Text = text;
    }

    public string Id { get; }
    public string Text { get; set; } = string.Empty;

    public override bool Equals(object? obj) => obj is Point point && point.Text == Text;
    public override int GetHashCode() => Text.GetHashCode();
    public override string ToString() => Text;
}