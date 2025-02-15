Basic implementation

Exmaple:

1. Define Contexts in DI container
```<language>
    services.AddScoped<IDbContextFactory<DbContextOne>, DbContextFactory<DbContextOne>>();
    services.AddScoped<IDbContextFactory<DbContextTwo>, DbContextFactory<DbContextTwo>>();
```


2. Inject in a class
```<language>
public class DbLogService : IDbLogService
{
    private readonly IDbContextFactory<DbContextOne> _dbContextFactory;
    
    public DbLogService(IDbContextFactory<DbContextOne> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task SaveAsync()
    {
        await using var dbContext = _dbContextFactory.CreateForWrite(dbCnnStr);
        await dbContext.AccessLogs.AddAsync(accessLogs).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);

    }
}
```


 