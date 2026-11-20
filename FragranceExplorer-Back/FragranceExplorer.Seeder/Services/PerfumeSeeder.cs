using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.DAL.Constants;
using FragranceExplorer.DAL.Context;
using FragranceExplorer.DAL.Entities;
using FragranceExplorer.Seeder.Configuration;
using FragranceExplorer.Seeder.Interfaces;
using FragranceExplorer.Seeder.Mappers;
using FragranceExplorer.Seeder.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FragranceExplorer.Seeder.Services;

public class PerfumeSeeder : IPerfumeSeeder
{
    private readonly FragranceDbContext _dbContext;
    private readonly IPerfumeDataSetParser _perfumeParser;
    private readonly SeederOptions _options;
    private readonly ILogger<PerfumeSeeder> _logger;

    public PerfumeSeeder(
        FragranceDbContext dbContext,
        IPerfumeDataSetParser perfumeParser,
        IOptions<SeederOptions> options,
        ILogger<PerfumeSeeder> logger)
    {
        _dbContext = dbContext;
        _perfumeParser = perfumeParser;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedPerfumesAsync(
        string filePath,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting perfumes seeding from: {FilePath}", filePath);

        var parserOptions = new ParserOptions
        {
            PathToDataset = filePath,
            MaxPerfumesToParse = null
        };

        var batchSize = Math.Max(1, _options.BatchSize);
        var batch = new List<Perfume>(batchSize);

        await foreach (var perfume in _perfumeParser.ParseAsync(parserOptions, cancellationToken))
        {
            if (!cache.SeenPerfumeIds.Add(perfume.Id))
            {
                statistics.PerfumesSkippedDuplicates++;
                continue;
            }

            batch.Add(perfume);

            if (batch.Count >= batchSize)
            {
                await FlushBatchAsync(batch, cache, statistics, cancellationToken);
            }
        }

        if (batch.Count > 0)
        {
            await FlushBatchAsync(batch, cache, statistics, cancellationToken);
        }

        _logger.LogInformation(
            "Perfumes seeding completed. Added: {Added:N0}, Skipped duplicates: {Skipped:N0}",
            statistics.PerfumesAdded,
            statistics.PerfumesSkippedDuplicates);
        _logger.LogInformation("{ParserStats}", _perfumeParser.GetStatisticsReport());
    }

    private async Task FlushBatchAsync(
        List<Perfume> batch,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken)
    {
        var previousAutoDetect = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
        try
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            // Фаза А: збираємо та зберігаємо нові дефолтні бренди, ноти й акорди для поточного батчу
            await EnsureDictionariesForBatchAsync(batch, cache, statistics, cancellationToken);

            // Фаза Б: мапимо та зберігаємо самі парфуми з числовими FK (BrandId, NoteId, AccordId)
            var perfumeEntities = new List<PerfumeEntity>(batch.Count);

            foreach (var perfume in batch)
            {
                if (!cache.TryGetBrandId(perfume.Brand, out var brandId))
                {
                    continue;
                }

                var entity = PerfumeEntityMapper.MapToEntity(
                    perfume,
                    brandId,
                    cache.NoteIdsByKey,
                    cache.AccordIdsByKey);

                perfumeEntities.Add(entity);
            }

            _dbContext.Perfumes.AddRange(perfumeEntities);
            await _dbContext.SaveChangesAsync(cancellationToken);

            statistics.PerfumesAdded += perfumeEntities.Count;

            if (statistics.PerfumesAdded % (batch.Count * 5) == 0 || batch.Count < _options.BatchSize)
            {
                _logger.LogInformation(
                    "Progress: {Perfumes:N0} perfumes saved (Notes: {Notes:N0}, Accords: {Accords:N0}, Default brands: {DefaultBrands:N0})",
                    statistics.PerfumesAdded,
                    cache.NoteIdsByKey.Count,
                    cache.AccordIdsByKey.Count,
                    statistics.DefaultBrandsCreated);
            }
        }
        finally
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = previousAutoDetect;
            batch.Clear();
        }
    }

    private async Task EnsureDictionariesForBatchAsync(
        List<Perfume> batch,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken)
    {
        var pendingBrandsByKey = new Dictionary<string, BrandEntity>(StringComparer.OrdinalIgnoreCase);
        var pendingNotesByKey = new Dictionary<string, NoteEntity>(StringComparer.OrdinalIgnoreCase);
        var pendingAccordsByKey = new Dictionary<string, AccordEntity>(StringComparer.OrdinalIgnoreCase);

        foreach (var perfume in batch)
        {
            var brandKey = TextNormalizer.Normalize(perfume.Brand);
            if (!string.IsNullOrEmpty(brandKey) &&
                !cache.BrandIdsByKey.ContainsKey(brandKey) &&
                !pendingBrandsByKey.ContainsKey(brandKey))
            {
                var defaultBrand = CreateUniqueDefaultBrand(perfume.Brand, cache);
                pendingBrandsByKey[brandKey] = defaultBrand;
            }

            foreach (var note in perfume.Notes)
            {
                var noteKey = PerfumeEntityMapper.NormalizeComponentKey(note.Name);
                if (!string.IsNullOrEmpty(noteKey) &&
                    !cache.NoteIdsByKey.ContainsKey(noteKey) &&
                    !pendingNotesByKey.ContainsKey(noteKey))
                {
                    var trimmedName = Truncate(note.Name.Trim(), DbConstants.NoteNameMaxLength);
                    pendingNotesByKey[noteKey] = new NoteEntity { Name = trimmedName };
                }
            }

            foreach (var accord in perfume.Accords)
            {
                var accordKey = PerfumeEntityMapper.NormalizeComponentKey(accord.Name);
                if (!string.IsNullOrEmpty(accordKey) &&
                    !cache.AccordIdsByKey.ContainsKey(accordKey) &&
                    !pendingAccordsByKey.ContainsKey(accordKey))
                {
                    var trimmedName = Truncate(accord.Name.Trim(), DbConstants.AccordNameMaxLength);
                    pendingAccordsByKey[accordKey] = new AccordEntity { Name = trimmedName };
                }
            }
        }

        if (pendingBrandsByKey.Count == 0 && pendingNotesByKey.Count == 0 && pendingAccordsByKey.Count == 0)
        {
            return;
        }

        if (pendingBrandsByKey.Count > 0)
        {
            _dbContext.Brands.AddRange(pendingBrandsByKey.Values);
        }

        if (pendingNotesByKey.Count > 0)
        {
            _dbContext.Notes.AddRange(pendingNotesByKey.Values);
        }

        if (pendingAccordsByKey.Count > 0)
        {
            _dbContext.Accords.AddRange(pendingAccordsByKey.Values);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var brand in pendingBrandsByKey.Values)
        {
            cache.RegisterBrand(brand);
        }

        foreach (var (key, noteEntity) in pendingNotesByKey)
        {
            cache.NoteIdsByKey[key] = noteEntity.Id;
        }

        foreach (var (key, accordEntity) in pendingAccordsByKey)
        {
            cache.AccordIdsByKey[key] = accordEntity.Id;
        }

        statistics.DefaultBrandsCreated += pendingBrandsByKey.Count;
        statistics.NotesAdded += pendingNotesByKey.Count;
        statistics.AccordsAdded += pendingAccordsByKey.Count;

        _dbContext.ChangeTracker.Clear();
    }

    private static BrandEntity CreateUniqueDefaultBrand(string rawBrandName, SeederLookupCache cache)
    {
        var baseName = Truncate(rawBrandName.Trim(), DbConstants.BrandNameMaxLength - 10);
        var uniqueName = baseName;
        int nameSuffix = 1;

        while (!cache.ExistingBrandNames.Add(uniqueName))
        {
            uniqueName = $"{baseName} ({nameSuffix++})";
        }

        var baseSlug = Truncate(BrandEntityMapper.BuildSlug(uniqueName), DbConstants.BrandNameMaxLength - 10);
        var uniqueSlug = baseSlug;
        int slugSuffix = 1;

        while (!cache.ExistingBrandSlugs.Add(uniqueSlug))
        {
            uniqueSlug = $"{baseSlug}-{slugSuffix++}";
        }

        return BrandEntityMapper.CreateDefault(uniqueName, uniqueSlug);
    }

    private static string Truncate(string value, int maxLength) =>
        value.Length <= maxLength ? value : value[..maxLength];
}
