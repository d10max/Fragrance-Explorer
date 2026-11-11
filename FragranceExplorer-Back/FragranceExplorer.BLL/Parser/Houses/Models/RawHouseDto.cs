namespace FragranceExplorer.BLL.Parser.Houses.Models;

public class RawHouseDto
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? DisplayName { get; set; }

    public string? Initials { get; set; }

    public string? Location { get; set; }

    public string? OriginLabel { get; set; }

    public string? EstablishedLabel { get; set; }

    public string? DossierEyebrow { get; set; }

    public string? Description { get; set; }

    public int ArchiveCount { get; set; }

    public string? Count { get; set; }

    public double? Rating { get; set; }

    public string? DnaBadge { get; set; }

    public string? ReviewsLabel { get; set; }

    public List<RawOlfactoryDnaDto> OlfactoryDna { get; set; } = new();

    public Dictionary<string, int>? FamilyCounts { get; set; }

    public List<string> SimilarHouseIds { get; set; } = new();

    public List<string> BrandAliases { get; set; } = new(); 
}