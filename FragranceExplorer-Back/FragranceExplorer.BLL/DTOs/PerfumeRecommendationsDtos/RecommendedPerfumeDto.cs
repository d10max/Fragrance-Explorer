namespace FragranceExplorer.BLL.DTOs.PerfumeRecommendationsDtos;

public record RecommendedPerfumeDto(
    int Id,
    string Name,
    string BrandName,
    string? ImageUrl,
    string? Description,
    double SimilarityScore);
