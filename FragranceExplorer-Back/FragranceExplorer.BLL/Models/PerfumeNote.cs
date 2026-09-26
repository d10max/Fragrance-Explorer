using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;

namespace FragranceExplorer.BLL.Models;

public class PerfumeNote : ScentComponent
{
    public NoteLayer NoteLayer { get; private set; }

    public PerfumeNote(string name, double intensity, NoteLayer noteLayer)
        : base(name, intensity)
    {
        if(!Enum.IsDefined(noteLayer))
        {
            throw new ArgumentOutOfRangeException(
                nameof(noteLayer),
                ErrorMessagesConstants.InvalidNoteLayer);
        }

        NoteLayer = noteLayer;
    }

    public override double CalculateSignificance()
    {
        var layerWeight = ScentConstants.GetNoteLayerWeight(NoteLayer);
        return Intensity * layerWeight;
    }
}
