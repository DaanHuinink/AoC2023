using Aoc2023;

namespace AoC2023.Tests;

internal sealed class DayTestCase<TDay>(
        string input1,
        int expectedOutput1,
        string input2,
        int expectedOutput2)
    : DayTestCase(
        new TDay(),
        input1,
        expectedOutput1,
        input2,
        expectedOutput2) where TDay : IDay, new();

internal abstract class DayTestCase(
    IDay day,
    string input1,
    int expectedOutput1,
    string input2,
    int expectedOutput2)
{
    public IDay Day { get; } = day;

    public string Input1 { get; } = input1;
    public string Input2 { get; } = input2;

    public int ExpectedOutput1 { get; } = expectedOutput1;
    public int ExpectedOutput2 { get; } = expectedOutput2;
}