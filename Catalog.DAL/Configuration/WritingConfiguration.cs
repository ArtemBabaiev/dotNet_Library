using Catalog.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.Configuration
{
    public class WritingConfiguration : IEntityTypeConfiguration<Writing>
    {
        public void Configure(EntityTypeBuilder<Writing> builder)
        {
            builder.ToTable("writings");

            builder.Property(e => e.Id)
                    .UseIdentityColumn()
                    .IsRequired();

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");

            builder.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

            builder.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");

            builder.HasMany(d => d.Authors)
                    .WithMany(p => p.Writings)
                    .UsingEntity<Dictionary<string, object>>(
                        "WritingHasAuthor",
                        l => l.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_writing_has_authors_authors"),
                        r => r.HasOne<Writing>().WithMany().HasForeignKey("WritingId").HasConstraintName("FK_writing_has_authors_writings"),
                        j =>
                        {
                            j.HasKey("WritingId", "AuthorId");

                            j.ToTable("writing_has_authors");
                        });
        }
    }
}
