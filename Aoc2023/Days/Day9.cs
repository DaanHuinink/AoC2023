using System.Collections.ObjectModel;

namespace Aoc2023.Days;

public sealed class Day9 : IDay
{
    public int PartOne(string input)
    {
        return Parse(input)
            .Select(CalculateNextValue)
            .Sum();
    }

    public int PartTwo(string input)
    {
        return Parse(input)
            .Select(CalculatePreviousValue)
            .Sum();
    }

    private static IEnumerable<List<int>> Parse(string input)
    {
        return input
            .ToLines()
            .Select(l => l.Split(" "))
            .Select(s => s.Select(int.Parse).ToList());
    }

    private static int CalculateNextValue(IList<int> values)
    {
        var tree = CalculateTree(values);

        // Reverse so we can iterate backwards
        tree.Reverse();
        return tree.Aggregate((current, next) =>
        {
            next.Add(current[^1] + next[^1]);
            return next;
        })[^1];
    }

    private static int CalculatePreviousValue(List<int> values)
    {
        var tree = CalculateTree(values);

        // Reverse iteration, no aggregate since we don't need another collection
        for (int index = tree.Count - 1; index >= 1; index--)
        {
            var current = tree[index];
            var next = tree[index - 1];
            next.Insert(0, next[0] - current[0]);
        }

        return tree[0][0];
    }

    private static List<IList<int>> CalculateTree(IList<int> values)
    {
        var tree = new List<IList<int>> { values };
        while (values.Any(i => i != 0))
        {
            var newValues = values
                .Zip(values.Skip(1), (current, next) => next - current)
                .ToList();
            tree.Add(newValues);
            values = newValues;
        }

        return tree;
    }
}