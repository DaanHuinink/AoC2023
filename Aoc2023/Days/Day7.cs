namespace Aoc2023.Days;

public class Day7 : IDay
{
    public int PartOne(string input)
    {
        var comparer = new HandComparer(SymbolOrderPart1);
        return input
            .ToLines()
            .Select(Hand.FromString)
            .OrderBy(h => h, comparer)
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();
    }

    public int PartTwo(string input)
    {
        var comparer = new HandComparer(SymbolOrderPart2);
        return input
            .ToLines()
            .Select(l => Hand.FromStringPart2(l, SymbolOrderPart2))
            .OrderBy(h => h, comparer)
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();
    }

    private static readonly IList<char> SymbolOrderPart2 = new[]
            { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' }
        .Reverse()
        .ToList();

    private static readonly IList<char> SymbolOrderPart1 = new[]
            { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2'}
        .Reverse()
        .ToList();

    private readonly record struct Hand(string Cards, int Bid, Hand.HandType Type)
    {
        public static Hand FromString(string input)
        {
            string[] tokens = input.Split(' ');
            return new Hand(tokens[0], int.Parse(tokens[1]), GetHandType(tokens[0]));
        }

        public static Hand FromStringPart2(string input, IList<char> symbolOrder)
        {
            string[] tokens = input.Split(' ');

            string cards = tokens[0];
            int bid = int.Parse(tokens[1]);
            HandType handType = GetHandTypeForPart2(symbolOrder, tokens);

            return new Hand(cards, bid, handType);
        }

        private static HandType GetHandTypeForPart2(IEnumerable<char> symbolOrder, string[] tokens)
        {
            if (tokens[0].Contains('J'))
            {
                return symbolOrder
                    .Select(c => tokens[0].Replace("J", new string(new[] { c })))
                    .Select(GetHandType)
                    .Max();
            }

            return GetHandType(tokens[0]);
        }

        private static HandType GetHandType(string cards)
        {
            var distinctCards = cards.Distinct().ToList();
            switch (distinctCards.Count)
            {
                case 1:
                    return HandType.FiveOfAKind;
                case 2:
                    int firstDistinctCardCount = cards.Count(c => c == distinctCards[0]);
                    return firstDistinctCardCount is 1 or 4 ? HandType.FourOfAKind : HandType.FullHouse;
                case 3:
                    return distinctCards.Any(d => cards.Count(c => c == d) == 3)
                        ? HandType.ThreeOfAKind
                        : HandType.TwoPair;
                case 4:
                    return HandType.OnePair;
                case 5:
                    return HandType.HighCard;

                default:
                    throw new InvalidOperationException();
            }
        }

        public enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }
    }

    private sealed class HandComparer(IList<char> symbolOrder) : IComparer<Hand>
    {
        public int Compare(Hand lhs, Hand rhs)
        {
            if (lhs.Type > rhs.Type)
            {
                return 1;
            }

            if (rhs.Type > lhs.Type)
            {
                return -1;
            }

            for (var i = 0; i < lhs.Cards.Length; ++i)
            {
                int valueLhs = ValueOfChar(lhs.Cards[i]);
                int valueRhs = ValueOfChar(rhs.Cards[i]);

                if (valueLhs > valueRhs)
                {
                    return 1;
                }

                if (valueLhs < valueRhs)
                {
                    return -1;
                }
            }

            return 0;
        }

        private int ValueOfChar(char c) => symbolOrder.IndexOf(c);
    }
}