using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Houses.Interfaces;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Mappers;

public class HouseMapper : IHouseMapper
{
    private readonly IOlfactoryDnaMapper _olfactoryDnaMapper;

    private static string NormalizeText(string? value, string fallback = "Not specified") =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    public HouseMapper(IOlfactoryDnaMapper olfactoryDnaMapper)
    {
        _olfactoryDnaMapper = olfactoryDnaMapper ?? throw new ArgumentNullException(nameof(olfactoryDnaMapper));
    }

    public HouseMapper() : this(new OlfactoryDnaMapper())
    {
    }

    public House? MapToDomain(RawHouseDto? dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Id) || string.IsNullOrWhiteSpace(dto.Name))
        {
            return null;
        }

        var id = dto.Id.Trim();
        var name = dto.Name.Trim();
        var displayName = NormalizeText(dto.DisplayName, name);
        var initials = ResolveInitials(dto.Initials, name);
        var location = NormalizeText(dto.Location);
        var originLabel = NormalizeText(dto.OriginLabel);
        var establishedLabel = NormalizeText(dto.EstablishedLabel);
        var dossierEyebrow = NormalizeText(dto.DossierEyebrow, "FRAGRANCE HOUSE DOSSIER");
        var description = NormalizeText(dto.Description, "No description available");
        var archiveCount = Math.Max(0, dto.ArchiveCount);
        var count = NormalizeText(dto.Count, $"{archiveCount} perfumes");
        var rating = ResolveRating(dto.Rating);
        var dnaBadge = NormalizeText(dto.DnaBadge, "Signature DNA");
        var reviewsLabel = NormalizeText(dto.ReviewsLabel, "0 reviews");

        var similarHouseIds = dto.SimilarHouseIds?
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .ToList() ?? [];

        var brandAliases = dto.BrandAliases?
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .Select(a => a.Trim())
            .ToList() ?? [];

        var house = new House(
            id,
            name,
            rating,
            archiveCount,
            displayName,
            initials,
            location,
            originLabel,
            establishedLabel,
            dossierEyebrow,
            description,
            count,
            dnaBadge,
            reviewsLabel,
            olfactoryDna: null,
            familyCounts: dto.FamilyCounts,
            similarHouseIds: similarHouseIds,
            brandAliases: brandAliases);

        foreach (var dna in _olfactoryDnaMapper.MapMany(dto.OlfactoryDna))
        {
            house.AddOlfactoryDna(dna);
        }

        return house;
    }

    private static string ResolveInitials(string? initials, string name)
    {
        if (!string.IsNullOrWhiteSpace(initials))
        {
            return initials.Trim().ToUpperInvariant();
        }

        return name.Length >= 2
            ? name[..2].ToUpperInvariant()
            : name.ToUpperInvariant();
    }

    private static double ResolveRating(double? rawRating)
    {
        if (!rawRating.HasValue)
        {
            return ScentConstants.MinPerfumeRating;
        }

        return Math.Clamp(rawRating.Value, ScentConstants.MinPerfumeRating, ScentConstants.MaxPerfumeRating);
    }
}

