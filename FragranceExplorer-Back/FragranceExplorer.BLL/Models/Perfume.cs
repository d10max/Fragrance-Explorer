using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Enums;

namespace FragranceExplorer.BLL.Models;

public class Perfume
{
    private readonly List<PerfumeNote> _notes = [];
    private readonly List<PerfumeAccord> _accords = [];

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Brand { get; private set; }

    public GenderCategory Gender { get; private set; } = GenderCategory.Unisex;

    public string? ImageUrl { get; private set; }

    public double? Rating { get; private set; }

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

        // FIX: (rating >= Min || rating <= Max) was always true for any number
        // causing an ArgumentOutOfRangeException to be erroneously thrown for all valid values
        // Fixed range check to (< Min || > Max).
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
        _notes.Add(note);
    }

    public void AddAccord(PerfumeAccord accord)
    {
        ArgumentNullException.ThrowIfNull(accord);
        _accords.Add(accord);
    }

    public ScentVector ToAccordVector()
    {
        var vector = new ScentVector();

        foreach (var accord in _accords)
        {
            var significance = accord.CalculateSignificance();
            vector.AddAccordSignificance(accord.Name.ToLower(), significance);
        }

        return vector;
    }

    public Dictionary<string, double> ToNoteWeights()
    {
        var weights = new Dictionary<string, double>();

        foreach(var note in _notes)
        {
            var significance = note.CalculateSignificance();
            weights[note.Name] = significance;
        }

        return weights;
    }
}
