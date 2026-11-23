using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.BLL.Models;
using Raffinert.FuzzySharp;

namespace FragranceExplorer.BLL.Services;

public class PerfumeSearchService : IPerfumeSearchService
{
    public IEnumerable<int> Search(
        string query,
        IEnumerable<PerfumeSearchEntry> catalog, 
        int maxResults = ScentConstants.DefaultSearchResult)
    {
        ArgumentNullException.ThrowIfNull(catalog);

        var normalizedQuery = TextNormalizer.Normalize(query);
        if (string.IsNullOrEmpty(normalizedQuery) || maxResults <= 0)
        {
            return [];
        }

        int limit = Math.Min(maxResults, ScentConstants.MaxSearchResultLimit);

        return catalog
            .AsParallel()
            .Select(entry => (entry.PerfumeId, entry.People, entry.Rating, Score: CalculateScore(normalizedQuery, entry)))
            .Where(result => result.Score >= ScentConstants.MinFuzzySearchScore)
            .OrderByDescending(result => result.Score)
            .ThenByDescending(result => result.People)
            .ThenByDescending(result => result.Rating)
            .Take(limit)
            .Select(result => result.PerfumeId)
            .ToList();
    }

    private static double CalculateScore(string normalizedQuery, PerfumeSearchEntry entry)
    {
        double fullNameScore = Fuzz.WeightedRatio(normalizedQuery, entry.NormalizedFullName);
        double nameScore = Fuzz.WeightedRatio(normalizedQuery, entry.NormalizedName);
        double score = Math.Max(fullNameScore, nameScore);

        if (entry.NormalizedName.StartsWith(normalizedQuery, StringComparison.Ordinal) ||
            entry.NormalizedFullName.StartsWith(normalizedQuery, StringComparison.Ordinal))
        {
            score = Math.Min(ScentConstants.MaxFuzzySearchScore, score + ScentConstants.PrefixMatchBoost);
        }

        return score;
    }
}
