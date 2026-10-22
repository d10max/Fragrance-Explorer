using FragranceExplorer.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FragranceExplorer.DAL.Configurations;

public class PerfumeNoteEntityConfiguration : IEntityTypeConfiguration<PerfumeNoteEntity>
{
    public void Configure(EntityTypeBuilder<PerfumeNoteEntity> builder)
    {
        builder.ToTable("PerfumeNotes");

        builder.HasKey(pn => new { pn.PerfumeId, pn.NoteId });

        builder.Property(pn => pn.Layer)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(pn => pn.Weight)
            .IsRequired();

        builder.HasOne(pn => pn.Perfume)
            .WithMany(p => p.PerfumeNotes)
            .HasForeignKey(pn => pn.PerfumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pn => pn.Note)
            .WithMany(n => n.PerfumeNotes)
            .HasForeignKey(pn => pn.NoteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
