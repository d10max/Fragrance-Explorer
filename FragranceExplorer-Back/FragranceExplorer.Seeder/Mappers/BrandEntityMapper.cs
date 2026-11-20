using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.DAL.Entities;
using DalHouseOlfactoryDna = FragranceExplorer.DAL.Entities.HouseOlfactoryDna;

namespace FragranceExplorer.Seeder.Mappers;

public static class BrandEntityMapper
{
    public static BrandEntity MapToEntity(House house)
    {
        ArgumentNullException.ThrowIfNull(house);

        return new BrandEntity
        {
            Slug = house.Id,
            Name = house.Name,
            DisplayName = house.DisplayName ?? house.Name,
            Initials = house.Initials,
            Location = house.Location,
            OriginLabel = house.OriginLabel,
            EstablishedLabel = house.EstablishedLabel,
            DossierEyebrow = house.DossierEyebrow,
            Description = house.Description,
            ArchiveCount = house.ArchiveCount,
            Count = house.Count,
            Rating = house.Rating,
            DnaBadge = house.DnaBadge,
            ReviewsLabel = house.ReviewsLabel,
            SimilarHouseIds = [.. house.SimilarHouseIds],
            BrandAliases = [.. house.BrandAliases],
            FamilyCounts = new Dictionary<string, int>(house.FamilyCounts),
            OlfactoryDna = house.OlfactoryDna
                .Select(dna => new DalHouseOlfactoryDna
                {
                    Family = dna.Family,
                    Notes = dna.Notes,
                    Percentage = dna.Percentage
                })
                .ToList()
        };
    }

    public static BrandEntity CreateDefault(string brandName, string? customSlug = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(brandName);

        var trimmedName = brandName.Trim();
        var slug = !string.IsNullOrWhiteSpace(customSlug)
            ? customSlug
            : BuildSlug(trimmedName);

        return new BrandEntity
        {
            Slug = slug,
            Name = trimmedName,
            DisplayName = trimmedName,
            Description = "Data has not been updated yet",
            ArchiveCount = 0,
            Rating = 0.0
        };
    }

    public static string BuildSlug(string text)
    {
        var normalized = TextNormalizer.Normalize(text);
        return string.IsNullOrWhiteSpace(normalized)
            ? $"brand-{Guid.NewGuid():N}"
            : normalized.Replace(' ', '-');
    }
}