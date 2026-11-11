using FragranceExplorer.BLL.Parser.Common.Validators;
using FragranceExplorer.BLL.Parser.Houses.Models;

namespace FragranceExplorer.BLL.Parser.Houses.Validators;

public class OlfactoryDnaDtoValidator : BaseValidator<RawOlfactoryDnaDto>
{
    public override bool Validate(RawOlfactoryDnaDto? item, out List<string> validationErrors)
    {
        validationErrors = new List<string>();
        _totalValidatedCount++;

        if (item is null)
        {
            validationErrors.Add("Olfactory DNA DTO cannot be null.");
            _invalidCount++;
            _lastErrors.Clear();
            _lastErrors.AddRange(validationErrors);
            return false;
        }

        ValidateFamity(item.Family, validationErrors);
        ValidateNotes(item.Notes, validationErrors);
        ValidatePercentage(item.Percentage, validationErrors);

        if (validationErrors.Count > 0)
        {
            _invalidCount++;
        }

        _lastErrors.Clear();
        _lastErrors.AddRange(validationErrors);
        return validationErrors.Count == 0;
    }

    private static void ValidateFamity(string? family, List<string> validationErrors)
    {
        if (string.IsNullOrWhiteSpace(family))
        {
            validationErrors.Add($"Family cannot be null or empty. Received: {family}");
        }
    }

    private static void ValidateNotes(string? notes, List<string> validationErrors)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            validationErrors.Add($"Notes cannot be null or empty. Received: {notes}");
        }
    }
    
    private static void ValidatePercentage(int percentage, List<string> validationErrors)
    {
        if (percentage < 0 || percentage > 100)
        {
            validationErrors.Add($"Percentage must be between 0 and 100. Received: {percentage}");
        }
    }
}