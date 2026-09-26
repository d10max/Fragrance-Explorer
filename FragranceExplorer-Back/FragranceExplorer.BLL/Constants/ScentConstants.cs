using FragranceExplorer.BLL.Enums;
using System.Diagnostics;

namespace FragranceExplorer.BLL.Constants;

public static class ScentConstants
{
    public const double MinIntensity = 0.0;
    public const double MaxIntensity = 1.0;

    public const double MinPerfumeRating = 0.0;
    public const double MaxPerfumeRating = 5.0;

    public const double BaseNoteLayerWeight = 0.5;
    public const double TopNoteLayerWeight = 0.2;
    public const double MiddleNoteLayerWeight = 0.3;
    public const double FlatNoteLayerWeight = 0.35;

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
                                ErrorMessagesConstants.InvalidNoteLayer),
        };
    }
}
