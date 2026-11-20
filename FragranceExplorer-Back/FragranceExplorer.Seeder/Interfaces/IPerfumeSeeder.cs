using FragranceExplorer.Seeder.Models;

namespace FragranceExplorer.Seeder.Interfaces;

public interface IPerfumeSeeder
{
    Task SeedPerfumesAsync(
        string filePath,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken = default);
}