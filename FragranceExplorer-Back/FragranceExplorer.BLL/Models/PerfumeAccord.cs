namespace FragranceExplorer.BLL.Models;

public class PerfumeAccord : ScentComponent
{
    public PerfumeAccord(string name, double intensity)
        : base(name, intensity)
    {
    }

    public override double CalculateSignificance() => Intensity;
}
