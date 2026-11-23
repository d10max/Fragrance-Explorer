namespace FragranceExplorer.BLL.DTOs.PerfumeSearchDtos;

public sealed record SearchedPerfumeDto(
    int Id,
    string Name,
    string BrandName,
    string? ImageUrl
);
