    
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tx.Core.GenericRepository
{
    
    public interface IGenericEFRepository<TEntity> where TEntity : class
    {
        Task<int> CountAsync(Expression<Func<TEntity, bool>> match);
        Task<int> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity, object key);
        Task<int> DeleteAsync(TEntity entity);
        Task<IQueryable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> match);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> ExistAsync(Expression<Func<TEntity, bool>> match);
        
        Task<int> SaveAsync();
        void Dispose();
        
    }
}