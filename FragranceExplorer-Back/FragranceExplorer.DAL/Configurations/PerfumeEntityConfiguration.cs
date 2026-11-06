using FragranceExplorer.DAL.Constants;
using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FragranceExplorer.DAL.Configurations;

public class PerfumeEntityConfiguration : IEntityTypeConfiguration<PerfumeEntity>
{
    public void Configure(EntityTypeBuilder<PerfumeEntity> builder)
    {
        builder.ToTable("Perfumes");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.PerfumeNameMaxLength);

        builder.HasIndex(p => p.Name);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(DbConstants.UrlMaxLength);

        builder.Property(p => p.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Perfumes)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
