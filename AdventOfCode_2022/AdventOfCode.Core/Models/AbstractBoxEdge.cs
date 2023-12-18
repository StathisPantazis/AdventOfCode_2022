using AdventOfCode.Core.Models;
using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core;

public class AbstractBoxEdge
{
    public Coordinates Start { get; set; }
    public Coordinates AfterStartPosition { get; set; }
    public Coordinates End { get; set; }
    public Coordinates BeforeEndPosition { get; set; }
    public AbstractBoxEdge PreviousEdge { get; set; }
    public AbstractBoxEdge NextEdge { get; set; }
    public Direction InnerDirection { get; set; }

    public override string ToString() => $"({Start}) - ({End})";
}
