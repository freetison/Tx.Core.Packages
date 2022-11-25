using Microsoft.EntityFrameworkCore;

namespace Tx.Core.DbContextFactory;

public interface IDbContextFactory<out TContext> where TContext : DbContext
{
    TContext CreateForRead(string connectionString);

    TContext CreateForWrite(string connectionString);

    DbContext Create(DbContextOptions<DbContext> options);
}