namespace FragranceExplorer.DAL.Entities.MathDataEntities;

public sealed record PerfumeFormulaData(
    int Id,
    int? People,
    double Rating,
    IEnumerable<NoteWeightData> Notes,
    IEnumerable<AccordWeightData> Accords
);
