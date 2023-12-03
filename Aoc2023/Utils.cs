namespace Aoc2023;

public static class Utils
{
    private static readonly string[] Separator = { "\r\n", "\r", "\n" };

    public static string[] ToLines(this string str)
    {
        return str.Split(Separator, StringSplitOptions.None);
    }
}