using System.Globalization;
using System.Text;

namespace FragranceExplorer.BLL.Helpers;

public static class TextNormalizer
{
    public static string Normalize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        var normalized = text.Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder(); 

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;

            if (char.IsLetterOrDigit(c))
            {
                builder.Append(char.ToLowerInvariant(c));
            }
            else
            {
                if (builder.Length > 0 && builder[^1] != ' ')
                {
                    builder.Append(' ');     
                }
            }
        }  

        return builder.ToString().TrimEnd();
    }
}   