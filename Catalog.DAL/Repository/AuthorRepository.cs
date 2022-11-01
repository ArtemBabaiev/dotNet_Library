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
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        {}

        public override async Task<Author> GetCompleteEntityAsync(long id)
        {
            var author = await table.Include(entity => entity.Writings)
                                     .Include(entity => entity.Literatures)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return author;
        }
    }
}
