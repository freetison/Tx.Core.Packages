using System;
using Microsoft.EntityFrameworkCore;

namespace Tx.Core.DbContextFactory
{
    public interface IContextFactory<out TContext> where TContext : DbContext, IDisposable
    {
        TContext CreateForRead();

        TContext CreateForWrite();

        DbContext Create(DbContextOptions<DbContext> options);
    }


}