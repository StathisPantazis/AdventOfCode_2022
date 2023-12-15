using AdventOfCode.Core.Extensions;
using AdventOfCode.Core.Models.Interfaces;

namespace AdventOfCode.Core.Models;

public class FileDescription(IDay day, AoCResourceType resourceType)
{
    public string AoCDay { get; init; } = day.ToString().AoCDay() is int dayParsed && dayParsed < 10 ? $"0{dayParsed}" : dayParsed.ToString();
    public int AoCYear { get; init; } = day.GetType().AoCYear();
    public AoCResourceType ResourceType { get; init; } = resourceType;
}
