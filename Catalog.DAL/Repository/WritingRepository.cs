using Catalog.DAL.Entity;
using Catalog.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.Repository
{
    public class WritingRepository : GenericRepository<Writing>, IWritingRepository
    {
        public WritingRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Writing> GetCompleteEntityAsync(long id)
        {
            var writing = await table.Include(entity => entity.Literatures)
                                     .Include(entity => entity.Authors)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return writing;
        }

        public async Task<IEnumerable<Writing>> GetWritingsWithAuthors(long authorId)
        {
            return await Task<IEnumerable<Writing>>.Run(() => from writings in table
                                                              where writings.Authors.Any(a => a.Id == authorId)
                                                              select writings);
        }
    }
}
