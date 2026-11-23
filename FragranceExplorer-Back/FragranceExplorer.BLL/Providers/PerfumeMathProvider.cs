using FragranceExplorer.BLL.Models.MathModels;
using FragranceExplorer.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace FragranceExplorer.BLL.Providers;

public sealed class PerfumeMathProvider
{
    private const string CacheKey = "perfume_math_models";
    private readonly IPerfumeRepository _repository;
    private readonly IMemoryCache _cache;

    public PerfumeMathProvider(IPerfumeRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<Dictionary<int, PerfumeMathModel>> GetModelsAsync(CancellationToken ct = default)
    {
        return await _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.SetPriority(CacheItemPriority.NeverRemove);
            var formulas = await _repository.GetAllFormulasAsync(ct);
            return formulas.ToDictionary(f => f.Id, PerfumeMathModel.FromFormulaData);
        }) ?? [];
    }
}