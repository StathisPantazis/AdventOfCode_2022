namespace AdventOfCode.Core.Models.Bases;

public abstract class Emptyable
{
    public string ValueToFillWithEmptyName { get; protected set; }
}

public abstract class GridInput<T> : Emptyable
{
    // Sample constructor
    // public ClassName()
    // {
    //     EmptyValue = ".";
    //     ValueToFillWithEmptyName = nameof(Text);
    // }

    public T EmptyValue { get; protected set; }
}
