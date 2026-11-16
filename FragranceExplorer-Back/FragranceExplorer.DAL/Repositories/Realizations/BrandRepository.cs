using FragranceExplorer.DAL.Context;
using FragranceExplorer.DAL.Entities;
using FragranceExplorer.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FragranceExplorer.DAL.Repositories.Realizations;

public class BrandRepository : IBrandRepository
{
    private readonly FragranceDbContext _context;

    public BrandRepository(FragranceDbContext context)
    {
        _context = context;
    }

    public async Task<BrandEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Brands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, ct);
    }
}
