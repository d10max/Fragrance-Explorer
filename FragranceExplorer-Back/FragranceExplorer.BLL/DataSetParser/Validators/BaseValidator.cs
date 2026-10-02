using FragranceExplorer.BLL.DataSetParser.Interfaces;

namespace FragranceExplorer.BLL.DataSetParser.Validators;

public abstract class BaseValidator<T> : IValidator<T> where T : class
{
    protected readonly List<string> _lastErrors = new();
    protected long _totalValidatedCount;
    protected long _invalidCount;

    public IReadOnlyList<string> LastErrors => _lastErrors.AsReadOnly();
    public long TotalValidatedCount => _totalValidatedCount;
    public long InvalidCount => _invalidCount;

    public abstract bool Validate(T? item, out List<string> validationErrors);

    public virtual bool Validate(T? item)
    {
        return Validate(item, out _);
    }

    public virtual IEnumerable<T> FilterValid(IEnumerable<T>? items)
    {
        if (items is null)
        {
            yield break;
        }

        foreach (var item in items)
        {
            if (Validate(item))
            {
                yield return item;
            }
        }
    }

    public double GetValidationFailureRate()
    {
        if (_totalValidatedCount == 0)
        {
            return 0.0;
        }

        return Math.Round((double)_invalidCount / _totalValidatedCount * 100.0, 2);
    }

    protected static string? SanitizeText(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var trimmed = input.Trim();
        return trimmed.Length > 0 ? trimmed : null;
    }
}
