using System;
using Microsoft.EntityFrameworkCore;

namespace Tx.Core.DbContextFactory
{
    public class ContextFactory<TContext> : IContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _connectionString;
        private readonly DbContextOptionsBuilder<TContext> _optionsBuilder = new DbContextOptionsBuilder<TContext>();

        public ContextFactory(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public TContext CreateForRead()
        {
            var context = Create();
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return context;
        }

        public TContext CreateForWrite() => Create();

        public DbContext Create(DbContextOptions<DbContext> options) => (DbContext)Activator.CreateInstance(typeof(DbContext), options);

        private TContext Create()
        {
            _optionsBuilder.UseSqlServer(_connectionString);
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _optionsBuilder.Options);
            if (context == null) { throw new NullReferenceException($"{nameof(Create)} must not return null."); }

            return context;

        }
    }
}