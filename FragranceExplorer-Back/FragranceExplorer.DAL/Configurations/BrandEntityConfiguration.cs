using FragranceExplorer.DAL.Constants;
using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FragranceExplorer.DAL.Configurations;

public class BrandEntityConfiguration : IEntityTypeConfiguration<BrandEntity>
{
    public void Configure(EntityTypeBuilder<BrandEntity> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.BrandNameMaxLength);

        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}
