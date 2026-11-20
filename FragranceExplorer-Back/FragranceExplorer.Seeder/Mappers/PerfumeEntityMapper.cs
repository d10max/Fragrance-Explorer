using FragranceExplorer.BLL.Helpers;
using FragranceExplorer.BLL.Models;
using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.Seeder.Mappers;

public static class PerfumeEntityMapper
{
    public static string NormalizeComponentKey(string name) => TextNormalizer.Normalize(name);

    public static PerfumeEntity MapToEntity(
        Perfume perfume,
        int brandId,
        IReadOnlyDictionary<string, int> noteLookup,
        IReadOnlyDictionary<string, int> accordLookup)
    {
        ArgumentNullException.ThrowIfNull(perfume);
        ArgumentNullException.ThrowIfNull(noteLookup);
        ArgumentNullException.ThrowIfNull(accordLookup);

        var slug = !string.IsNullOrWhiteSpace(perfume.Slug) && perfume.Slug != "unknown-slug"
            ? perfume.Slug
            : BrandEntityMapper.BuildSlug($"{perfume.Brand}-{perfume.Name}");

        var entity = new PerfumeEntity
        {
            Id = perfume.Id,
            Name = perfume.Name,
            BrandId = brandId,
            Gender = perfume.Gender,
            ImageUrl = perfume.ImageUrl,
            Rating = perfume.Rating,
            Year = perfume.Year,
            Slug = slug,
            Description = perfume.Description,
            People = perfume.People,
            Perfumers = [.. perfume.Perfumers],
            Longevity = perfume.Longevity,
            Sillage = perfume.Sillage,
            BestSeasons = perfume.BestSeasons,
            VibeQuote = perfume.VibeQuote,
            Family = perfume.Family,
            Concentration = perfume.Concentration,
            SimilarPerfumeIds = perfume.SimilarPerfumeIds.Distinct().ToList(),
            PerfumeNotes = MapPerfumeNotes(perfume, noteLookup),
            PerfumeAccords = MapPerfumeAccords(perfume, accordLookup)
        };

        return entity;
    }

    private static List<PerfumeNoteEntity> MapPerfumeNotes(
        Perfume perfume,
        IReadOnlyDictionary<string, int> noteLookup)
    {
        var results = new List<PerfumeNoteEntity>();

        var groupedNotes = perfume.Notes
            .Select(n =>
            {
                var key = NormalizeComponentKey(n.Name);
                int noteId = 0;
                var found = !string.IsNullOrEmpty(key) && noteLookup.TryGetValue(key, out noteId);
                return (Found: found, NoteId: noteId, Note: n);
            })
            .Where(x => x.Found)
            .GroupBy(x => (x.NoteId, x.Note.NoteLayer));

        foreach (var group in groupedNotes)
        {
            results.Add(new PerfumeNoteEntity
            {
                PerfumeId = perfume.Id,
                NoteId = group.Key.NoteId,
                Layer = group.Key.NoteLayer,
                Weight = group.Max(x => x.Note.Intensity)
            });
        }

        return results;
    }

    private static List<PerfumeAccordEntity> MapPerfumeAccords(
        Perfume perfume,
        IReadOnlyDictionary<string, int> accordLookup)
    {
        var results = new List<PerfumeAccordEntity>();

        var groupedAccords = perfume.Accords
            .Select(a =>
            {
                var key = NormalizeComponentKey(a.Name);
                int accordId = 0;
                var found = !string.IsNullOrEmpty(key) && accordLookup.TryGetValue(key, out accordId);
                return (Found: found, AccordId: accordId, Accord: a);
            })
            .Where(x => x.Found)
            .GroupBy(x => x.AccordId);

        foreach (var group in groupedAccords)
        {
            results.Add(new PerfumeAccordEntity
            {
                PerfumeId = perfume.Id,
                AccordId = group.Key,
                Weight = group.Max(x => x.Accord.Intensity)
            });
        }

        return results;
    }
}
