using FragranceExplorer.BLL.DTOs.PerfumeSearchDtos;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Services;
using FragranceExplorer.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FragranceExplorer.BLL.Handlers;

public sealed class PerfumeSearchHandler
{
    private const string SearchCacheKey = "perfume_search_entries";

    private readonly IPerfumeRepository _perfumeRepository;
    private readonly IPerfumeSearchService _searchService;
    private readonly IMemoryCache _memoryCache;

    public PerfumeSearchHandler(
        IPerfumeRepository perfumeRepository,
        IPerfumeSearchService searchService,
        IMemoryCache memoryCache)
    {
        _perfumeRepository = perfumeRepository;
        _searchService = searchService;
        _memoryCache = memoryCache;
    }

    public async Task<PerfumeSearchDto> HandleAsync(
        PerfumeSearchQuery query,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var entries = await _memoryCache.GetOrCreateAsync(SearchCacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            var searchData = await _perfumeRepository.GetAllForSearchAsync(ct);
            return searchData.Select(PerfumeSearchEntry.FromSearchData).ToList();
        }) ?? [];

        var matchedIds = _searchService.Search(query.Query, entries, query.MaxResults);
        if (!matchedIds.Any())
        {
            return new PerfumeSearchDto([]);
        }

        var entities = await _perfumeRepository.GetByIdsAsync(matchedIds, ct);
        var entitiesById = entities.ToDictionary(e => e.Id);
        var result = new List<SearchedPerfumeDto>(matchedIds.Count());

        foreach (var id in matchedIds)
        {
            if (entitiesById.TryGetValue(id, out var entity))
            {
                result.Add(new SearchedPerfumeDto(
                    entity.Id,
                    entity.Name,
                    entity.Brand.Name,
                    entity.ImageUrl
                ));
            }
        }

        return new PerfumeSearchDto(result);
    }
}
