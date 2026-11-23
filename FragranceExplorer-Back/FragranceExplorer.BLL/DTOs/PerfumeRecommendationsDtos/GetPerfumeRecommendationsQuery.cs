using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.DTOs.PerfumeRecommendationsDtos;
public record GetPerfumeRecommendationsQuery(
    int PerfumeId,
    int MaxResults = 10,
    SearchProfile SearchProfile = SearchProfile.Default);
