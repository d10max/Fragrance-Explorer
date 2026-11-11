using FragranceExplorer.BLL.Parser.Perfumes.Models;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.Parser.Perfumes.Interfaces;

public interface INoteMapper
{
    PerfumeNote? Map(RawNoteDto? dto, NoteLayer layer);

    IEnumerable<PerfumeNote> MapNotes(RawNotesDto? dto);
}
