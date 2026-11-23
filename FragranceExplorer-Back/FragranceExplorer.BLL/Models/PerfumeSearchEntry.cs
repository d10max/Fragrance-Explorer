using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.BLL.Models;

public sealed record PerfumeSearchEntry(
    int PerfumeId,
    string NormalizedName,
    string NormalizedBrand,
    string NormalizedFullName,
    int People,
    double Rating)
{
    public static PerfumeSearchEntry FromSearchData(PerfumeSearchData data)
    {
        var normalizedName = TextNormalizer.Normalize(data.Name);
        var normalizedBrand = TextNormalizer.Normalize(data.BrandName);
        var normalizedFullName = $"{normalizedBrand} {normalizedName}".Trim();

        return new PerfumeSearchEntry(
            data.Id,
            normalizedName,
            normalizedBrand,
            normalizedFullName,
            data.People ?? 0,
            data.Rating);
    }
}
