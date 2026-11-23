namespace FragranceExplorer.BLL.Models.MathModels;

public sealed class ScentVector<T>
    where T : ScentComponent
{
    private readonly Dictionary<int, double> _significances = [];

    public IReadOnlyDictionary<int, double> Significances => _significances.AsReadOnly();

    public void AddSignificance(T scentComponent)
    {
        ArgumentNullException.ThrowIfNull(scentComponent);

        _significances.TryAdd(scentComponent.Id, scentComponent.Intensity);
    }

    public ScentVector()
    {
    }

    public ScentVector(Dictionary<int, double> significances)
    {
        ArgumentNullException.ThrowIfNull(significances);
        _significances = significances;
    }

    public double DotProduct(ScentVector<T> other)
    {
        ArgumentNullException.ThrowIfNull(other);

        var (smaller, larger) = _significances.Count <= other._significances.Count ?
            (_significances, other._significances) :
            (other._significances, _significances);

        double sum = 0.0;
        foreach (var (key, value) in smaller)
        {
            if (larger.TryGetValue(key, out double otherValue))
            {
                sum += value * otherValue;
            }
        }

        return sum;
    }

    public double Magnitude()
    {
        double sumOfSquares = 0.0;
        foreach (var item in _significances)
        {
            sumOfSquares += Math.Pow(item.Value, 2);
        }

        return Math.Sqrt(sumOfSquares);
    }
}

