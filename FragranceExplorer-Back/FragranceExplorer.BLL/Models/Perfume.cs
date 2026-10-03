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

    public Perfume(
        int id,
        string name,
        string brand,
        GenderCategory gender,
        double rating,
        string? imageUrl = null)
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

        Id = id;
        Name = name;
        Brand = brand;
        Gender = gender;
        ImageUrl = imageUrl;
        Rating = rating;
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
}
