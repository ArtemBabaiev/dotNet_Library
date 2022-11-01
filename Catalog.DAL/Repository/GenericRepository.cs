using Catalog.DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.DAL.Repository
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly dotNet_CatalogContext databaseContext;

        protected readonly DbSet<TEntity> table;

        public GenericRepository(dotNet_CatalogContext databaseContext)
        {
            this.databaseContext = databaseContext;
            table = this.databaseContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await table.ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(long id)
        {
            return await table.FindAsync(id);
        }

        public abstract Task<TEntity> GetCompleteEntityAsync(long id);

        public virtual async Task InsertAsync(TEntity entity)
        {
            await table.AddAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => table.Update(entity));
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            await Task.Run(() => table.Remove(entity));
        }

    }
}
