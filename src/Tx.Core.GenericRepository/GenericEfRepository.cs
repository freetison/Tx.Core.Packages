using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Tx.Core.GenericRepository
{
    public abstract class GenericEFRepository<TDbContext, TEntity> : IGenericEFRepository<TEntity> where TEntity : class
        where TDbContext  : DbContext
    {
        private bool _disposed;
        private DbContext DbContext { get; }

        protected GenericEFRepository(DbContext dbContext) => DbContext = dbContext;

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> match) => DbContext.Set<TEntity>().CountAsync();

        public async Task<int> AddAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, object key)
        {
            if (entity == null) return null;
            var exist = await DbContext.Set<TEntity>().FindAsync(key);
            if (exist != null)
            {
                DbContext.Entry(exist).CurrentValues.SetValues(entity); 
                await DbContext.SaveChangesAsync();
            }

            return exist;
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> match)
        {
           var result = await DbContext.Set<TEntity>().Where(match).ToListAsync().ConfigureAwait(false);
           return result.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            var data = await DbContext.Set<TEntity>().ToListAsync();
            return data.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = await GetAllAsync();
            var data = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
            return data.AsQueryable();
        }

        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> match)
        {
            var result = await DbContext.Set<TEntity>().FirstOrDefaultAsync(match);
            return result != default;
        }

        public async Task<int> SaveAsync() => await DbContext.SaveChangesAsync().ConfigureAwait(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) DbContext.Dispose();
            _disposed = true;
        }
    }
}