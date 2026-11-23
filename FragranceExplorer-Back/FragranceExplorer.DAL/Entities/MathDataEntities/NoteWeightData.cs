using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.DAL.Entities.MathDataEntities;

public record NoteWeightData(
    int NoteId,
    double Weight,
    NoteLayer Layer
);
