using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;

namespace FragranceExplorer.BLL.Models;

public class Perfume
{
    private readonly List<PerfumeNote> _notes = [];
    private readonly List<PerfumeAccord> _accords = [];

    public List<PerfumeNote> Notes => _notes;

    public List<PerfumeAccord> Accords => _accords;

    public int Id { get; private set; } 

    public string Name { get; private set; } 

    public string Brand { get; private set; } 

    public GenderCategory Gender { get; private set; } = GenderCategory.Unisex; 

    public string? ImageUrl { get; private set; } 

    public double Rating { get; private set; } 

    public int? Year {get; private set;}

    public string? Slug {get; private set;}

    public string? Description {get; private set;}

    public int? People {get; private set;}

    public List<string> Perfumers {get; private set; } = [];

    public double? Longevity {get; private set;}

    public double? Sillage { get; private set; }

    public List<int> SimilarPerfumeIds {get; private set;} = [];

    public string? BestSeasons {get; private set;}

    public string? VibeQuote {get; private set;}

    public string? Family {get; private set;}

    public string? Concentration { get; private set; }

    public Perfume(
        int id,
        string name,
        string brand,
        GenderCategory gender,
        double rating,
        string? imageUrl = null,
        int? year = null,
        string? slug = null,
        string? description = null,
        int? people = null,
        double? longevity = null,
        double? sillage = null,
        string? bestSeasons = null,
        string? vibeQuote = null,
        string? family = null,
        string? concentration = null,
        IEnumerable<string>? perfumers = null, 
        IEnumerable<int>? similarPerfumeIds = null     
        )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(brand);

        if (!Enum.IsDefined(gender))
        {
            throw new ArgumentOutOfRangeException(
                nameof(gender),
                ErrorMessagesConstants.InvalidGenderCategory);
        }

        if (rating < ScentConstants.MinPerfumeRating || rating > ScentConstants.MaxPerfumeRating)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rating),
                ErrorMessagesConstants.PerfumeRatingIsOutOfRange());
        }

        if (perfumers != null) Perfumers.AddRange(perfumers);
        if (similarPerfumeIds != null) SimilarPerfumeIds.AddRange(similarPerfumeIds);

        Id = id;
        Name = name;
        Brand = brand;
        Gender = gender;
        ImageUrl = imageUrl;
        Rating = rating;
        Year = year;
        Slug = slug;
        Description = description;
        People = people;
        Longevity = longevity;
        Sillage = sillage;
        BestSeasons = bestSeasons;
        VibeQuote = vibeQuote;
        Family = family;
        Concentration  = concentration;
    }

    public void AddNote(PerfumeNote note)
    {
        ArgumentNullException.ThrowIfNull(note);

        bool hasFlatNotes = _notes.Any(n => n.NoteLayer == NoteLayer.Flat);

        if (note.NoteLayer != NoteLayer.Flat && hasFlatNotes)
        {
            throw new ArgumentException(ErrorMessagesConstants.ErrorPyramidAddedToFlat(note.Name));
        }

        if (note.NoteLayer == NoteLayer.Flat && !hasFlatNotes)
        {
            throw new ArgumentException(ErrorMessagesConstants.ErrorFlatAddedToPyramid(note.Name));
        }

        _notes.Add(note);
    }

    public void AddAccord(PerfumeAccord accord)
    {
        ArgumentNullException.ThrowIfNull(accord);
        _accords.Add(accord);
    }

    public ScentVector<PerfumeAccord> ToAccordsVector()
    {
        var vector = new ScentVector<PerfumeAccord>();

        foreach (var accord in _accords)
        {
            vector.AddSignificance(accord);
        }

        return vector;
    }

    public ScentVector<PerfumeNote> ToNotesVector()
    {
        var vector = new ScentVector<PerfumeNote>();

        foreach (var note in _notes)
        {
            vector.AddSignificance(note);
        }

        return vector;
    }

    public ScentVector<PerfumeNote> ToNotesVectorByLayer(NoteLayer layer)
    {
        var vector = new ScentVector<PerfumeNote>();

        foreach (var note in _notes.Where(n => n.NoteLayer == layer))
        {
            vector.AddSignificance(note);
        }

        return vector;
    }

    public bool IsFlatStructure()
    {
        return _notes.Any(n => n.NoteLayer == NoteLayer.Flat);
    }

    public IEnumerable<string> KeyNotes => _notes
    .OrderByDescending(n => n.Intensity)
    .Take(3)
    .Select(n => n.Name);
}
