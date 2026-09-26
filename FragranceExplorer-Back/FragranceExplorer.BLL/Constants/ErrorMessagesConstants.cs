using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.Constants;

public static class ErrorMessagesConstants
{
    public static string ScentComponentIntensityIsOutOfRange()
    {
        return $"Scent component intensity should be between {ScentConstants.MinIntensity} and {ScentConstants.MaxIntensity}.";
    }

    public static string PerfumeRatingIsOutOfRange()
    {
        return $"Perfume rating should be between {ScentConstants.MinPerfumeRating} and {ScentConstants.MaxPerfumeRating}.";
    }

    public const string InvalidNoteLayer = "Invalid perfume note layer. Allowed values are Top, Middle, Base, Flat.";

    public const string InvalidGenderCategory = "Invalid gender category. Allowed values are Unisex, Male, Female.";
}
