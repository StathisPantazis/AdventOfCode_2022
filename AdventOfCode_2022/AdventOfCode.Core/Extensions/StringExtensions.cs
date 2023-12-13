using AdventOfCode.Core.Models;

namespace AdventOfCode.Core.Extensions;

public static class StringExtensions
{
    public static int AoCDay(this string str) => int.Parse(str.Split('_')[1]);

    public static int FromBinaryToInt(this string binary) => Convert.ToInt32(binary, 2);

    public static string ReverseString(this string str) => string.Join(string.Empty, str.Reverse());

    public static int Count(this string str, char character) => str.Count(x => x == character);
    public static int CountImmediate(this string str, params char[] characters) => str.TakeWhile((c, i) => i == 0 || characters.Contains(c)).Count(c => characters.Contains(c));

    public static bool HasAt(this string str, char character, int index) => str[index] == character;

    /// <summary>
    /// abcd, '2' --> bcd 
    /// </summary>
    public static string CropFrom(this string str, char character) => str.CropFrom(character, out var _, out var _);
    /// <summary>
    /// 1234, "2" --> 34
    /// </summary>
    public static string CropFrom(this string str, string stringToMatch) => str.CropFrom(stringToMatch, out var _, out var _);
    /// <summary>
    /// 1234, 1 --> 34
    /// </summary>
    public static string CropFrom(this string str, int index) => str.CropFrom(index, out var _, out var _);
    /// <summary>
    /// abcd, '2' --> bcd 
    /// </summary>
    public static string CropFrom(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropFrom(character.ToString(), out stringChanged, out charsCropped);
    /// <summary>
    /// 1234, "2" --> 34
    /// </summary>
    public static string CropFrom(this string str, string stringToMatch, out bool stringChanged, out int charsCropped) => str.CropFrom(str.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    /// <summary>
    /// 1234, 1 --> 34
    /// </summary>
    public static string CropFrom(this string str, int index, out bool stringChanged, out int charsCropped)
    {
        stringChanged = false;
        charsCropped = 0;

        str = str.StringAfterBorderCheck(index, out var isOutsideOfBorders);
        if (isOutsideOfBorders)
        {
            return str;
        }

        var result = str[index..];
        charsCropped = str.Length - result.Length;
        stringChanged = charsCropped > 0;
        return result;
    }

    public static string CropAfter(this string str, char character) => str.CropAfter(character, out _, out _);
    public static string CropAfter(this string str, string stringToMatch) => str.CropAfter(stringToMatch, out _, out _);
    public static string CropAfter(this string str, int index) => str.CropAfter(index, out _, out _);
    public static string CropAfter(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropAfter(character.ToString(), out stringChanged, out charsCropped);
    // TODO :: CROP AFTER DOESNT WORK FOR string
    public static string CropAfter(this string str, string stringToMatch, out bool stringChanged, out int charsCropped) => str.CropAfter(str.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public static string CropAfter(this string str, int index, out bool stringChanged, out int charsCropped)
    {
        stringChanged = false;
        charsCropped = 0;

        str = str.StringAfterBorderCheck(index + 1, out var isOutsideOfBorders);
        if (isOutsideOfBorders)
        {
            return str;
        }

        var result = str.CropFrom(index + 1);
        charsCropped = str.Length - result.Length;
        stringChanged = charsCropped > 0;
        return result;
    }

    /// <summary>
    /// Does not include character.
    /// </summary>
    public static string CropUntil(this string str, char character) => str.CropUntil(character, out _, out _);
    /// <summary>
    /// Does not include string.
    /// </summary>
    public static string CropUntil(this string str, string stringToMatch) => str.CropUntil(stringToMatch, out _, out _);
    /// <summary>
    /// Does not include index.
    /// </summary>
    public static string CropUntil(this string str, int index) => str.CropUntil(index, out _, out _);
    /// <summary>
    /// Does not include character.
    /// </summary>
    public static string CropUntil(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropUntil(character.ToString(), out stringChanged, out charsCropped);
    /// <summary>
    /// Does not include string.
    /// </summary>
    public static string CropUntil(this string str, string stringToMatch, out bool stringChanged, out int charsCropped) => str.CropUntil(str.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    /// <summary>
    /// Does not include index.
    /// </summary>
    public static string CropUntil(this string str, int index, out bool stringChanged, out int charsCropped)
    {
        stringChanged = false;
        charsCropped = 0;

        str = str.StringAfterBorderCheck(index, out var isOutsideOfBorders);
        if (isOutsideOfBorders)
        {
            return str;
        }

        var result = str[..index];
        charsCropped = str.Length - result.Length;
        stringChanged = charsCropped > 0;
        return result;
    }

    public static string CropUntilWith(this string str, char character) => str.CropUntilWith(character, out _, out _);
    public static string CropUntilWith(this string str, string stringToMatch) => str.CropUntilWith(stringToMatch, out _, out _);
    public static string CropUntilWith(this string str, int index) => str.CropUntil(index, out _, out _);
    public static string CropUntilWith(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropUntilWith(character.ToString(), out stringChanged, out charsCropped);
    public static string CropUntilWith(this string str, string stringToMatch, out bool stringChanged, out int charsCropped) => str.CropUntilWith(str.IndexOf(stringToMatch), out stringChanged, out charsCropped);
    public static string CropUntilWith(this string str, int index, out bool stringChanged, out int charsCropped) => str.CropUntil(index + 1, out stringChanged, out charsCropped);

    public static string RemoveOnce(this string str, string stringToRemove) => str.CropUntil(stringToRemove) + str.CropAfter(stringToRemove);
    public static string RemoveOnceFromEnd(this string str, string stringToRemove)
    {
        str = str.ReverseString();
        stringToRemove = stringToRemove.ReverseString();
        str = str.CropUntil(stringToRemove) + str.CropAfter(stringToRemove);
        return str.ReverseString();
    }

    public static string Add(this string str, string stringToAdd) => $"{str}{stringToAdd}";
    public static string Add(this string str, params string[] stringsToAdd)
    {
        foreach (var stringToAdd in stringsToAdd)
        {
            str = str.Add(stringToAdd);
        }

        return str;
    }

    public static List<int> IndexesOf(this string str, char character)
    {
        var indexes = new List<int>();

        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] == character)
            {
                indexes.Add(i);
            }
        }

        return indexes;
    }

    public static List<string> GetAllCombinations(this string baseString, char combinationCharacter, int count, bool includeBrokenPairs = true) => baseString.GetAllCombinations(combinationCharacter, count, count, includeBrokenPairs);

    /// <summary>
    /// Gets all combinations on the baseString with the combinationCharacter: .... --> o... , .o.. , ..o. , ...o , oo.. , ... ... ...
    /// </summary>
    /// <param name="fromCount">Minimum number of characters for the result combinations.</param>
    /// <param name="toCount">Maximum number of characters for the result combinations.</param>
    /// <param name="includeBrokenPairs">Broken pairs are .... --> o.o. , .o.o , o..o</param>
    /// <returns></returns>
    public static List<string> GetAllCombinations(this string baseString, char combinationCharacter, int? fromCount = null, int? toCount = null, bool includeBrokenPairs = true)
    {
        var combinations = new HashSet<string>();
        var start = fromCount is null ? 1 : fromCount.Value.AtLeast(1);
        var end = toCount is null ? baseString.Length + 1 : toCount.Value.AtMost(baseString.Length + 1);
        end = start == end ? end + 1 : end;

        for (var repeats = start; repeats < end; repeats++)
        {
            if (includeBrokenPairs && start != 0 && !combinations.Any())
            {
                repeats--;
            }

            if (includeBrokenPairs && combinations.Any())
            {
                var previousCombinations = combinations.Where(x => x.Count(combinationCharacter) == repeats - 1).ToList();
                var newCombinations = new List<string>();
                var total = previousCombinations.Count;

                foreach (var previousCombination in previousCombinations)
                {
                    for (var j = 0; j < baseString.Length; j++)
                    {
                        if (baseString[j] == combinationCharacter)
                        {
                            continue;
                        }

                        var newCombination = previousCombination.ReplaceAt(combinationCharacter, j);
                        combinations.Add(newCombination);
                    }
                }
            }
            else
            {
                for (var j = 0; j < baseString.Length; j++)
                {
                    if (j + repeats > baseString.Length)
                    {
                        continue;
                    }

                    var combination = baseString
                        .AsFluent()
                        .CropUntil(j)
                        .AddRepeat(combinationCharacter, repeats)
                        .AddCropFrom(j + repeats);

                    combinations.Add(combination);
                }
            }
        }

        return includeBrokenPairs && start > 0
            ? combinations.Where(x => x.Count(combinationCharacter) > start - 1).ToList()
            : combinations.ToList();
    }

    public static string ReplaceAt(this string str, char newCharacter, int index) => $"{str.CropUntil(index)}{newCharacter}{str.CropAfter(index)}";

    public static FluentString AsFluent(this string str) => new(str);

    public static string Repeat(this char character, int times) => new(character, times);

    private static string StringAfterBorderCheck(this string str, int index, out bool isOutsideOfBorders)
    {
        isOutsideOfBorders = true;

        if (index < 0)
        {
            return str;
        }
        else if (index >= str.Length)
        {
            return string.Empty;
        }

        isOutsideOfBorders = false;
        return str;
    }
}
