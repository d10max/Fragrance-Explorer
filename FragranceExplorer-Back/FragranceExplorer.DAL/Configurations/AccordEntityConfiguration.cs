using FragranceExplorer.DAL.Constants;
using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FragranceExplorer.DAL.Configurations;

public class AccordEntityConfiguration : IEntityTypeConfiguration<AccordEntity>
{
    public void Configure(EntityTypeBuilder<AccordEntity> builder)
    {
        builder.ToTable("Accords");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(DbConstants.AccordNameMaxLength);

        builder.HasIndex(a => a.Name)
            .IsUnique();
    }
}
