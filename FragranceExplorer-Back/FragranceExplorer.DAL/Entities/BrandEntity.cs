namespace FragranceExplorer.DAL.Entities;

public class BrandEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<PerfumeEntity> Perfumes { get; set; } = [];
}
