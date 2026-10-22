namespace FragranceExplorer.DAL.Entities;

public class AccordEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<PerfumeAccordEntity> PerfumeAccords { get; set; } = []; 
}
