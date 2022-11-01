using Catalog.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.Repository.Interface
{
    public interface IWritingRepository : IGenericRepository<Writing>
    {
        public Task<IEnumerable<Writing>> GetWritingsWithAuthors(long authorId);
    }
}
