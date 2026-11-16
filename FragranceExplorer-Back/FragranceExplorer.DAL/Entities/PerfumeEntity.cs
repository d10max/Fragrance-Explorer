using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.DAL.Entities;

public class PerfumeEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public int BrandId { get; set; }

    public GenderCategory Gender { get; set; } = GenderCategory.Unisex;

    public string? ImageUrl { get; set; }

    public double Rating { get; set; }

    public int? Year { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int? People { get; set; }

    public List<string> Perfumers { get; set; } = [];

    public double? Longevity { get; set; }

    public double? Sillage { get; set; }

    public string? BestSeasons { get; set; }

    public string? VibeQuote { get; set; }

    public string? Family { get; set; }

    public string? Concentration { get; set; }

    public BrandEntity Brand { get; set; } = null!;

    public ICollection<PerfumeNoteEntity> PerfumeNotes { get; set; } = [];

    public ICollection<PerfumeAccordEntity> PerfumeAccords { get; set; } = [];
}
