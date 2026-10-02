using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Interfaces;

public interface INoteMapper
{
    PerfumeNote? Map(RawNoteDto? dto, NoteLayer layer);

    IEnumerable<PerfumeNote> MapNotes(RawNotesDto? dto);
}
