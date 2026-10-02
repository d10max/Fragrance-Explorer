using FragranceExplorer.BLL.DataSetParser.Models;

namespace FragranceExplorer.BLL.DataSetParser.Validators;

public class NoteDtoValidator : BaseValidator<RawNoteDto>
{
    public override bool Validate(RawNoteDto? item, out List<string> validationErrors)
    {
        validationErrors = new List<string>();
        _totalValidatedCount++;

        if (item is null)
        {
            validationErrors.Add("Note DTO cannot be null.");
            _invalidCount++;
            _lastErrors.Clear();
            _lastErrors.AddRange(validationErrors);
            return false;
        }

        ValidateName(item.Name, validationErrors);
        ValidateWeight(item.Weight, validationErrors);

        if (validationErrors.Count > 0)
        {
            _invalidCount++;
        }

        _lastErrors.Clear();
        _lastErrors.AddRange(validationErrors);
        return validationErrors.Count == 0;
    }

    private static void ValidateName(string? name, List<string> errors)
    {
        var sanitized = SanitizeText(name);
        if (sanitized is null)
        {
            errors.Add("Note name cannot be empty.");
        }
    }

    private static void ValidateWeight(int weight, List<string> errors)
    {
        if (weight < 0 || weight > 100)
        {
            errors.Add($"Note weight must be between 0 and 100. Received: {weight}");
        }
    }
}
