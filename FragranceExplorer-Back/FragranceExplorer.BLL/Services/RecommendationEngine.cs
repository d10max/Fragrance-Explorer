using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Strategies;

namespace FragranceExplorer.BLL.Services;

public class RecommendationEngine
{
    private readonly ISimilarityStrategy _accordStrategy;
    private readonly ISimilarityStrategy _noteStrategy;

    public RecommendationEngine(ISimilarityStrategy accordStrategy, ISimilarityStrategy noteStrategy)
    {
        ArgumentNullException.ThrowIfNull(accordStrategy);
        ArgumentNullException.ThrowIfNull(noteStrategy);

        _accordStrategy = accordStrategy;
        _noteStrategy = noteStrategy;
    }

    public IEnumerable<ScoredPerfume> GetRecommendations(
        Perfume target,
        IEnumerable<Perfume> catalog,
        int maxResults,
        SearchProfile searchProfile = SearchProfile.Default)
    {
        var (accordWeight, noteWeight) = ScentConstants.GetSimilarityWeights(searchProfile);

        var activeStrategy = new CombinedSimilarityStrategy(
            _noteStrategy,
            _accordStrategy,
            accordWeight,
            noteWeight);

        return catalog
            .AsParallel()
            .Where(perfume => perfume.Id != target.Id)
            .Select(perfume => new ScoredPerfume(
                perfume,
                activeStrategy.CalculateSimilarity(target, perfume)))
            .OrderByDescending(result => result.Score)
            .Take(maxResults)
            .ToList();
    }
}
