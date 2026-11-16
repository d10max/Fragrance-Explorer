using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.DAL.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<BrandEntity?> GetByIdAsync(int id, CancellationToken ct = default);
}
