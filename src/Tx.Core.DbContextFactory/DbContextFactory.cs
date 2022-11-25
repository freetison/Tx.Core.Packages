using System;
using Microsoft.EntityFrameworkCore;

namespace Tx.Core.DbContextFactory;

public class DbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
{
    private readonly DbContextOptionsBuilder<TContext> _optionsBuilder = new DbContextOptionsBuilder<TContext>();

    public TContext CreateForRead(string connectionString)
    {
        TContext forRead = this.Create(connectionString);
        forRead.ChangeTracker.AutoDetectChangesEnabled = false;
        forRead.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        return forRead;
    }

    public TContext CreateForWrite(string connectionString) => this.Create(connectionString);

    public DbContext Create(DbContextOptions<DbContext> options) => (DbContext)Activator.CreateInstance(typeof(DbContext), (object)options);

    private TContext Create(string connectionString)
    {
        this._optionsBuilder.UseSqlServer<TContext>(connectionString);
        return (TContext)Activator.CreateInstance(typeof(TContext), (object)this._optionsBuilder.Options) ?? throw new NullReferenceException("Create must not return null.");
    }
}