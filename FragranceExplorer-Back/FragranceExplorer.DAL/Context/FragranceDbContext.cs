using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FragranceExplorer.DAL.Context;

public class FragranceDbContext : DbContext 
{
    public FragranceDbContext(DbContextOptions<FragranceDbContext> options) : base(options)
    {
    }

    public DbSet<AccordEntity> Accords { get; set; }
    public DbSet<BrandEntity> Brands { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    public DbSet<PerfumeAccordEntity> PerfumeAccords { get; set; }
    public DbSet<PerfumeNoteEntity> PerfumeNotes { get; set; }
    public DbSet<PerfumeEntity> Perfumes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FragranceDbContext).Assembly);
    }
}
