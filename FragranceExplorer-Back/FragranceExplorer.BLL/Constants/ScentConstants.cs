using FragranceExplorer.BLL.Enums;
using System.Diagnostics;

namespace FragranceExplorer.BLL.Constants;

public static class ScentConstants
{
    public const double MinIntensity = 0.0;
    public const double MaxIntensity = 1.0;

    public const double MinPerfumeRating = 0.0;
    public const double MaxPerfumeRating = 5.0;

    public const double MinPerfumeSimilarityScore = 0.0;
    public const double MaxPerfumeSimilarityScore = 1.0;

    public const double BaseNoteLayerWeight = 0.5;
    public const double TopNoteLayerWeight = 0.15;
    public const double MiddleNoteLayerWeight = 0.35;
    public const double FlatNoteLayerWeight = 0.35;

    public const double Epsilon = 1e-5;

    public const int TestMaxPerfumesAmountToParse = 1000;

    public readonly static (double, double) DefaultWeights = (0.65, 0.35);
    public readonly static (double, double) ExactCloneWeights = (0.40, 0.60);
    public readonly static (double, double) SimilarVibeWeights = (0.85, 0.15);

    public static double GetNoteLayerWeight(NoteLayer noteLayer)
    {
        return noteLayer switch
        {
            NoteLayer.Base => BaseNoteLayerWeight,
            NoteLayer.Top => TopNoteLayerWeight,
            NoteLayer.Middle => MiddleNoteLayerWeight,
            NoteLayer.Flat => FlatNoteLayerWeight,
            _ => throw new ArgumentOutOfRangeException(
                                nameof(noteLayer),
                                ErrorMessagesConstants.InvalidNoteLayer)
        };
    }

    public static (double accordWeight, double noteWeight) GetSimilarityWeights(SearchProfile profile)
    {
        return profile switch
        { 
            SearchProfile.Default => DefaultWeights,
            SearchProfile.ExactClone => ExactCloneWeights,
            SearchProfile.SimilarVibe => SimilarVibeWeights,
            _ => throw new ArgumentOutOfRangeException(
                                nameof(profile),
                                ErrorMessagesConstants.InvalidSearchProfile)
        };
    }
}
