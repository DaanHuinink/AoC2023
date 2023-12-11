using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace AoC2023.Tests;

[TestFixtureSource(nameof(TestSources))]
internal sealed class DayTestFixture(DayTestCase dayTestCase)
{
    private static readonly object[] TestSources =
    {
        TestCases.Day1,
        TestCases.Day2,
        TestCases.Day3,
        TestCases.Day4,
        TestCases.Day5,
        TestCases.Day6,
        TestCases.Day7,
        TestCases.Day8,
        TestCases.Day9,
        TestCases.Day10,
        TestCases.Day11
    };

    [Test]
    public void PartOne()
    {
        int result = dayTestCase.Day.PartOne(dayTestCase.Input1);
        Assert.That(result, Is.EqualTo(dayTestCase.ExpectedOutput1));
    }

    [Test]
    public void PartTwo()
    {
        int result = dayTestCase.Day.PartTwo(dayTestCase.Input2);
        Assert.That(result, Is.EqualTo(dayTestCase.ExpectedOutput2));
    }
}
