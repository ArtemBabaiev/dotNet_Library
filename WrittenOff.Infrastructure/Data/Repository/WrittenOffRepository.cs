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
    public class WrittenOffRepository : GenericRepository<WrittenOff>, IWrittenOffRepository
    {
        public WrittenOffRepository(dotNet_WrittenOffManagmentContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<WrittenOff> GetCompleteEntityAsync(long id)
        {
            var writtenOff = await table.Include(entity => entity.Employee)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return writtenOff;
        }
    }
}
