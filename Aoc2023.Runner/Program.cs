using Aoc2023;
using Aoc2023.Days;

Run<Day1>();
Run<Day2>();
Run<Day3>();
Run<Day4>();
Run<Day5>();
Run<Day6>();
Run<Day7>();
Run<Day8>();
Run<Day9>();
Run<Day10>();
Run<Day11>();
return;

void Run<TDay>() where TDay : IDay, new()
{
    var day = new TDay();
    string dayName = day.GetType().Name;
    Console.WriteLine($"> Running day {dayName}!");

    try
    {
        var file = $"Inputs/{dayName}.txt";
        Console.WriteLine($"  > Reading input {file}");
        
        string input = File.ReadAllText(file);
    
        int output1 = day.PartOne(input);
        Console.WriteLine($"  > {dayName} {nameof(IDay.PartOne)}: {output1}");
    
        int output2 = day.PartTwo(input);
        Console.WriteLine($"  > {dayName} {nameof(IDay.PartTwo)}: {output2}");
    }
    catch (Exception exception)
    {
        Console.Error.WriteLine($"Error running {dayName}: {exception}");
    }
}