using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models;

public class FileDescription
{
    public FileDescription(IDay day, AoCResourceType resourceType)
    {
        AoCDay = day.ToString().AoCDay();
        AoCYear = day.GetType().AoCYear();
        ResourceType = resourceType;
    }

    public int AoCDay { get; init; }
    public int AoCYear { get; init; }
    public AoCResourceType ResourceType { get; init; }
}
