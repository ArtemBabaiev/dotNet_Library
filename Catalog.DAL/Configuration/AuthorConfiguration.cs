﻿using Catalog.DAL.Entity;
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
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");

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
