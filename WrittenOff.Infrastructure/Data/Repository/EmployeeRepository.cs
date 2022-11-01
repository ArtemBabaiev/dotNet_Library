using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.Entities;
using WrittenOffManagement.Domain.Interface;
using WrittenOffManagement.Infrastructure.Persistence;

namespace WrittenOffManagement.Infrastructure.Data.Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(dotNet_WrittenOffManagmentContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Employee> GetCompleteEntityAsync(long id)
        {
            var employee = await table.Include(entity => entity.WrittenOffs)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return employee;
        }
    }
}
