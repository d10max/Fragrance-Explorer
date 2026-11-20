using System.Diagnostics;
using System.IO.Compression;
using FragranceExplorer.DAL.Context;
using FragranceExplorer.Seeder.Configuration;
using FragranceExplorer.Seeder.Interfaces;
using FragranceExplorer.Seeder.Mappers;
using FragranceExplorer.Seeder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FragranceExplorer.Seeder.Services;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly FragranceDbContext _dbContext;
    private readonly IHouseSeeder _houseSeeder;
    private readonly IPerfumeSeeder _perfumeSeeder;
    private readonly SeederOptions _options;
    private readonly SeederCliOptions _cliOptions;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        FragranceDbContext dbContext,
        IHouseSeeder houseSeeder,
        IPerfumeSeeder perfumeSeeder,
        IOptions<SeederOptions> options,
        SeederCliOptions cliOptions,
        ILogger<DatabaseSeeder> logger)
    {
        _dbContext = dbContext;
        _houseSeeder = houseSeeder;
        _perfumeSeeder = perfumeSeeder;
        _options = options.Value;
        _cliOptions = cliOptions;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var cache = new SeederLookupCache();
        var statistics = new SeedingStatistics();

        _logger.LogInformation("Applying pending EF Core migrations (if any)...");
        await _dbContext.Database.MigrateAsync(cancellationToken);

        if (_cliOptions.Reseed)
        {
            _logger.LogWarning("--reseed flag detected. Truncating all tables...");
            await TruncateAllTablesAsync(cancellationToken);
        }

        await WarmUpCacheFromDatabaseAsync(cache, cancellationToken);

        var seedHouses = !_cliOptions.PerfumesOnly;
        var seedPerfumes = !_cliOptions.HousesOnly;

        if (seedHouses)
        {
            var hasBrands = await _dbContext.Brands.AnyAsync(cancellationToken);
            if (hasBrands && !_cliOptions.Reseed)
            {
                _logger.LogInformation("Brands table already contains data. Skipping houses seeding (use --reseed to overwrite).");
            }
            else
            {
                var housesFilePath = ResolveDatasetFilePath(_options.HousesFile);
                await _houseSeeder.SeedHousesAsync(housesFilePath, cache, statistics, cancellationToken);
            }
        }

        if (seedPerfumes)
        {
            var hasPerfumes = await _dbContext.Perfumes.AnyAsync(cancellationToken);
            if (hasPerfumes && !_cliOptions.Reseed)
            {
                _logger.LogInformation("Perfumes table already contains data. Skipping perfumes seeding (use --reseed to overwrite).");
            }
            else
            {
                var perfumesFilePath = ResolveDatasetFilePath(_options.PerfumesFile);
                await _perfumeSeeder.SeedPerfumesAsync(perfumesFilePath, cache, statistics, cancellationToken);
            }
        }

        stopwatch.Stop();
        _logger.LogInformation("\n{Summary}", statistics.GetSummaryReport(stopwatch.Elapsed));
    }

    private async Task TruncateAllTablesAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            TRUNCATE TABLE "PerfumeNotes", "PerfumeAccords", "Perfumes", "Brands", "Notes", "Accords"
            RESTART IDENTITY CASCADE;
            """;

        await _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        _logger.LogInformation("All tables truncated successfully.");
    }

    private async Task WarmUpCacheFromDatabaseAsync(SeederLookupCache cache, CancellationToken cancellationToken)
    {
        var existingBrands = await _dbContext.Brands
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var brand in existingBrands)
        {
            cache.RegisterBrand(brand);
        }

        var existingNotes = await _dbContext.Notes
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var note in existingNotes)
        {
            var key = PerfumeEntityMapper.NormalizeComponentKey(note.Name);
            if (!string.IsNullOrEmpty(key))
            {
                cache.NoteIdsByKey.TryAdd(key, note.Id);
            }
        }

        var existingAccords = await _dbContext.Accords
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var accord in existingAccords)
        {
            var key = PerfumeEntityMapper.NormalizeComponentKey(accord.Name);
            if (!string.IsNullOrEmpty(key))
            {
                cache.AccordIdsByKey.TryAdd(key, accord.Id);
            }
        }

        if (existingBrands.Count > 0 || existingNotes.Count > 0 || existingAccords.Count > 0)
        {
            _logger.LogInformation(
                "Loaded existing lookups from DB: {Brands} brands, {Notes} notes, {Accords} accords.",
                existingBrands.Count,
                existingNotes.Count,
                existingAccords.Count);
        }
    }

    private string ResolveDatasetFilePath(string fileName)
    {
        var directPath = Path.GetFullPath(Path.Combine(_options.DatasetsPath, fileName));
        if (File.Exists(directPath))
        {
            return directPath;
        }

        var searchRoots = new[]
        {
            new DirectoryInfo(AppContext.BaseDirectory),
            new DirectoryInfo(Directory.GetCurrentDirectory())
        };

        foreach (var root in searchRoots)
        {
            var currentDir = root;
            while (currentDir is not null)
            {
                var candidateFromOption = Path.GetFullPath(Path.Combine(currentDir.FullName, _options.DatasetsPath, fileName));
                if (File.Exists(candidateFromOption))
                {
                    return candidateFromOption;
                }

                var candidateInBll = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "Parser", fileName);
                if (File.Exists(candidateInBll))
                {
                    return candidateInBll;
                }

                var zipFileName = Path.ChangeExtension(fileName, ".zip");
                var zipCandidateInBll = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "Parser", zipFileName);
                if (File.Exists(zipCandidateInBll))
                {
                    var extractDir = Path.GetDirectoryName(zipCandidateInBll)!;
                    _logger.LogInformation("Extracting {ZipFile} to {ExtractDir}...", zipCandidateInBll, extractDir);
                    ZipFile.ExtractToDirectory(zipCandidateInBll, extractDir, overwriteFiles: true);

                    if (File.Exists(candidateInBll))
                    {
                        return candidateInBll;
                    }
                }

                currentDir = currentDir.Parent;
            }
        }

        throw new FileNotFoundException($"Dataset file '{fileName}' was not found in '{_options.DatasetsPath}' or parent directories.");
    }
}
