using Catalog.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.Configuration
{
    public class LiteratureConfiguration : IEntityTypeConfiguration<Literature>
    {
        public void Configure(EntityTypeBuilder<Literature> builder)
        {
            builder.ToTable("literature");

            builder.Property(e => e.Id)
                .UseIdentityColumn()
                .IsRequired();

            builder.HasIndex(e => e.Isbn, "UK_literature")
                .IsUnique();

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");

            builder.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Isbn)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");

            builder.HasOne(d => d.Author)
                .WithMany(p => p.Literatures)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_literature_authors");

            builder.HasOne(d => d.Genre)
                .WithMany(p => p.Literatures)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_literature_genres");

            builder.HasOne(d => d.Publisher)
                .WithMany(p => p.Literatures)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_literature_publishers");

            builder.HasOne(d => d.Type)
                .WithMany(p => p.Literatures)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_literature_types");

            builder.HasMany(d => d.Writings)
                .WithMany(p => p.Literatures)
                .UsingEntity<Dictionary<string, object>>(
                    "LiteratureHasWriting",
                    l => l.HasOne<Writing>().WithMany().HasForeignKey("WritingId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_literature_has_writings_writings"),
                    r => r.HasOne<Literature>().WithMany().HasForeignKey("LiteratureId").HasConstraintName("FK_literature_has_writings_literature"),
                    j =>
                    {
                        j.HasKey("LiteratureId", "WritingId");

                        j.ToTable("literature_has_writings");
                    });
        }
    }
}
