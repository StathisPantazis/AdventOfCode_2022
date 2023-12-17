using AdventOfCode.Core.Models.Bases;

namespace AdventOfCode.Core.SearchAlgorithms;

public abstract class DijkstraNode : CoordinatesNode
{
    public int Cost { get; set; }
}
