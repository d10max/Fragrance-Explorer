using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.DAL.Entities;

public class PerfumeNoteEntity
{
    public int NoteId { get; set; }

    public int PerfumeId { get; set; }

    public double Weight { get; set; }

    public NoteLayer Layer { get; set; }

    public PerfumeEntity Perfume { get; set; } = null!;

    public NoteEntity Note { get; set; } = null!;
}
