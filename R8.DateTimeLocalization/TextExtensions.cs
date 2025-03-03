using System;

namespace R8.DateTimeLocalization;

internal static class TextExtensions
{
    public static string RemoveRlmChar(this string str)
    {
        Span<char> c = stackalloc char[str.Length];
        var lastIndex = -1;
        for (var index = 0; index < str.Length; index++)
        {
            var ch = str[index];
            if (ch == '\u200F')
                continue;

            c[++lastIndex] = ch;
        }

        c = c[..(lastIndex + 1)];
        return new string(c);
    }
}