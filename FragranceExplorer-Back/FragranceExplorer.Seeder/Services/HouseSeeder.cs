using FragranceExplorer.BLL.Parser.Common;
using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.DAL.Context;
using FragranceExplorer.DAL.Entities;
using FragranceExplorer.Seeder.Configuration;
using FragranceExplorer.Seeder.Interfaces;
using FragranceExplorer.Seeder.Mappers;
using FragranceExplorer.Seeder.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FragranceExplorer.Seeder.Services;

public class HouseSeeder : IHouseSeeder
{
    private readonly FragranceDbContext _dbContext;
    private readonly IHouseDataSetParser _houseParser;
    private readonly SeederOptions _options;
    private readonly ILogger<HouseSeeder> _logger;

    public HouseSeeder(
        FragranceDbContext dbContext,
        IHouseDataSetParser houseParser,
        IOptions<SeederOptions> options,
        ILogger<HouseSeeder> logger)
    {
        _dbContext = dbContext;
        _houseParser = houseParser;
        _options = options.Value;
        _logger = logger;
    }

    public async Task SeedHousesAsync(
        string filePath,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting houses seeding from: {FilePath}", filePath);

        var parserOptions = new ParserOptions
        {
            PathToDataset = filePath,
            MaxPerfumesToParse = null
        };

        var batchSize = Math.Max(1, _options.BatchSize);
        var batch = new List<BrandEntity>(batchSize);

        await foreach (var house in _houseParser.ParseAsync(parserOptions, cancellationToken))
        {
            if (cache.ExistingBrandNames.Contains(house.Name) ||
                cache.ExistingBrandSlugs.Contains(house.Id))
            {
                statistics.HousesSkippedDuplicates++;
                continue;
            }

            var entity = BrandEntityMapper.MapToEntity(house);

            cache.ExistingBrandNames.Add(entity.Name);
            cache.ExistingBrandSlugs.Add(entity.Slug);

            batch.Add(entity);

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
            "Houses seeding completed. Added: {Added:N0}, Skipped duplicates: {Skipped:N0}",
            statistics.HousesAdded,
            statistics.HousesSkippedDuplicates);
        _logger.LogInformation("{ParserStats}", _houseParser.GetStatisticsReport());
    }

    private async Task FlushBatchAsync(
        List<BrandEntity> batch,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken)
    {
        var previousAutoDetect = _dbContext.ChangeTracker.AutoDetectChangesEnabled;
        try
        {
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            _dbContext.Brands.AddRange(batch);
            await _dbContext.SaveChangesAsync(cancellationToken);

            foreach (var brand in batch)
            {
                cache.RegisterBrand(brand);
            }

            statistics.HousesAdded += batch.Count;
            _logger.LogInformation("Saved houses batch. Total houses added so far: {Total:N0}", statistics.HousesAdded);
        }
        finally
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = previousAutoDetect;
            batch.Clear();
        }
    }
}
