namespace Aoc2023.Days;

public class Day6 : IDay
{
    public int PartOne(string input)
    {
        var lineTokens = input
            .ToLines()
            .Select(l => l[11..])
            .Select(l => l.Split(' ').Where(t => !string.IsNullOrWhiteSpace(t)))
            .ToList();

        return (int)lineTokens[0]
            .Select(long.Parse)
            .Zip(lineTokens[1].Select(long.Parse))
            .Select(c => new Race(c.First, c.Second))
            .Select(r => r.CalculateMargin())
            .Aggregate((r1, r2) => r1 * r2);
    }

    public int PartTwo(string input)
    {
        var longs = input
            .ToLines()
            .Select(l => l[11..])
            .Select(l => new string(l.Where(char.IsDigit).ToArray()))
            .Select(long.Parse)
            .ToList();
        return (int)new Race(longs[0], longs[1]).CalculateMargin();
    }

    private readonly record struct Race(long Time, long Distance)
    {
        public long CalculateMargin()
        {
            long time = Time;
            long distance = Distance;
            return Enumerable.Range(0, (int)Time)
                .Count(speed => (time - speed) * speed > distance);
        }
    }
}