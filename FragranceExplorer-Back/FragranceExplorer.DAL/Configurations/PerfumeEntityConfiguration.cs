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

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.PerfumeNameMaxLength);

        builder.Property(p => p.Slug)
            .HasMaxLength(DbConstants.UrlMaxLength);

        builder.HasIndex(p => p.Name);

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(DbConstants.UrlMaxLength);

        builder.Property(p => p.Gender)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(p => p.Rating)
            .IsRequired()
            .HasDefaultValue(0.0);

        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Perfumes)
            .HasForeignKey(p => p.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
