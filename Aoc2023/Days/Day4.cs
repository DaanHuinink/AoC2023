using System.Diagnostics;

namespace Aoc2023.Days;

public sealed class Day4 : IDay
{
    public int PartOne(string input)
    {
        return input
            .ToLines()
            .Select(Card.FromLine)
            .Select(c => c.Points)
            .Sum();
    }

    public int PartTwo(string input)
    {
        var cards = input
            .ToLines()
            .Select(Card.FromLine)
            .ToList();

        return ProcessCards(cards, cards.ToDictionary(card => card.Id, card => card));
    }

    // In terms of performance this sucks mega-ass and can easily be improved
    // Simply do a loop over the cards with a nested loop for every card to count the amount of cards we get
    private static int ProcessCards(
        IReadOnlyList<Card> cards,
        IReadOnlyDictionary<int, Card> cardDictionary)
    {
        var newCards = new List<Card>();
        foreach (var card in cards)
        {
            if (card.AmountOfMatchingNumbers  == 0)
            {
                continue;
            }

            for (int id = card.Id + 1;
                 id < card.Id + 1 + card.AmountOfMatchingNumbers;
                 ++id)
            {
                newCards.Add(cardDictionary[id]);
            }
        }

        if (newCards.Count > 0)
        {
            return cards.Count + ProcessCards(newCards, cardDictionary);
        }

        return cards.Count;
    }
    private readonly record struct Card(
        int Id,
        IReadOnlyCollection<int> WinningNumbers,
        IReadOnlyCollection<int> NumbersYouHave)
    {
        public static Card FromLine(string line)
        {
            string[] parts = line.Split('|');

            int cardId = int.Parse(parts[0][5..parts[0].IndexOf(':')]);

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

            return new Card(cardId, winningNumbers, numbersYouHave);
        }

        public int Points
        {
            get
            {
                var winningNumbers = WinningNumbers;
                return (int)Math.Pow(2, NumbersYouHave.Count(n => winningNumbers.Contains(n)) - 1);
            }
        }

        public int AmountOfMatchingNumbers
        {
            get
            {
                var winningNumbers = WinningNumbers;
                return NumbersYouHave.Count(n => winningNumbers.Contains(n));
            }
        }
    }
}