using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface INoteMapper
{
    PerfumeNote? Map(RawNoteDto? dto, NoteLayer layer);

    IEnumerable<PerfumeNote> MapNotes(RawNotesDto? dto);
}
