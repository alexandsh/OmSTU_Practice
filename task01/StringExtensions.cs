namespace task01;

using System;
using System.Linq;

public static class StringExtensions
{
    public static string Reverse(this string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        
        return new string(charArray);
    }

    public static string Clean(this string str)
    {
        return new string(str.Where(c => !char.IsWhiteSpace(c) && !char.IsPunctuation(c)).ToArray());
    }

    public static bool IsPalindrome(this string str)
    {
        return str == "" ? false : string.Equals(str.Clean(), str.Clean().Reverse(), StringComparison.OrdinalIgnoreCase);
    }
}
