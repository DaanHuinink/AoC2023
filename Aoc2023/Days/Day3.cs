namespace Aoc2023.Days;

public class Day3 : IDay
{
    public int PartOne(string input)
    {
        var board = CreateBoard(input);
        return board.SumOfAdjacentNumbers;
    }

    private static Board CreateBoard(string input)
    {
        List<Part> parts = new();
        List<Symbol> symbols = new();

        string[] lines = input.ToLines();

        int width = lines[0].Length;
        int height = lines.Length;

        for (var y = 0; y < height; ++y)
        {
            for (var x = 0; x < width; ++x)
            {
                if (char.IsDigit(lines[y][x]))
                {
                    // Look ahead for the number
                    List<Position> positions = new();
                    while (x < width && char.IsDigit(lines[y][x]))
                    {
                        positions.Add(new Position(x, y));
                        ++x;
                    }

                    string numberStr = lines[y].Substring(x - positions.Count, positions.Count);
                    int numberValue = int.Parse(numberStr);
                    parts.Add(new Part(numberValue, positions));

                    // Walk ahead, but not too far
                    --x;
                }
                else if (lines[y][x] != '.')
                {
                    symbols.Add(new Symbol(new Position(x, y), lines[y][x]));
                }
            }
        }

        return new Board(parts, symbols);
    }

    public int PartTwo(string input)
    {
        var board = CreateBoard(input);
        return board.SumOfGearRatios;
    }

    private readonly record struct Position(int X, int Y)
    {
        public bool IsAdjacentTo(Position other) => IsAdjacentToOther(X, other.X) && IsAdjacentToOther(Y, other.Y);

        private static bool IsAdjacentToOther(int number, int otherNumber) =>
            number == otherNumber ||
            number == otherNumber - 1 ||
            number == otherNumber + 1;
    }

    private readonly record struct Part(int Value, IReadOnlyCollection<Position> Positions)
    {
        public bool IsAdjacentTo(Symbol symbol)
        {
            return Positions.Any(position => position.IsAdjacentTo(symbol.Position));
        }
    }

    private record struct Symbol(Position Position, char Char);

    private readonly record struct Board(
        IReadOnlyCollection<Part> Parts,
        IReadOnlyCollection<Symbol> Symbols)
    {
        public int SumOfAdjacentNumbers
        {
            get
            {
                var symbols = Symbols;
                return Parts
                    .Where(number => symbols.Any(number.IsAdjacentTo))
                    .Select(n => n.Value)
                    .Sum();
            }
        }

        public int SumOfGearRatios
        {
            get
            {
                var parts = Parts;
                return Symbols
                    .Where(s => s.Char == '*')
                    .Select(symbol => parts.Where(part => part.IsAdjacentTo(symbol)).ToList())
                    .Where(gearParts => gearParts.Count == 2)
                    .Select(gearParts => gearParts[0].Value * gearParts[1].Value)
                    .Sum();
            }
        }
    }
}