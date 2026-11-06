namespace FragranceExplorer.DAL.Entities;

public class NoteEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<PerfumeNoteEntity> PerfumeNotes { get; set; } = [];
}
