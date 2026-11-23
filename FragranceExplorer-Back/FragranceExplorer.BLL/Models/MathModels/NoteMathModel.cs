using FragranceExplorer.BLL.Constants;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.Models.MathModels;

public class NoteMathModel : ScentComponent
{
    public NoteLayer NoteLayer { get; private set; }

    public NoteMathModel(int id, double intensity, NoteLayer noteLayer)
        : base(id, intensity)
    {
        if (!Enum.IsDefined(noteLayer))
        {
            throw new ArgumentOutOfRangeException(
                nameof(noteLayer),
                ErrorMessagesConstants.InvalidNoteLayer);
        }

        NoteLayer = noteLayer;
    }
}
