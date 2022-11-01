using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;

namespace WrittenOffManagement.Infrastructure.Persistence.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("employees");

            builder.Property(e => e.Id)
                    .UseIdentityColumn()
                    .IsRequired();

            builder.OwnsOne(e => e.Name,
                            navigationBuilder =>
                            {
                                navigationBuilder.Property(name => name.FirstName)
                                                 .HasColumnName("FirstName");
                                navigationBuilder.Property(name => name.LastName)
                                                 .HasColumnName("LastName");
                            });


            builder.Property(e => e.CreatedAt).HasColumnType("datetime");
            builder.Property(e => e.UpdatedAt).HasColumnType("datetime");
        }
    }
}
