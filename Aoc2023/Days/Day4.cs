using System.Diagnostics;

namespace Aoc2023.Days;

public sealed class Day4 : IDay
{
    public int PartOne(string input)
    {
        return input
            .ToLines()
            .Select(Card.FromLine)
            .Select(c => c.CalculatePoints())
            .Sum();
    }

    public int PartTwo(string input)
    {
        var cards = input
            .ToLines()
            .Select(Card.FromLine)
            .ToList();

        return ProcessCards(cards);
    }

    private static int ProcessCards(IReadOnlyList<Card> cards)
    {
        for (var i = 0; i < cards.Count; ++i)
        {
            Card card = cards[i];
            for (int j = i; j < i + card.AmountOfMatchingNumbers; ++j)
            {
                cards[j + 1].Amount += card.Amount;
            }
        }

        return cards.Sum(c => c.Amount);
    }

    private record Card(
        IReadOnlyCollection<int> WinningNumbers,
        IReadOnlyCollection<int> NumbersYouHave,
        int AmountOfMatchingNumbers)
    {
        public int Amount = 1;

        public int CalculatePoints()
        {
            return (int)Math.Pow(2, NumbersYouHave.Count(n => WinningNumbers.Contains(n)) - 1);
        }

        public static Card FromLine(string line)
        {
            string[] parts = line.Split('|');

            string winningNumberString = parts[0][(parts[0].IndexOf(':') + 1)..];
            string numbersYouHaveString = parts[1];

            var winningNumbers = winningNumberString
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToList();

            var numbersYouHave = numbersYouHaveString
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(int.Parse)
                .ToList();

            int amountOfMatchingNumbers = CalculateAmountOfMatchingNumbers(numbersYouHave, winningNumbers);
            return new Card(winningNumbers, numbersYouHave, amountOfMatchingNumbers);
        }

        private static int CalculateAmountOfMatchingNumbers(
            IEnumerable<int> numbersYouHave,
            IEnumerable<int> winningNumbers)
        {
            return numbersYouHave.Count(winningNumbers.Contains);
        }
    }
}