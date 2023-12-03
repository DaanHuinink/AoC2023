namespace Aoc2023.Days;

public sealed class Day2 : IDay
{
    public int PartOne(string input)
    {
        return input
            .ToLines()
            .Select(CreateMaxRequiredCubesGame)
            .Where(g => g.IsPossible())
            .Select(g => g.Id)
            .Sum();
    }

    public int PartTwo(string input)
    {
        return input
            .ToLines()
            .Select(CreateMaxRequiredCubesGame)
            .Select(g => g.Powaaaaaaah)
            .Sum();
    }

    private static Game CreateMaxRequiredCubesGame(string input)
    {
        int gameId = int.Parse(input.Split(" ")[1].Replace(":", ""));
        string stringWithoutGameHeader = input[(input.IndexOf(':') + 1)..];
        string[] roundStrings = stringWithoutGameHeader.Split(';');

        var maxRed = 0;
        var maxBlue = 0;
        var maxGreen = 0;

        foreach (string roundStr in roundStrings)
        {
            string[] playStrs = roundStr.Split(',');
            foreach (string playStr in playStrs)
            {
                string[] des = playStr.Split(' ');
                switch (des[2])
                {
                    case "red":
                        maxRed = Max(des[1], maxRed);
                        break;
                    case "blue" : 
                        maxBlue = Max(des[1], maxBlue);
                        break;
                    case "green":
                        maxGreen = Max(des[1], maxGreen);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(input), $"Unknown str {des[1]}");
                }
            }
        }
        return new Game(gameId, maxRed, maxGreen, maxBlue);
    }

    private static int Max(string str, int red)
    {
        return Math.Max(int.Parse(str), red);
    }

    private readonly record struct Game(int Id, int Red, int Green, int Blue)
    {
        public bool IsPossible()
        {
            const int ALLOWED_RED = 12;
            const int ALLOWED_GREEN = 13;
            const int ALLOWED_BLUE = 14;

            return Red <= ALLOWED_RED &&
                   Green <= ALLOWED_GREEN &&
                   Blue <= ALLOWED_BLUE;
        }

        public int Powaaaaaaah => Red * Green * Blue;
    }
}