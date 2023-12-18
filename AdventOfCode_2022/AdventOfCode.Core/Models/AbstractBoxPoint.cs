using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core;

public class AbstractBoxPoint(string text) : CoordinatedBase
{
    public string Text { get; set; } = text;

    public override string ToString() => Text;
}
