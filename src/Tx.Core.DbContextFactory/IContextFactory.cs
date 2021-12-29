using Microsoft.EntityFrameworkCore;

namespace Tx.Core.DbContextFactory
{
    public interface IContextFactory<out TContext> where TContext : DbContext
    {
        TContext CreateForRead();

        TContext CreateForWrite();

        DbContext Create(DbContextOptions<DbContext> options);
    }
}