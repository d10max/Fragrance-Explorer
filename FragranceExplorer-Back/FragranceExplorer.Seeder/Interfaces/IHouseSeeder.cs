using FragranceExplorer.Seeder.Models;

namespace FragranceExplorer.Seeder.Interfaces;

public interface IHouseSeeder
{
    Task SeedHousesAsync(
        string filePath,
        SeederLookupCache cache,
        SeedingStatistics statistics,
        CancellationToken cancellationToken = default);
}