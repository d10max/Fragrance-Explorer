using FragranceExplorer.BLL.Constants;

namespace FragranceExplorer.BLL.Models;

public abstract class ScentComponent
{
    public string Name { get; private set; }

    public double Intensity { get; private set; }

    protected ScentComponent(string name, double intensity)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (intensity < ScentConstants.MinIntensity ||  intensity > ScentConstants.MaxIntensity)
        {
            throw new ArgumentOutOfRangeException(
                ErrorMessagesConstants.ScentComponentIntensityIsOutOfRange());
        }

        Name = name;
        Intensity = intensity;
    }

    public abstract double CalculateSignificance();
}
