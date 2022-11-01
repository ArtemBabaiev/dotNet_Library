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
    public class ExemplarRepository : GenericRepository<Exemplar>, IExemplarRepository
    {
        public ExemplarRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Exemplar> GetCompleteEntityAsync(long id)
        {
            var exemplar = await table.Include(entity => entity.Literature)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return exemplar;
        }
    }
}
