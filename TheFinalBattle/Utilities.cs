using System.Globalization;
using System.Numerics;

namespace TheFinalBattle;

public static class Utilities
{
    public static bool ReadYesNo(string question)
    {
        Console.Write(question);
        Console.Write(" ");
        var response = Console.ReadLine();
        return response?.ToLower() switch
        {
            "yes" => true,
            "no" => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string? ReadAnswer(string question)
    {
        Console.Write(question);
        Console.Write(" ");
        return Console.ReadLine();
    }

    public static T ReadAnswerOf<T>(string question) where T : INumberBase<T>
    {
        Console.Write(question);
        Console.Write(" ");
        return T.Parse(Console.ReadLine()!, NumberStyles.Any, null);
    }
}