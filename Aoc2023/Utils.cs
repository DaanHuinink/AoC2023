namespace Aoc2023;

public static class Utils
{
    private static readonly string[] LineSeparator = { "\r\n", "\r", "\n" };

    public static string[] ToLines(this string str)
    {
        return str.Split(LineSeparator, StringSplitOptions.None);
    }

    public static char[][] ToGrid(this string str)
    {
        return str.ToLines().Select(s => s.ToArray()).ToArray();
    }

    public static List<List<char>> ToMatrix(this string str)
    {
        return str.ToLines().Select(s => s.ToList()).ToList();
    }
}