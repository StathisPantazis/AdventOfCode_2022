using AdventOfCode.Core.Extensions;

namespace AdventOfCode.Core.Models;

public class FluentString
{
    private readonly string _init;
    private string _value;

    public FluentString(string value)
    {
        _init = value;
        _value = value;
    }

    public static implicit operator string(FluentString fluentString) => fluentString._value;

    public FluentString CropFrom(char character) => CropFrom(character, out var _, out var _);
    public FluentString CropFrom(string stringToMatch) => CropFrom(stringToMatch, out var _, out var _);
    public FluentString CropFrom(int index) => CropFrom(index, out var _, out var _);
    public FluentString CropFrom(char character, out bool stringChanged, out int charsCropped) => CropFrom(character.ToString(), out stringChanged, out charsCropped);
    public FluentString CropFrom(string stringToMatch, out bool stringChanged, out int charsCropped) => CropFrom(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString CropFrom(int index, out bool stringChanged, out int charsCropped)
    {
        _value = _value.CropFrom(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString AddCropFrom(char character) => AddCropFrom(character, out var _, out var _);
    public FluentString AddCropFrom(string stringToMatch) => AddCropFrom(stringToMatch, out var _, out var _);
    public FluentString AddCropFrom(int index) => AddCropFrom(index, out var _, out var _);
    public FluentString AddCropFrom(char character, out bool stringChanged, out int charsCropped) => AddCropFrom(character.ToString(), out stringChanged, out charsCropped);
    public FluentString AddCropFrom(string stringToMatch, out bool stringChanged, out int charsCropped) => AddCropFrom(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString AddCropFrom(int index, out bool stringChanged, out int charsCropped)
    {
        _value += _init.CropFrom(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString CropAfter(char character) => CropAfter(character, out _, out _);
    public FluentString CropAfter(string stringToMatch) => CropAfter(stringToMatch, out _, out _);
    public FluentString CropAfter(int index) => CropAfter(index, out _, out _);
    public FluentString CropAfter(char character, out bool stringChanged, out int charsCropped) => CropAfter(character.ToString(), out stringChanged, out charsCropped);
    public FluentString CropAfter(string stringToMatch, out bool stringChanged, out int charsCropped) => CropAfter(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString CropAfter(int index, out bool stringChanged, out int charsCropped)
    {
        _value = _init.CropAfter(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString AddCropAfter(char character) => AddCropAfter(character, out _, out _);
    public FluentString AddCropAfter(string stringToMatch) => AddCropAfter(stringToMatch, out _, out _);
    public FluentString AddCropAfter(int index) => AddCropAfter(index, out _, out _);
    public FluentString AddCropAfter(char character, out bool stringChanged, out int charsCropped) => AddCropAfter(character.ToString(), out stringChanged, out charsCropped);
    public FluentString AddCropAfter(string stringToMatch, out bool stringChanged, out int charsCropped) => AddCropAfter(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString AddCropAfter(int index, out bool stringChanged, out int charsCropped)
    {
        _value += _init.CropAfter(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString CropUntil(char character) => CropUntil(character, out _, out _);
    public FluentString CropUntil(string stringToMatch) => CropUntil(stringToMatch, out _, out _);
    public FluentString CropUntil(int index) => CropUntil(index, out _, out _);
    public FluentString CropUntil(char character, out bool stringChanged, out int charsCropped) => CropUntil(character.ToString(), out stringChanged, out charsCropped);
    public FluentString CropUntil(string stringToMatch, out bool stringChanged, out int charsCropped) => CropUntil(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString CropUntil(int index, out bool stringChanged, out int charsCropped)
    {
        _value = _init.CropUntil(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString CropUntilWith(char character) => CropUntilWith(character, out _, out _);
    public FluentString CropUntilWith(string stringToMatch) => CropUntilWith(stringToMatch, out _, out _);
    public FluentString CropUntilWith(int index) => CropUntil(index, out _, out _);
    public FluentString CropUntilWith(char character, out bool stringChanged, out int charsCropped) => CropUntilWith(character.ToString(), out stringChanged, out charsCropped);
    public FluentString CropUntilWith(string stringToMatch, out bool stringChanged, out int charsCropped) => CropUntilWith(_init.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public FluentString CropUntilWith(int index, out bool stringChanged, out int charsCropped)
    {
        _value = _init.CropUntilWith(index, out stringChanged, out charsCropped);
        return this;
    }

    public FluentString Add(string stringToAdd)
    {
        _value = _value.Add(stringToAdd);
        return this;
    }
    public FluentString Add(params string[] stringsToAdd)
    {
        _value = _value.Add(stringsToAdd);
        return this;
    }

    public FluentString AddRepeat(char character, int times)
    {
        _value += character.Repeat(times);
        return this;
    }

    public override string ToString() => _value;
}