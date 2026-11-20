using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.Seeder.Models;

public class SeederLookupCache
{
    public Dictionary<string, int> BrandIdsByKey { get; } = new(StringComparer.OrdinalIgnoreCase);

    public HashSet<string> ExistingBrandNames { get; } = new(StringComparer.OrdinalIgnoreCase);
    public HashSet<string> ExistingBrandSlugs { get; } = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, int> NoteIdsByKey { get; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, int> AccordIdsByKey { get; } = new(StringComparer.OrdinalIgnoreCase);

    public HashSet<int> SeenPerfumeIds { get; } = [];

    public void RegisterBrand(BrandEntity brand)
    {
        ExistingBrandNames.Add(brand.Name);
        ExistingBrandSlugs.Add(brand.Slug);

        AddBrandKey(brand.Name, brand.Id);
        AddBrandKey(brand.DisplayName, brand.Id);
        AddBrandKey(brand.Slug, brand.Id);

        foreach (var alias in brand.BrandAliases)
        {
            AddBrandKey(alias, brand.Id);
        }
    }

    public bool TryGetBrandId(string? rawBrandName, out int brandId)
    {
        var key = TextNormalizer.Normalize(rawBrandName);
        if (string.IsNullOrEmpty(key))
        {
            brandId = 0;
            return false;
        }

        return BrandIdsByKey.TryGetValue(key, out brandId);
    }

    private void AddBrandKey(string? rawValue, int brandId)
    {
        var key = TextNormalizer.Normalize(rawValue);
        if (!string.IsNullOrEmpty(key))
        {
            BrandIdsByKey.TryAdd(key, brandId);
        }
    }
}