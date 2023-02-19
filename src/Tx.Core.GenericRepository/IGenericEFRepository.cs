    
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tx.Core.GenericRepository
{
    
    public interface IGenericEFRepository<TEntity> where TEntity : class
    {
        Task<int> Count(Expression<Func<TEntity, bool>> match);
        Task<int> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity, object key);
        Task<int> Delete(TEntity entity);
        Task<IQueryable<TEntity>> GetBy(Expression<Func<TEntity, bool>> match);
        Task<IQueryable<TEntity>> GetAll();
        Task<IQueryable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<bool> Exist(Expression<Func<TEntity, bool>> match);
        
        Task<int> Save();
        void Dispose();
        
    }
}