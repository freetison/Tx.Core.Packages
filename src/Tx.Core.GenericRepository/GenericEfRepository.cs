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

        public async Task<int> Count(Expression<Func<TEntity, bool>> match) => await DbContext.Set<TEntity>().CountAsync();

        public async Task<int> Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Update(TEntity entity, object key)
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

        public async Task<int> Delete(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<IQueryable<TEntity>> GetBy(Expression<Func<TEntity, bool>> match)
        {
           var result = await DbContext.Set<TEntity>().Where(match).ToListAsync();
           return result.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAll()
        {
            var data = await DbContext.Set<TEntity>().ToListAsync();
            return data.AsQueryable();
        }

        public async Task<IQueryable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = await GetAll();
            var data = includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
            return data.AsQueryable();
        }

        public async Task<bool> Exist(Expression<Func<TEntity, bool>> match)
        {
            var result = await DbContext.Set<TEntity>().FirstOrDefaultAsync(match);
            return result != default;
        }

        public async Task<int> Save() => await DbContext.SaveChangesAsync();

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