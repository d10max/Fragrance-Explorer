using FragranceExplorer.DAL.Entities;

namespace FragranceExplorer.DAL.Repositories.Interfaces;

public interface IPerfumeRepository
{
    Task<PerfumeEntity?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<PerfumeEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
}
