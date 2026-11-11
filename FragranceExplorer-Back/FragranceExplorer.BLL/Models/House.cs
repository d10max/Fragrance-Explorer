using FragranceExplorer.BLL.Constants;

namespace FragranceExplorer.BLL.Models;

public class House
{
    private readonly List<HouseOlfactoryDna> _olfactoryDna = [];
    private readonly Dictionary<string, int> _familyCounts = [];

    public List<HouseOlfactoryDna> OlfactoryDna => _olfactoryDna;

    public Dictionary<string, int> FamilyCounts => _familyCounts;

    public string Id { get; private set; }

    public string Name { get; private set; }

    public string? DisplayName { get; private set; }

    public string? Initials { get; private set; }

    public string? Location { get; private set; }

    public string? OriginLabel { get; private set; }

    public string? EstablishedLabel { get; private set; }

    public string? DossierEyebrow { get; private set; }

    public string? Description { get; private set; }

    public int ArchiveCount { get; private set; }

    public string? Count { get; private set; }

    public double Rating { get; private set; }

    public string? DnaBadge { get; private set; }

    public string? ReviewsLabel { get; private set; }

    public List<string> SimilarHouseIds { get; private set; } = [];

    public List<string> BrandAliases { get; private set; } = [];

    public House(
        string id,
        string name,
        double rating,
        int archiveCount = 0,
        string? displayName = null,
        string? initials = null,
        string? location = null,
        string? originLabel = null,
        string? establishedLabel = null,
        string? dossierEyebrow = null,
        string? description = null,
        string? count = null,
        string? dnaBadge = null,
        string? reviewsLabel = null,
        IEnumerable<HouseOlfactoryDna>? olfactoryDna = null,
        IDictionary<string, int>? familyCounts = null,
        IEnumerable<string>? similarHouseIds = null,
        IEnumerable<string>? brandAliases = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(archiveCount);

        if (rating < ScentConstants.MinPerfumeRating || rating > ScentConstants.MaxPerfumeRating)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rating),
                ErrorMessagesConstants.PerfumeRatingIsOutOfRange());
        }

        if (olfactoryDna != null) _olfactoryDna.AddRange(olfactoryDna);
        if (similarHouseIds != null) SimilarHouseIds.AddRange(similarHouseIds);
        if (brandAliases != null) BrandAliases.AddRange(brandAliases);

        if (familyCounts != null)
        {
            foreach (var kvp in familyCounts)
            {
                _familyCounts[kvp.Key] = kvp.Value;
            }
        }

        Id = id;
        Name = name;
        Rating = rating;
        ArchiveCount = archiveCount;
        DisplayName = displayName;
        Initials = initials;
        Location = location;
        OriginLabel = originLabel;
        EstablishedLabel = establishedLabel;
        DossierEyebrow = dossierEyebrow;
        Description = description;
        Count = count;
        DnaBadge = dnaBadge;
        ReviewsLabel = reviewsLabel;
    }

    public void AddOlfactoryDna(HouseOlfactoryDna dna)
    {
        ArgumentNullException.ThrowIfNull(dna);
        _olfactoryDna.Add(dna);
    }
} 