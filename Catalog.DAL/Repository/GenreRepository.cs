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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Genre> GetCompleteEntityAsync(long id)
        {
            var genre = await table.Include(entity => entity.Literatures)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return genre;
        }
    }
}
