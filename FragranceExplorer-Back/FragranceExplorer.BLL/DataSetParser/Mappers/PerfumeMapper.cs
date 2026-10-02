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

        var perfume = new Perfume(dto.Id, name, brand, gender, rating, imageUrl);

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
}