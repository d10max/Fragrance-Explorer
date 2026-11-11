using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.Parser.Common.Validators;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Validators;

public class HouseDtoValidator : BaseValidator<RawHouseDto>
{
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" }; // pictures and thumbai will be added to dataset in near future

    public override bool Validate(RawHouseDto? item, out List<string> validationErrors)
    {    
        validationErrors = new List<string>();
        _totalValidatedCount++;

        if (item is null)
        {
            validationErrors.Add("House cannot be null");
            _invalidCount++;
            _lastErrors.Clear();
            _lastErrors.AddRange(validationErrors);
            return false;
        }

        ValidateName(item.Name, validationErrors);
        ValidateId(item.Id, validationErrors);
        ValidateRating(item.Rating, validationErrors);
        ValidateArchiveCount(item.ArchiveCount, validationErrors);
        
        if (validationErrors.Count > 0)
        {
            _invalidCount++;
        }

        _lastErrors.Clear();
        _lastErrors.AddRange(validationErrors);
        return validationErrors.Count == 0;
    }

    private static void ValidateId(string? id, List<string> errors)
    {
        if (SanitizeText(id) is null)
        {
            errors.Add($"House ID cannot be empty. Received: {id}");
        }
    }

    private static void ValidateName(string? name, List<string> errors)
    {
        if (SanitizeText(name) is null)
        {
            errors.Add($"House name cannot be empty. Received: {name}");
        }
    }

    private static void ValidateRating(double? rating, List<string> errors)
    {
        if (rating.HasValue && (rating.Value < ScentConstants.MinPerfumeRating || rating.Value > ScentConstants.MaxPerfumeRating))
        {
            errors.Add(FormattableString.Invariant($"Perfume rating must be between {ScentConstants.MinPerfumeRating} and {ScentConstants.MaxPerfumeRating}. Received: {rating.Value}"));
        }
    }

    private static void ValidateArchiveCount(int archiveCount, List<string> errors)
    {
        if (archiveCount < 0)
        {
            errors.Add($"Count cannot be  negative. Received: {archiveCount}");
        }
    }
}