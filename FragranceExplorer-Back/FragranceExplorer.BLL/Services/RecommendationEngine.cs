using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models.MathModels;
using FragranceExplorer.BLL.Strategies;
using FragranceExplorer.DAL.Enums;

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

    public IEnumerable<SimilarityResult> GetRecommendations(
        PerfumeMathModel target,
        IEnumerable<PerfumeMathModel> catalog,
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
            .Where(candidate => candidate.Id != target.Id)
            .Select(candidate => (
                Candidate: candidate,
                Score: activeStrategy.CalculateSimilarity(target, candidate)
            ))
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Candidate.People)
            .ThenByDescending(x => x.Candidate.Rating)
            .Take(maxResults)
            .Select(x => new SimilarityResult(x.Candidate.Id, x.Score))
            .ToList();
    }
}
