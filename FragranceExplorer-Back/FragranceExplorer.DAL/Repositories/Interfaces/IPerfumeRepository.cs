using FragranceExplorer.DAL.Entities;
using FragranceExplorer.DAL.Entities.MathDataEntities;

namespace FragranceExplorer.DAL.Repositories.Interfaces;

public interface IPerfumeRepository
{
    Task<PerfumeEntity?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<PerfumeEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);

    Task<IEnumerable<PerfumeFormulaData>> GetAllFormulasAsync(CancellationToken ct = default);

    Task<IEnumerable<PerfumeSearchData>> GetAllForSearchAsync(CancellationToken ct = default);
}
