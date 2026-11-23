using FragranceExplorer.BLL.DTOs.PerfumeRecommendationsDtos;
using FragranceExplorer.BLL.Providers;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.DAL.Repositories.Interfaces;

namespace FragranceExplorer.BLL.Handlers;

public class GetPerfumeRecommendationsHandler
{
    private readonly IPerfumeRepository _perfumeRepository;
    private readonly RecommendationEngine _recommendationEngine;
    private readonly PerfumeMathProvider _perfumeMathProvider;

    public GetPerfumeRecommendationsHandler(
        IPerfumeRepository perfumeRepository,
        RecommendationEngine recommendationEngine,
        PerfumeMathProvider perfumeMathProvider)
    {
        _perfumeRepository = perfumeRepository;
        _recommendationEngine = recommendationEngine;
        _perfumeMathProvider = perfumeMathProvider;
    }

    public async Task<PerfumeRecommendationsDto?> HandleAsync(
        GetPerfumeRecommendationsQuery query,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var models = await _perfumeMathProvider.GetModelsAsync(ct);

        if (!models.TryGetValue(query.PerfumeId, out var targetModel))
        {
            return null;
        }

        var recommendations = _recommendationEngine.GetRecommendations(
            targetModel,
            models.Values,
            query.MaxResults,
            query.SearchProfile);

        if (!recommendations.Any())
        {
            return new PerfumeRecommendationsDto([]);
        }

        var topIds = recommendations.Select(r => r.PerfumeId);
        var entities = await _perfumeRepository.GetByIdsAsync(topIds, ct);

        var entitiesById = entities.ToDictionary(e => e.Id);
        var result = new List<RecommendedPerfumeDto>(recommendations.Count());

        foreach (var rec in recommendations)
        {
            if (entitiesById.TryGetValue(rec.PerfumeId, out var entity))
            {
                result.Add(new RecommendedPerfumeDto(
                    entity.Id,
                    entity.Name,
                    entity.Brand.Name,
                    entity.ImageUrl,
                    entity.Description,
                    Math.Round(rec.Score, 4)
                ));
            }
        }

        return new PerfumeRecommendationsDto(result);
    }
}
