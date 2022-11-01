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
    public class LiteratureRepository : GenericRepository<Literature>, ILiteratureRepository
    {
        public LiteratureRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Literature> GetCompleteEntityAsync(long id)
        {
            var literature = await table.Include(entity => entity.Genre)
                                     .Include(entity => entity.Author)
                                     .Include(entity => entity.Exemplars)
                                     .Include(entity => entity.Publisher)
                                     .Include(entity => entity.Type)
                                     .Include(entity => entity.Writings)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return literature;
        }
        public async Task<IEnumerable<Literature>> GetLiteratureWithAuthor(long authorId)
        {
            await Task.Run(() =>  table.Where(lit => lit.AuthorId == authorId).Load());
            return table;
        }

    }
}
