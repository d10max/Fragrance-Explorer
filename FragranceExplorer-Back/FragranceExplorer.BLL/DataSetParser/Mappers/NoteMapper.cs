using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Mappers;

public class NoteMapper : INoteMapper
{
    public PerfumeNote? Map(RawNoteDto? dto, NoteLayer layer)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return null;
        }

        var normalizedIntensity = Math.Clamp(
            dto.Weight / 100.0,
            ScentConstants.MinIntensity,
            ScentConstants.MaxIntensity);

        return new PerfumeNote(dto.Name.Trim(), normalizedIntensity, layer);
    }

    public IEnumerable<PerfumeNote> MapNotes(RawNotesDto? dto)
    {
        if (dto is null)
        {
            yield break;
        }

        if (dto.Tiered is not null)
        {
            foreach (var note in MapNoteList(dto.Tiered.Top, NoteLayer.Top))
            {
                yield return note;
            }

            foreach (var note in MapNoteList(dto.Tiered.Middle, NoteLayer.Middle))
            {
                yield return note;
            }

            foreach (var note in MapNoteList(dto.Tiered.Base, NoteLayer.Base))
            {
                yield return note;
            }
        }

        if (dto.Flat is not null)
        {
            foreach (var note in MapNoteList(dto.Flat, NoteLayer.Flat))
            {
                yield return note;
            }
        }
    }

    private IEnumerable<PerfumeNote> MapNoteList(IEnumerable<RawNoteDto>? noteDtos, NoteLayer layer)
    {
        if (noteDtos is null)
        {
            yield break;
        }

        foreach (var noteDto in noteDtos)
        {
            var note = Map(noteDto, layer);
            if (note is not null)
            {
                yield return note;
            }
        }
    }
}
