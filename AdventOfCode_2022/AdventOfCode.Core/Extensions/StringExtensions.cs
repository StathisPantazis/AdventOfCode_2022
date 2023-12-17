namespace AdventOfCode.Core.Extensions;

public static class StringExtensions
{
    public static int AoCDay(this string str) => int.Parse(str.Split('_')[1]);

    public static int FromBinaryToInt(this string binary) => Convert.ToInt32(binary, 2);

    public static int ToAscii(this char character) => Convert.ToInt32(character);

    public static int ToAscii(this string str) => str.Length != 1 ? throw new InvalidDataException() : Convert.ToInt32(str[0]);

    public static string ReverseString(this string str) => string.Join(string.Empty, str.Reverse());

    public static string AddLastLimitLength(this string str, string addition, int length)
    {
        var newString = $"{str}{addition}";
        return newString.Length > length ? newString.CropFrom(addition.Length) : newString;
    }

    public static string AddFirstLimitLength(this string str, string addition, int length)
    {
        var newString = $"{str}{addition}";
        return newString.Length > length ? newString.CropUntil(str.Length - str.Length) : newString;
    }

    public static string[]? SplitMinLengthCheck(this string str, string separator, int minLength, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => str.Split(separator, stringSplitOptions) is string[] arr && arr.Length >= minLength ? arr : null;
    public static string[]? SplitMaxLengthCheck(this string str, string separator, int maxLength, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => str.Split(separator, stringSplitOptions) is string[] arr && arr.Length <= maxLength ? arr : null;
    public static string[]? SplitExactLengthCheck(this string str, string separator, int length, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        => str.Split(separator, stringSplitOptions) is string[] arr && arr.Length == length ? arr : null;

    public static int Count(this string str, char character) => str.Count(x => x == character);
    public static int CountImmediate(this string str, params char[] characters)
    {
        var count = 0;

        foreach (var c in str)
        {
            if (characters.Contains(c))
            {
                count++;
            }
            else
            {
                break;
            }
        }

        return count;
    }
    public static int CountFirstOccurence(this string str, params char[] characters)
    {
        var count = 0;

        foreach (var c in str)
        {
            if (characters.Contains(c))
            {
                count++;
            }
            else if (count != 0)
            {
                break;
            }
        }

        return count;
    }

    public static bool HasAt(this string str, char character, int index) => str[index] == character;

    public static string CropFrom(this string str, char character) => str.CropFrom(character, out var _, out var _);
    public static string CropFrom(this string str, string stringToMatch) => str.CropFrom(stringToMatch, out var _, out var _);
    public static string CropFrom(this string str, int index) => str.CropFrom(index, out var _, out var _);
    public static string CropFrom(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropFrom(str.IndexOf(character), out stringChanged, out charsCropped);
    public static string CropFrom(this string str, string stringToMatch, out bool stringChanged, out int charsCropped)
        => str.CropFrom(str.IndexOf(stringToMatch) is int index && str.IndexIsIn(index) ? index : -10, out stringChanged, out charsCropped);
    public static string CropFrom(this string str, int index, out bool stringChanged, out int charsCropped)
    {
        stringChanged = false;
        charsCropped = 0;

        if (str.IndexIsOut(index))
        {
            return str;
        }

        var result = str[index..];
        charsCropped = str.Length - result.Length;
        stringChanged = charsCropped > 0;
        return result;
    }

    public static string CropFromWithout(this string str, char character) => str.CropFromWithout(character, out var _, out var _);
    public static string CropFromWithout(this string str, string stringToMatch) => str.CropFromWithout(stringToMatch, out var _, out var _);
    public static string CropFromWithout(this string str, int index) => str.CropFromWithout(index, out var _, out var _);
    public static string CropFromWithout(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropFromWithout(str.IndexOf(character), out stringChanged, out charsCropped);
    public static string CropFromWithout(this string str, string stringToMatch, out bool stringChanged, out int charsCropped)
        => str.CropFromWithout(str.IndexOf(stringToMatch) is int index && str.IndexIsIn(index) ? index + stringToMatch.Length - 1 : -10, out stringChanged, out charsCropped);
    public static string CropFromWithout(this string str, int index, out bool stringChanged, out int charsCropped) => str.CropFrom(index + 1, out stringChanged, out charsCropped);

    public static string CropUntil(this string str, char character) => str.CropUntil(character, out _, out _);
    public static string CropUntil(this string str, string stringToMatch) => str.CropUntil(stringToMatch, out _, out _);
    public static string CropUntil(this string str, int index) => str.CropUntil(index, out _, out _);
    public static string CropUntil(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropUntil(str.IndexOf(character), out stringChanged, out charsCropped);
    public static string CropUntil(this string str, string stringToMatch, out bool stringChanged, out int charsCropped)
        => str.CropUntil(str.IndexOf(stringToMatch) is int index && str.IndexIsIn(index) ? index : -10, out stringChanged, out charsCropped);
    public static string CropUntil(this string str, int index, out bool stringChanged, out int charsCropped)
    {
        stringChanged = false;
        charsCropped = 0;

        if (str.IndexIsOut(index))
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
    public static string CropUntilWith(this string str, int index) => str.CropUntilWith(index, out _, out _);
    public static string CropUntilWith(this string str, char character, out bool stringChanged, out int charsCropped) => str.CropUntilWith(str.IndexOf(character), out stringChanged, out charsCropped);
    public static string CropUntilWith(this string str, string stringToMatch, out bool stringChanged, out int charsCropped)
        => str.CropUntilWith(str.IndexOf(stringToMatch) is int index && str.IndexIsIn(index) ? index + stringToMatch.Length - 1 : -10, out stringChanged, out charsCropped);
    public static string CropUntilWith(this string str, int index, out bool stringChanged, out int charsCropped) => str.CropUntil(index + 1, out stringChanged, out charsCropped);

    public static string RemoveOnce(this string str, string stringToRemove) => str.CropUntil(stringToRemove) + str.CropFrom(stringToRemove);
    public static string RemoveOnceFromEnd(this string str, string stringToRemove)
    {
        str = str.ReverseString();
        stringToRemove = stringToRemove.ReverseString();
        str = str.CropUntil(stringToRemove) + str.CropFrom(stringToRemove);
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
        void addBrokenPairCombinations(HashSet<string> combinations, int repeats)
        {
            var previousCombinations = combinations.Where(x => x.Count(combinationCharacter) == repeats).ToList();
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

        var combinations = new HashSet<string>();
        var start = fromCount is null ? 1 : fromCount.Value.AtLeast(1);
        var end = toCount is null ? baseString.Length + 1 : toCount.Value.AtMost(baseString.Length + 1);
        end = start == end ? end + 1 : end;
        var entered = false;

        for (var repeats = start; repeats < end; repeats++)
        {
            if (includeBrokenPairs && start != 0 && !entered)
            {
                entered = true;
            }

            if (includeBrokenPairs && combinations.Any())
            {
                addBrokenPairCombinations(combinations, repeats);
            }
            else
            {
                for (var j = 0; j < baseString.Length; j++)
                {
                    if (j + repeats > baseString.Length || baseString[j] == combinationCharacter)
                    {
                        continue;
                    }

                    var combination = baseString.CropUntil(j).Add(combinationCharacter.Repeat(repeats));
                    combination += baseString.CropFrom(j + repeats);

                    combinations.Add(combination);
                }

                if (includeBrokenPairs)
                {
                    addBrokenPairCombinations(combinations, repeats);
                }
            }
        }

        return combinations.ToList();
    }

    public static string ReplaceAt(this string str, char newCharacter, int index) => $"{str.CropUntil(index)}{newCharacter}{str.CropFrom(index)}";

    public static string Repeat(this char character, int times) => new(character, times);

    public static string TrimStartUpToNTimes(this string str, char character, int times) => str.TrimStartUpToNTimes(character, times, out _);
    public static string TrimStartUpToNTimes(this string str, char character, int times, out bool trimmed)
    {
        if (str.CountImmediate(character) is int count && count >= times)
        {
            trimmed = true;
            return $"{character.Repeat(count - times)}{str.TrimStart(character)}";
        }

        trimmed = false;
        return str;
    }

    public static string TrimStartExactlyNTimes(this string str, char character, int times) => str.TrimStartExactlyNTimes(character, times, out _);
    public static string TrimStartExactlyNTimes(this string str, char character, int times, out bool trimmed)
    {
        if (str.CountImmediate(character) is int count && count == times && times > 0)
        {
            trimmed = true;
            return str.TrimStart(character);
        }

        trimmed = false;
        return str;
    }

    public static string TrimEndUpToNTimes(this string str, char character, int times) => str.TrimEndUpToNTimes(character, times, out _);
    public static string TrimEndUpToNTimes(this string str, char character, int times, out bool trimmed)
    {
        if (str.ReverseString().CountImmediate(character) is int count && count >= times && times > 0)
        {
            trimmed = true;
            return $"{str.TrimEnd(character)}{character.Repeat(count - times)}";
        }

        trimmed = false;
        return str;
    }

    public static string TrimEndExactlyNTimes(this string str, char character, int times) => str.TrimEndExactlyNTimes(character, times, out _);
    public static string TrimEndExactlyNTimes(this string str, char character, int times, out bool trimmed)
    {
        if (str.ReverseString().CountImmediate(character) is int count && count == times && times > 0)
        {
            trimmed = true;
            return str.TrimEnd(character);
        }

        trimmed = false;
        return str;
    }

    private static bool IndexIsOut(this string str, int index) => index < 0 || index >= str.Length;
    private static bool IndexIsIn(this string str, int index) => !str.IndexIsOut(index);
}
