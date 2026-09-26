namespace FragranceExplorer.BLL.Models;

public sealed class ScentVector : IEquatable<ScentVector>
{
    private readonly Dictionary<string, double> _significances = [];

    public IReadOnlyDictionary<string, double> Significances => _significances.AsReadOnly();

    public void AddAccordSignificance(string accordName, double significance)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accordName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(significance);

        _significances.Add(accordName, significance);
    }

    public ScentVector()
    {
    }

    public ScentVector(Dictionary<string, double> significances)
    {
        ArgumentNullException.ThrowIfNull(significances);
        _significances = significances;
    }

    public double DotProduct(ScentVector other)
    {
        ArgumentNullException.ThrowIfNull(other);

        var (smaller, larger) = _significances.Count <= other._significances.Count ?
            (_significances, other._significances) :
            (other._significances, _significances);

        double sum = 0.0;
        foreach(var (key, value) in smaller)
        {
            if (larger.TryGetValue(key, out double otherValue))
            {
                sum += value * otherValue;
            }
        }

        return sum;
    }

    public static ScentVector operator +(ScentVector left, ScentVector right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        var resultSignificances = new Dictionary<string, double>(left._significances);

        foreach(var (key, value) in right._significances)
        {
            resultSignificances[key] = resultSignificances.GetValueOrDefault(key) + value;
        }

        return new ScentVector(resultSignificances);
    }

    public static ScentVector operator *(ScentVector left, double right)
    {
        ArgumentNullException.ThrowIfNull(left);

        var resultSignificances = new Dictionary<string, double>();

        foreach(var (key, value) in left._significances)
        {
            resultSignificances[key] = value * right;
        }

        return new ScentVector(resultSignificances);
    }

    public static ScentVector operator *(double left, ScentVector right) => right * left;

    public override bool Equals(object? obj)
    {
        return Equals(obj as ScentVector);
    }

    public bool Equals(ScentVector? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        if (ReferenceEquals(_significances, other._significances))
            return true;

        if (_significances.Count != other._significances.Count)
            return false;

        foreach (var (key, value) in _significances)
        {
            if (!other._significances.TryGetValue(key, out var otherValue))
                return false;

            if (!EqualityComparer<double>.Default.Equals(value, otherValue))
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(_significances.Count);

        int elementsHash = 0;
        foreach (var (key, value) in _significances)
        {
            elementsHash ^= HashCode.Combine(key, value);
        }

        hash.Add(elementsHash);
        return hash.ToHashCode();
    }

    public static bool operator ==(ScentVector? left, ScentVector? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ScentVector? left, ScentVector? right) => !(left == right);
}
