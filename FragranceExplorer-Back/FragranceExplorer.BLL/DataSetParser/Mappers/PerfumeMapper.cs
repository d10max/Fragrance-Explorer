using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DataSetParser.Interfaces;
using FragranceExplorer.BLL.DataSetParser.Models;
using FragranceExplorer.BLL.Enums;
using FragranceExplorer.BLL.Models;

namespace FragranceExplorer.BLL.DataSetParser.Mappers;

public class PerfumeMapper : IPerfumeMapper
{
    private readonly IAccordMapper _accordMapper;
    private readonly INoteMapper _noteMapper;
    private static string NormalizeText(string? value, string fallback = "Not specified") => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    public PerfumeMapper(IAccordMapper accordMapper, INoteMapper noteMapper)
    {
        _accordMapper = accordMapper ?? throw new ArgumentNullException(nameof(accordMapper));
        _noteMapper = noteMapper ?? throw new ArgumentNullException(nameof(noteMapper));
    }

    public PerfumeMapper() : this(new AccordMapper(), new NoteMapper())
    {
    }

    public Perfume? MapToDomain(RawPerfumeDto? dto)
    {
        if (dto is null || dto.Id <= 0) 
        {
            return null;
        }

        var gender = ParseGender(dto.Gender);
        var imageUrl = ResolveImageUrl(dto.Picture, dto.Thumbnail);
        var rating = ResolveRating(dto.Rating?.Average);
        var name = NormalizeText(dto.Name, "Unknown Perfume");
        var brand = NormalizeText(dto.Brand, "Unknown Brand");
        var year = dto.Year;
        var slug = NormalizeText(dto.Slug, "unknown-slug");
        var description = NormalizeText(dto.Description, "No description available");
        var people = dto.People;
        var perfumers = dto.Perfumers?.Select(p => NormalizeText(p.Name, "Unknown Perfumer")).ToList() ?? [];
        var longevity = dto.Longevity?.Average;
        var sillage = dto.Sillage?.Average;
        var similarPerfumeIds = dto.Similar?.RemindsMeOf?.Select(s => s.Id).ToList() ?? [];
        var bestSeasons = ResolveBestSeasons(dto.Seasons);
        var vibeQuote = NormalizeText(dto.AiSummary?.Pros?.FirstOrDefault()?.Text, "A captivating fragrance");
        var family = ResolveFamily(dto.Accords);
        var concentration = ResolveConcentration(name, description);

        var perfume = new Perfume(dto.Id, name, brand, gender, rating, imageUrl, year, slug, description, people, longevity, sillage, bestSeasons, vibeQuote, family, concentration, perfumers, similarPerfumeIds);

        foreach (var accord in _accordMapper.MapMany(dto.Accords))
        {
            perfume.AddAccord(accord);
        }

        foreach (var note in _noteMapper.MapNotes(dto.Notes))
        {
            perfume.AddNote(note);
        }

        return perfume;
    }

    private static GenderCategory ParseGender(string? gender) => gender?.Trim().ToLowerInvariant() switch
    {
        "female" => GenderCategory.Female,
        "male" => GenderCategory.Male,
        _ => GenderCategory.Unisex
    };

    private static string? ResolveImageUrl(string? picture, string? thumbnail)
    {
        if (!string.IsNullOrWhiteSpace(picture))
        {
            return picture.Trim();
        }

        if (!string.IsNullOrWhiteSpace(thumbnail))
        {
            return thumbnail.Trim();
        }

        return null;
    }

    private static double ResolveRating(double? rawRating)
    {
        if (!rawRating.HasValue)
        {
            return ScentConstants.MinPerfumeRating;
        }

        return Math.Clamp(rawRating.Value, ScentConstants.MinPerfumeRating, ScentConstants.MaxPerfumeRating);
    }

    private static string ResolveBestSeasons(RawSeasonsDto? seasons)
    {
        if (seasons is null) return "All seasons";

        var votes = new (string Name, int Count)[]
        {
            ("Winter", seasons.Winter),
            ("Spring", seasons.Spring),
            ("Summer", seasons.Summer),
            ("Autumn", seasons.Autumn)
        };

        var top = votes.OrderByDescending(v => v.Count).FirstOrDefault();
        return top.Count > 0 ? top.Name : "All seasons";
    }

    private static string ResolveFamily(List<RawAccordDto>? accords)
    {
        if (accords == null || accords.Count == 0) return "Woody";

        var topAccord = accords.OrderByDescending(a => a.Strength).FirstOrDefault()?.Name?.ToLowerInvariant() ?? "";

        if (topAccord.Contains("citrus")) return "Citrus";
        if (topAccord.Contains("spicy")) return "Spicy";
        if (topAccord.Contains("fresh") || topAccord.Contains("green") || topAccord.Contains("aromatic")) return "Fresh";
        if (topAccord.Contains("amber") || topAccord.Contains("floral") || topAccord.Contains("sweet") || topAccord.Contains("vanilla")) return "Amber";

        return "Woody";
    }

    private static string ResolveConcentration(string name, string? description)
    {
        var text = $"{name} {description}".ToLowerInvariant();
        if (text.Contains("extrait")) return "Extrait de Parfum";
        if (text.Contains("eau de toilette") || text.Contains("edt")) return "Eau de Toilette";
        if (text.Contains("cologne")) return "Eau de Cologne";
        return "Eau de Parfum";
    }
}