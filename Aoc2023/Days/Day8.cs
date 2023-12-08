namespace Aoc2023.Days;

public class Day8 : IDay
{
    public int PartOne(string input)
    {
        string[] lines = input.ToLines();
        string instructions = lines[0];

        return(int) AmountOfStepsRequiredToReachCondition(
            ParseTree(lines).Single(n => n.Name == "AAA"),
            instructions,
            n => n.Name == "ZZZ");
    }

    public int PartTwo(string input)
    {
        string[] lines = input.ToLines();
        string instructions = lines[0];

        long result = ParseTree(lines)
            .Where(n => n.Name.EndsWith('A'))
            .Select(n => AmountOfStepsRequiredToReachCondition(n, instructions, node => node.Name.EndsWith('Z')))
            .Aggregate(1L, CalculateLcm);

        Console.WriteLine($"RESULT, TAKE THIS ONE, THE INT CONVERSION IS INCORRECT: {result}");
        return (int)result;
    }

    private static long AmountOfStepsRequiredToReachCondition(
        Node node,
        string instructions,
        Func<Node, bool> condition)
    {
        var instructionIndex = 0;
        var amountOfSteps = 0;
        while (!condition(node))
        {
            node = instructions[instructionIndex++] == 'L' ? node.Left : node.Right;
            instructionIndex %= instructions.Length;
            ++amountOfSteps;
        }

        return amountOfSteps;
    }

    private static IEnumerable<Node> ParseTree(string[] lines)
    {
        var lineStrings = lines[2..]
            .Select(line => new
            {
                Left = line[7..10],
                Right = line[12..15],
                Node = new Node(line[0..3])
            })
            .ToList();

        foreach (var lineString in lineStrings)
        {
            lineString.Node.Left = lineStrings.Single(l => l.Node.Name == lineString.Left).Node;
            lineString.Node.Right = lineStrings.Single(l => l.Node.Name == lineString.Right).Node;
        }

        return lineStrings.Select(l => l.Node);
    }

    private sealed record Node(string Name)
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
    }

    private static long CalculateLcm(long a, long b)
    {
        return a / CalculateGcf(a, b) * b;
    }

    private static long CalculateGcf(long a, long b)
    {
        while (b != 0)
        {
            long c = b;
            b = a % b;
            a = c;
        }

        return a;
    }
}