using System.Text;

namespace FragranceExplorer.Seeder.Models;

public class SeedingStatistics
{
    public int HousesAdded { get; set; }

    public int HousesSkippedDuplicates { get; set; }

    public int DefaultBrandsCreated { get; set; }

    public int NotesAdded { get; set; }

    public int AccordsAdded { get; set; }

    public int PerfumesAdded { get; set; }

    public int PerfumesSkippedDuplicates { get; set; }

    public string GetSummaryReport(TimeSpan elapsed)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("             DATABASE SEEDING SUMMARY            ");
        sb.AppendLine("                                                 ");
        sb.AppendLine($"Elapsed time:               {elapsed:hh\\:mm\\:ss\\.fff}");
        sb.AppendLine($"Houses added:               {HousesAdded:N0}");
        sb.AppendLine($"Houses skipped (duplicates):{HousesSkippedDuplicates:N0}");
        sb.AppendLine($"Default brands created:     {DefaultBrandsCreated:N0}");
        sb.AppendLine($"Unique notes added:         {NotesAdded:N0}");
        sb.AppendLine($"Unique accords added:       {AccordsAdded:N0}");
        sb.AppendLine($"Perfumes added:             {PerfumesAdded:N0}");
        sb.AppendLine($"Perfumes skipped (dups):    {PerfumesSkippedDuplicates:N0}");
        sb.AppendLine("                                                           ");
        return sb.ToString();
    }
}