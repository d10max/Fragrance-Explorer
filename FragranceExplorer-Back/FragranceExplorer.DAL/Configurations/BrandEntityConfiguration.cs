using FragranceExplorer.DAL.Constants;
using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

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

        builder.Property(b => b.Rating)
            .IsRequired()
            .HasDefaultValue(0.0);

        builder.Property(b => b.ArchiveCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.OwnsMany(b => b.OlfactoryDna, nav =>
        {
            nav.ToJson();
        });

        builder.Property(b => b.Slug)
            .IsRequired()
            .HasMaxLength(DbConstants.BrandNameMaxLength);

        builder.HasIndex(b => b.Slug)
            .IsUnique();

        var dictionaryComparer = new ValueComparer<Dictionary<string, int>>(
            (d1, d2) => d1 != null && d2 != null && d1.Count == d2.Count && !d1.Except(d2).Any(),
            d => d.Aggregate(0, (a, v) => HashCode.Combine(a, v.Key.GetHashCode(), v.Value.GetHashCode())),
            d => new Dictionary<string, int>(d));

        builder.Property(b => b.FamilyCounts)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<Dictionary<string, int>>(v, (JsonSerializerOptions?)null) ?? new())
            .Metadata.SetValueComparer(dictionaryComparer);

        builder.HasIndex(b => b.Name)
            .IsUnique();
    }
}
