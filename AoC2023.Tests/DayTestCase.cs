using Aoc2023;
using Aoc2023.Days;

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

internal static class TestCases
{
    public static DayTestCase<Day1> Day1 =>
        new(
            """
            1abc2
            pqr3stu8vwx
            a1b2c3d4e5f
            treb7uchet
            """,
            142,
            """
            two1nine
            eightwothree
            abcone2threexyz
            xtwone3four
            4nineeightseven2
            zoneight234
            7pqrstsixteen
            """,
            281);

    public static DayTestCase<Day2> Day2 =>
        new(
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """,
            8,
            """
            Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
            Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
            Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
            Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
            Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
            """,
            2286);

    public static DayTestCase<Day3> Day3 =>
        new(
            """
            467..114..
            ...*......
            ..35..633.
            ......#...
            617*......
            .....+.58.
            ..592.....
            ......755.
            ...$.*....
            .664.598..
            """,
            4361,
            """
            467..114..
            ...*......
            ..35..633.
            ......#...
            617*......
            .....+.58.
            ..592.....
            ......755.
            ...$.*....
            .664.598..
            """,
            467835);

    public static DayTestCase<Day4> Day4 =>
        new(
            """
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """,
            13,
            """
            Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
            Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
            Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
            Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
            Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
            Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
            """,
            30);

    public static DayTestCase<Day5> Day5 =>
        new(
            """
            seeds: 79 14 55 13

            seed-to-soil map:
            50 98 2
            52 50 48

            soil-to-fertilizer map:
            0 15 37
            37 52 2
            39 0 15

            fertilizer-to-water map:
            49 53 8
            0 11 42
            42 0 7
            57 7 4

            water-to-light map:
            88 18 7
            18 25 70

            light-to-temperature map:
            45 77 23
            81 45 19
            68 64 13

            temperature-to-humidity map:
            0 69 1
            1 0 69

            humidity-to-location map:
            60 56 37
            56 93 4
            """,
            35,
            """
            seeds: 79 14 55 13

            seed-to-soil map:
            50 98 2
            52 50 48

            soil-to-fertilizer map:
            0 15 37
            37 52 2
            39 0 15

            fertilizer-to-water map:
            49 53 8
            0 11 42
            42 0 7
            57 7 4

            water-to-light map:
            88 18 7
            18 25 70

            light-to-temperature map:
            45 77 23
            81 45 19
            68 64 13

            temperature-to-humidity map:
            0 69 1
            1 0 69

            humidity-to-location map:
            60 56 37
            56 93 4
            """,
            46);

    public static DayTestCase<Day6> Day6 =>
        new(
            """
            Time:      7  15   30
            Distance:  9  40  200
            """,
            288,
            """
            Time:      7  15   30
            Distance:  9  40  200
            """,
            71503);

    public static DayTestCase<Day7> Day7 => new(
        """
        32T3K 765
        T55J5 684
        KK677 28
        KTJJT 220
        QQQJA 483
        """
        ,
        6440,
        """
        32T3K 765
        T55J5 684
        KK677 28
        KTJJT 220
        QQQJA 483
        """,
        5905);

    public static DayTestCase<Day8> Day8 => new(
        """
        LLR

        AAA = (BBB, BBB)
        BBB = (AAA, ZZZ)
        ZZZ = (ZZZ, ZZZ)
        """,
        6,
        """
        LR

        11A = (11B, XXX)
        11B = (XXX, 11Z)
        11Z = (11B, XXX)
        22A = (22B, XXX)
        22B = (22C, 22C)
        22C = (22Z, 22Z)
        22Z = (22B, 22B)
        XXX = (XXX, XXX)
        """,
        6);

    public static DayTestCase<Day9> Day9 => new(
        """
        0 3 6 9 12 15
        1 3 6 10 15 21
        10 13 16 21 30 45
        """,
        114,
        """
        0 3 6 9 12 15
        1 3 6 10 15 21
        10 13 16 21 30 45
        """,
        2);

    public static DayTestCase<Day10> Day10 => new(
        """
        ..F7.
        .FJ|.
        SJ.L7
        |F--J
        LJ...
        """,
        8,
        """
        .F----7F7F7F7F-7....
        .|F--7||||||||FJ....
        .||.FJ||||||||L7....
        FJL7L7LJLJ||LJ.L-7..
        L--J.L7...LJS7F-7L7.
        ....F-J..F7FJ|L7L7L7
        ....L7.F7||L7|.L7L7|
        .....|FJLJ|FJ|F7|.LJ
        ....FJL-7.||.||||...
        ....L---J.LJ.LJLJ...
        """,
        8);

    public static DayTestCase<Day11> Day11 => new(
        """
        ...#......
        .......#..
        #.........
        ..........
        ......#...
        .#........
        .........#
        ..........
        .......#..
        #...#.....
        """,
        8410,
        "",
        0);
}