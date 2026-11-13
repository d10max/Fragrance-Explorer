using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Repositories;
using Raffinert.FuzzySharp;

namespace FragranceExplorer.BLL.Services;

public class PerfumeSearchService : IPerfumeSearchService
{
    private readonly IPerfumeRepository _repository;
    private readonly Lock _indexLock = new();
    private List<SearchEntry>? _indexedCatalog;

    public PerfumeSearchService(IPerfumeRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    private sealed record SearchEntry(
        Perfume Perfume,
        string NormalizedName,
        string NormalizedBrand,
        string NormalizedFullName);

    public IEnumerable<Perfume> Search(string query, int maxResults = ScentConstants.DefaultSearchResult)
    {
        var normalizedQuery = TextNormalizer.Normalize(query);
        if (string.IsNullOrEmpty(normalizedQuery) || maxResults <= 0)
        {
            return [];
        }

        int limit = Math.Min(maxResults, ScentConstants.MaxSearchResultLimit);
        var catalog = GetIndexedCatalog();

        return catalog
            .AsParallel()
            .Select(entry => (entry.Perfume, Score: CalculateScore(normalizedQuery, entry)))
            .Where(result => result.Score >= ScentConstants.MinFuzzySearchScore)
            .OrderByDescending(result => result.Score)
            .ThenByDescending(result => result.Perfume.People ?? 0)
            .ThenByDescending(result => result.Perfume.Rating)
            .Take(limit)
            .Select(result => result.Perfume)
            .ToList();
    }

    private List<SearchEntry> GetIndexedCatalog()
    {
        if (_indexedCatalog is { Count: > 0 })
        {
            return _indexedCatalog;
        }

        lock (_indexLock)
        {
            if (_indexedCatalog is { Count: > 0 })
            {
                return _indexedCatalog;
            }

            var entries = _repository
                .GetAll()
                .Select(perfume =>
                {
                    var normalizedName = TextNormalizer.Normalize(perfume.Name);
                    var normalizedBrand = TextNormalizer.Normalize(perfume.Brand);
                    var normalizedFullName = $"{normalizedBrand} {normalizedName}".Trim();

                    return new SearchEntry(
                        perfume,
                        normalizedName,
                        normalizedBrand,
                        normalizedFullName);
                })
                .ToList();

            if (entries.Count > 0)
            {
                _indexedCatalog = entries;
            }

            return entries;
        }
    }

    private static double CalculateScore(string normalizedQuery, SearchEntry entry)
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
