using FragranceExplorer.DAL.Context;
using FragranceExplorer.DAL.Entities;
using FragranceExplorer.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FragranceExplorer.DAL.Repositories.Realizations;

public class PerfumeRepository : IPerfumeRepository
{
    private readonly FragranceDbContext _context;

    public PerfumeRepository(FragranceDbContext context)
    {
        _context = context;
    }

    public async Task<PerfumeEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Perfumes
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Brand)
            .Include(p => p.PerfumeNotes)
                .ThenInclude(pn => pn.Note)
            .Include(p => p.PerfumeAccords)
                .ThenInclude(pa => pa.Accord)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IEnumerable<PerfumeEntity>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
        {
            return [];
        }

        return await _context.Perfumes
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Brand)
            .Include(P => P.PerfumeNotes)
                .ThenInclude(pn => pn.Note)
            .Include(p => p.PerfumeAccords)
                .ThenInclude(pa => pa.Accord)
            .Where(p => idList.Contains(p.Id))
            .ToListAsync(ct);
    }
}
