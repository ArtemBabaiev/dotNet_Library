using dotWrittenOff.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;
using WrittenOffManagement.Infrastructure.Persistence.Configuration;

namespace WrittenOffManagement.Infrastructure.Persistence
{
    public partial class dotNet_WrittenOffManagmentContext : DbContext
    {
        public dotNet_WrittenOffManagmentContext()
        {
        }

        public dotNet_WrittenOffManagmentContext(DbContextOptions<dotNet_WrittenOffManagmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Authors { get; set; } = null!;
        public virtual DbSet<WrittenOff> Exemplars { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new WrittenOffConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
