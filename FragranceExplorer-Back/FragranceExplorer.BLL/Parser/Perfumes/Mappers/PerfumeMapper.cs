using System.Text.Json;
using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.BLL.Parser.Perfumes.Interfaces;
using FragranceExplorer.BLL.Parser.Perfumes.Models;
using FragranceExplorer.DAL.Enums;

namespace FragranceExplorer.BLL.Parser.Perfumes.Mappers;

public class PerfumeMapper : IPerfumeMapper
{
    private static readonly Lazy<Dictionary<string, string>> AccordToFamilyMap = new(LoadAccordToFamilyMap);

    private readonly IAccordMapper _accordMapper;
    private readonly INoteMapper _noteMapper;

    private static string NormalizeText(string? value, string fallback = "Not specified") =>
        string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

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

        var perfume = new Perfume(
            dto.Id,
            name,
            brand,
            gender,
            rating,
            imageUrl,
            year,
            slug,
            description,
            people,
            longevity,
            sillage,
            bestSeasons,
            vibeQuote,
            family,
            concentration,
            perfumers,
            similarPerfumeIds);

        foreach (var accord in _accordMapper.MapMany(dto.Accords))
        {
            perfume.AddAccord(accord);
        }

        var mappedNotes = _noteMapper.MapNotes(dto.Notes).ToList();
        var hasPyramidNotes = mappedNotes.Any(n => n.NoteLayer != NoteLayer.Flat);

        foreach (var note in mappedNotes)
        {
            if (hasPyramidNotes && note.NoteLayer == NoteLayer.Flat)
            {
                continue;
            }

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

        var map = AccordToFamilyMap.Value;

        foreach (var accord in accords.OrderByDescending(a => a.Strength))
        {
            var accordName = accord.Name?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(accordName))
            {
                continue;
            }

            if (map.TryGetValue(accordName, out var mappedFamily))
            {
                return mappedFamily;
            }
        }

        var topAccord = accords.OrderByDescending(a => a.Strength).FirstOrDefault()?.Name?.ToLowerInvariant() ?? "";

        if (topAccord.Contains("citrus")) return "Citrus";
        if (topAccord.Contains("spicy") || topAccord.Contains("amber") || topAccord.Contains("sweet") || topAccord.Contains("vanilla")) return "Spicy";
        if (topAccord.Contains("fresh") || topAccord.Contains("green") || topAccord.Contains("aromatic") || topAccord.Contains("floral") || topAccord.Contains("fruity")) return "Fresh";

        return "Woody";
    }

    private static Dictionary<string, string> LoadAccordToFamilyMap()
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
            string? mappingPath = null;

            while (currentDir is not null)
            {
                var candidate = Path.Combine(currentDir.FullName, "FragranceExplorer.BLL", "Parser", "family_mapping.json");
                if (File.Exists(candidate))
                {
                    mappingPath = candidate;
                    break;
                }

                currentDir = currentDir.Parent;
            }

            if (mappingPath is null)
            {
                return result;
            }

            using var doc = JsonDocument.Parse(File.ReadAllText(mappingPath));
            if (doc.RootElement.TryGetProperty("accord_to_family", out var accordElement) &&
                accordElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in accordElement.EnumerateObject())
                {
                    var val = prop.Value.GetString();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        result[prop.Name] = val;
                    }
                }
            }
        }
        catch
        {
            // Fallback to keyword rules if file cannot be read
        }

        return result;
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
