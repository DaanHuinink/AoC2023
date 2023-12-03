namespace Aoc2023.Days;

public sealed class Day1 : IDay
{
    private static readonly IReadOnlyDictionary<string, char> NumberDictionary = GetNumberDictionary();
    
    public int PartOne(string input)
    {
        return input
            .Split("\n")
            .Select(line => line.Where(char.IsDigit).ToArray())
            .Select(line => int.Parse(new string(new[] { line[0], line[^1] })))
            .Sum();
    }

    public int PartTwo(string input)
    {
        string[] lines = input.Split("\n");
        return lines.Sum(GetNumberToAdd);
    }

    private static int GetNumberToAdd(string line)
    {
        var foundNumberStrings = new List<(int index, char value)>();
        foreach ((string numberStr, char numberValue) in NumberDictionary)
        {
            foundNumberStrings.AddRange(AllIndexesOf(line, numberStr).Select(index => (index, numberValue)));
        }

        var ordered = foundNumberStrings.OrderBy(f => f.index).ToList();
        int result =  ordered[^1].value - '0' + (ordered[0].value - '0') * 10;
        return result;
    }

    private static Dictionary<string, char> GetNumberDictionary()
    {
        var numberStringDictionary = new Dictionary<string, char>
        {
            { "one", '1' },
            { "two", '2' },
            { "three", '3' },
            { "four", '4' },
            { "five", '5' },
            { "six", '6' },
            { "seven", '7' },
            { "eight", '8' },
            { "nine", '9' }
        };

        var numberCharDictionary = Enumerable
            .Range('1', 9)
            .ToDictionary(
                n => new string(new[] { (char)n }),
                n => (char)n);

        var numberDictionary = numberStringDictionary
            .Concat(numberCharDictionary)
            .ToDictionary();
        return numberDictionary;
    }
    
    private static IEnumerable<int> AllIndexesOf(string str, string searchString)
    {
        int minIndex = str.IndexOf(searchString, StringComparison.Ordinal);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchString, minIndex + searchString.Length, StringComparison.Ordinal);
        }
    }
}