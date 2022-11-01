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
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Publisher> GetCompleteEntityAsync(long id)
        {
            var publisher = await table.Include(entity => entity.Literatures)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return publisher;
        }
    }
}
