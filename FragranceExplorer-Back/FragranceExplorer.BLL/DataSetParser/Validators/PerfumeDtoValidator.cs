using FragranceExplorer.BLL.Constants;
using FragranceExplorer.BLL.DataSetParser.Models;

namespace FragranceExplorer.BLL.DataSetParser.Validators;

public class PerfumeDtoValidator : BaseValidator<RawPerfumeDto>
{
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    public override bool Validate(RawPerfumeDto? item, out List<string> validationErrors)
    {
        validationErrors = new List<string>();
        _totalValidatedCount++;

        if (item is null)
        {
            validationErrors.Add("Perfume DTO cannot be null.");
            _invalidCount++;
            _lastErrors.Clear();
            _lastErrors.AddRange(validationErrors);
            return false;
        }

        ValidateId(item.Id, validationErrors);
        ValidateRating(item.Rating?.Average, validationErrors);
        ValidateImageUrl(item.Picture, "Picture", validationErrors);
        ValidateImageUrl(item.Thumbnail, "Thumbnail", validationErrors);

        if (validationErrors.Count > 0)
        {
            _invalidCount++;
        }

        _lastErrors.Clear();
        _lastErrors.AddRange(validationErrors);
        return validationErrors.Count == 0;
    }

    private static void ValidateId(int id, List<string> errors)
    {
        if (id <= 0)
        {
            errors.Add($"Perfume ID must be greater than zero. Received: {id}");
        }
    }

    private static void ValidateRating(double? rating, List<string> errors)
    {
        if (rating.HasValue && (rating.Value < ScentConstants.MinPerfumeRating || rating.Value > ScentConstants.MaxPerfumeRating))
        {
            errors.Add($"Perfume rating must be between {ScentConstants.MinPerfumeRating} and {ScentConstants.MaxPerfumeRating}. Received: {rating.Value}");
        }
    }

    private static void ValidateImageUrl(string? url, string fieldName, List<string> errors)
    {
        var sanitized = SanitizeText(url);
        if (sanitized is null)
        {
            return;
        }

        if (!Uri.TryCreate(sanitized, UriKind.Absolute, out var uriResult) ||
            (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
        {
            errors.Add($"The {fieldName} field contains an invalid URL: {sanitized}");
            return;
        }

        var path = uriResult.AbsolutePath.ToLowerInvariant();
        var hasValidExtension = AllowedImageExtensions.Any(ext => path.EndsWith(ext));
        if (!hasValidExtension && !path.Contains('.'))
        {
            // If there is no dot or extension, it might be a CDN without an explicit extension; we skip it
        }
    }
}
