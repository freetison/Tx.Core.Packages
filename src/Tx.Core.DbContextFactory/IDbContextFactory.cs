using Microsoft.EntityFrameworkCore;

using System;

namespace Tx.Core.DbContextFactory;

public interface IDbContextFactory<out TContext> where TContext : DbContext, IDisposable
{
    TContext CreateForRead(string connectionString);

    TContext CreateForWrite(string connectionString);

    DbContext Create(DbContextOptions<DbContext> options);
}