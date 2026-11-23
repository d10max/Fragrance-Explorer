using FragranceExplorer.BLL.Constants;

namespace FragranceExplorer.BLL.DTOs.PerfumeSearchDtos;

public sealed record PerfumeSearchQuery(
    string Query,
    int MaxResults = ScentConstants.DefaultSearchResult
);
