using FragranceExplorer.BLL.Constants;

namespace FragranceExplorer.BLL.Models.MathModels;

public abstract class ScentComponent
{
    public int Id { get; private set; }

    public double Intensity { get; private set; }

    protected ScentComponent(int id, double intensity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        if (intensity < ScentConstants.MinIntensity || intensity > ScentConstants.MaxIntensity)
        {
            throw new ArgumentOutOfRangeException(
                ErrorMessagesConstants.ScentComponentIntensityIsOutOfRange());
        }

        Id = id;
        Intensity = intensity;
    }
}
