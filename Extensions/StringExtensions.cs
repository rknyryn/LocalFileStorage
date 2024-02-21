using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LocalFileStorage.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// This method is used to remove all accents from a string.
    /// </summary>
    /// <param name="text">The string to remove the accents from.</param>
    /// <returns>The string without any accents.</returns>
    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        { return text; }

        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
            != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    ///  This method is used to convert a string into a slug.
    /// </summary>
    /// <param name="phrase">The string to be converted into a slug.</param>
    /// <returns>The slug of the string.</returns>
    public static string Slugify(this string phrase)
    {
        phrase += string.Concat("-", Guid.NewGuid().ToString().Split('-').FirstOrDefault().AsSpan(0, 3));

        // Remove all accents and make the string lower case.
        string output = phrase.ToLower();
        output = output
            .Replace('ı', 'i')
            .Replace('ö', 'o')
            .Replace('ü', 'u')
            .Replace('ş', 's')
            .Replace('ç', 'c')
            .Replace('ğ', 'g');


        output = output.RemoveAccents();
        // Remove all special characters from the string.
        output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

        // Remove all additional spaces in favour of just one.
        output = Regex.Replace(output, @"\s+", " ").Trim();

        // Replace all spaces with the hyphen.
        output = Regex.Replace(output, @"\s", "-");

        // Return the slug.
        return output;
    }
}
