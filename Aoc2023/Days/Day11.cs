namespace Aoc2023.Days;

public class Day11 : IDay
{
    private const char GalaxyChar = '#';

    public int PartOne(string input)
    {
        return (int)CalculateTotalDistance(input, 100);
    }

    public int PartTwo(string input)
    {
        long result = CalculateTotalDistance(input, 1_000_000);
        Console.WriteLine($"RESULT WHICH DOESNT FIT IN INT: {result}");
        return (int)result;
    }

    private long CalculateTotalDistance(string input, long amountToExpand)
    {
        var universe = input.ToMatrix();
        var galaxies = GetGalaxies(universe);
        ExpandUniverse(universe, galaxies, amountToExpand);
        long total = CalculateTotalDistanceBetweenGalaxies(galaxies);
        return total;
    }

    private static long CalculateTotalDistanceBetweenGalaxies(IReadOnlyList<Galaxy> galaxies)
    {
        long total = 0;
        for (var i = 0; i < galaxies.Count; i++)
        {
            Galaxy g1 = galaxies[i];
            for (int j = i + 1; j < galaxies.Count; ++j)
            {
                Galaxy g2 = galaxies[j];
                total += Math.Abs(g1.X - g2.X) + Math.Abs(g1.Y - g2.Y);
            }
        }

        return total;
    }

    private static IReadOnlyList<Galaxy> GetGalaxies(IReadOnlyList<IReadOnlyList<char>> universe)
    {
        var galaxies = new List<Galaxy>();

        for (var y = 0; y < universe.Count; ++y)
        {
            for (var x = 0; x < universe[y].Count; ++x)
            {
                if (universe[y][x] == GalaxyChar)
                {
                    galaxies.Add(new Galaxy(x, y));
                }
            }
        }

        return galaxies;
    }

    private sealed record Galaxy(long X, long Y)
    {
        public long X { get; set; } = X;
        public long Y { get; set; } = Y;
    }

    private static void ExpandUniverse(
        IReadOnlyList<List<char>> universe,
        IEnumerable<Galaxy> galaxies,
        long amountToExpand)
    {
        amountToExpand -= 1;
        var rowsWithoutGalaxy = GetRowsWithoutGalaxy(universe);
        var colsWithoutGalaxy = GetRowsWithoutGalaxy(Transpose(universe));

        foreach (Galaxy galaxy in galaxies)
        {
            galaxy.Y += rowsWithoutGalaxy.Count(row => galaxy.Y > row) * amountToExpand;
            galaxy.X += colsWithoutGalaxy.Count(col => galaxy.X > col) * amountToExpand;
        }
    }

    private static List<int> GetRowsWithoutGalaxy(IEnumerable<IReadOnlyList<char>> universe)
    {
        return universe
            .Select((row, index) => (row, index))
            .Where(r => !r.row.Contains(GalaxyChar))
            .Select(r => r.index)
            .ToList();
    }

    private static List<List<T>> Transpose<T>(IReadOnlyList<IReadOnlyList<T>> matrix)
    {
        int rows = matrix.Count;
        int columns = matrix[0].Count;

        var result = new List<List<T>>();

        for (var c = 0; c < columns; c++)
        {
            result.Add([]);
            for (var r = 0; r < rows; r++)
            {
                result[c].Add(matrix[r][c]);
            }
        }

        return result;
    }
}