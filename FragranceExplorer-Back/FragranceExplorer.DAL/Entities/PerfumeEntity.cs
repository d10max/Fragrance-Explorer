using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.DAL.Entities;

public class PerfumeEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public GenderCategory Gender { get; set; } = GenderCategory.Unisex;

    public string? ImageUrl { get; set; }

    public double Rating { get; set; }

    public BrandEntity Brand { get; set; } = null!;

    public ICollection<PerfumeNoteEntity> PerfumeNotes { get; set; } = [];

    public ICollection<PerfumeAccordEntity> PerfumeAccords { get; set; } = [];
}
