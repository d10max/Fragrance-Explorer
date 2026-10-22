namespace FragranceExplorer.DAL.Entities;

public class PerfumeAccordEntity
{
    public int AccordId { get; set; }

    public int PerfumeId { get; set; }

    public double Weight { get; set; }

    public PerfumeEntity Perfume { get; set; } = null!;

    public AccordEntity Accord { get; set; } = null!;
}
