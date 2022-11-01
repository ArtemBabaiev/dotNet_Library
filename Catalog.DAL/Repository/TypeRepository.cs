using Catalog.DAL.Entity;
using Catalog.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = Catalog.DAL.Entity.Type;

namespace Catalog.DAL.Repository
{
    public class TypeRepository : GenericRepository<Type>, ITypeRepository
    {
        public TypeRepository(dotNet_CatalogContext databaseContext)
            : base(databaseContext)
        { }

        public override async Task<Type> GetCompleteEntityAsync(long id)
        {
            var type = await table.Include(entity => entity.Literatures)
                                     .SingleOrDefaultAsync(entity => entity.Id == id);
            return type;
        }
    }
}
