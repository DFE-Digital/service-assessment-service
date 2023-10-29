using System.Text.RegularExpressions;

namespace ServiceAssessmentService.WebApp.Test.TestHelpers;

internal static class TextHelper
{

    public static string NormaliseWhitespace(string str)
    {
        // Normalise all whitespace to spaces (e.g., tabs, newlines).
        str = Regex.Replace(str, @"\s", " ");

        // Condense sequences of two or more whitespace characters into a single space.
        str = Regex.Replace(str, @"\s{2,}", " ");

        // Trim any remaining leading/trailing whitespace
        str = str.Trim();

        return str;
    }

}
