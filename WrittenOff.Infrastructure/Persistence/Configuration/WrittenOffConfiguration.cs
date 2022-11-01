using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;

namespace dotWrittenOff.Infrastructure.Persistence.Configuration
{
    public class WrittenOffConfiguration : IEntityTypeConfiguration<WrittenOff>
    {
        public void Configure(EntityTypeBuilder<WrittenOff> builder)
        {
            builder.ToTable("writtenOffs");

            builder.Property(e => e.Id)
                    .UseIdentityColumn()
                    .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);


            builder.Property(e => e.Isbn)
                    .HasMaxLength(255)
                    .IsUnicode(false);

            builder.HasIndex(e => e.Isbn, "UK_writtenOffs")
                .IsUnique();

            builder.OwnsOne(writtenOff => writtenOff.Author,
                            navigationBuilder =>
                            {
                                navigationBuilder.Property(author => author.Name)
                                                 .HasColumnName("AuthorName");
                                navigationBuilder.Property(author => author.Descritption)
                                                 .HasColumnName("AuthorDescritption");
                            });
            builder.OwnsOne(writtenOff => writtenOff.Publisher,
                            navigationBuilder =>
                            {
                                navigationBuilder.Property(publisher => publisher.Name)
                                                 .HasColumnName("PublisherName");
                                navigationBuilder.Property(publisher => publisher.Descritption)
                                                 .HasColumnName("PublisherDescritption");
                            });

            builder.HasOne(d => d.Employee)
                .WithMany(p => p.WrittenOffs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_writenOffs_employees");

            builder.Property(e => e.CreatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        }
    }
}
