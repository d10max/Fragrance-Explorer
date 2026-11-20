namespace FragranceExplorer.DAL.Entities;

public class BrandEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? Location { get; set; }

    public string? OriginLabel { get; set; }

    public string? EstablishedLabel { get; set; }

    public string? Description { get; set; }

    public int ArchiveCount { get; set; }

    public double Rating { get; set; }

    public List<string> SimilarHouseIds { get; set; } = [];

    public List<string> BrandAliases { get; set; } = [];

    public List<HouseOlfactoryDna> OlfactoryDna { get; set; } = [];

    public Dictionary<string, int> FamilyCounts { get; set; } = [];

    public ICollection<PerfumeEntity> Perfumes { get; set; } = [];

    public string Slug { get; set; } = string.Empty;

    public string? Initials { get; set; }

    public string? DossierEyebrow { get; set; }

    public string? Count { get; set; }

    public string? DnaBadge { get; set; }

    public string? ReviewsLabel { get; set; }
}
