namespace FragranceExplorer.Seeder.Configuration;

public class SeederCliOptions
{
    public bool Reseed { get; init; }

    public bool HousesOnly { get; init; }

    public bool PerfumesOnly { get; init; }

    public static SeederCliOptions Parse(string[] args)
    {
        return new SeederCliOptions
        {
            Reseed = args.Contains("--reseed", StringComparer.OrdinalIgnoreCase),
            HousesOnly = args.Contains("--houses-only", StringComparer.OrdinalIgnoreCase),
            PerfumesOnly = args.Contains("--perfumes-only", StringComparer.OrdinalIgnoreCase)
        };
    }
}