namespace FragranceExplorer.Seeder.Configuration;

public class SeederOptions
{
    public const string SectionName = "Seeder";

    public string DatasetsPath { get; set; } = string.Empty;

    public string HousesFile { get; set; } = "house_catalog.jsonl";

    public string PerfumesFile { get; set; } = "perfumes_actual.jsonl";

    public int BatchSize { get; set; } = 1000;
}