using FragranceExplorer.BLL.Enums;
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

    public static string PerfumeSimilarityScoreIsOutOfRange()
    {
        return $"Perfume rating should be between {ScentConstants.MinPerfumeSimilarityScore} and {ScentConstants.MaxPerfumeSimilarityScore}.";
    }

    public static string ErrorFlatAddedToPyramid(string noteName)
    {
        return $"Cannot add Flat note '{noteName}' because the perfume already has pyramid notes.";
    }

    public static string ErrorPyramidAddedToFlat(string noteName)
    {
        return $"Cannot add note '{noteName}' because the perfume is marked as Flat.";
    }

    public const string InvalidSimilarityWeights = "Invalid similarity weights. Sum of weights should be equal to 1.0.";

    public const string InvalidNoteLayer = "Invalid perfume note layer. Allowed values are Top, Middle, Base, Flat.";

    public const string InvalidGenderCategory = "Invalid gender category. Allowed values are Unisex, Male, Female.";

    public const string InvalidSearchProfile = "Invalid search profile. Allowed values are Default, ExactClone, SimilarVibe.";

    public const string PerfumeNotFoundMessage = "Perfume was not found.";
}
