using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Type = Catalog.DAL.Entity.Type;

namespace Catalog.DAL.Configuration
{
    public class TypeConfiguration : IEntityTypeConfiguration<Type>
    {
        public void Configure(EntityTypeBuilder<Type> builder)
        {
            builder.ToTable("types");

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
        }
    }
}
