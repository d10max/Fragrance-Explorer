using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FragranceExplorer.DAL.Configurations;

public class PerfumeAccordEntityConfiguration : IEntityTypeConfiguration<PerfumeAccordEntity>
{
    public void Configure(EntityTypeBuilder<PerfumeAccordEntity> builder)
    {
        builder.ToTable("PerfumeAccords");

        builder.HasKey(pa => new { pa.PerfumeId, pa.AccordId });

        builder.Property(pa => pa.Weight)
            .IsRequired();

        builder.HasOne(pa => pa.Perfume)
            .WithMany(p => p.PerfumeAccords)
            .HasForeignKey(pa => pa.PerfumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pa => pa.Accord)
            .WithMany(a => a.PerfumeAccords)
            .HasForeignKey(pa => pa.AccordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
