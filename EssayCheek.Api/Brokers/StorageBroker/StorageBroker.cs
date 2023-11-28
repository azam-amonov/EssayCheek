using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial class StorageBroker : EFxceptionsContext
{
    private readonly IConfiguration _configuration;

    public StorageBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");
        
        builder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // todo: implement ConfiguringModelsR
        base.OnModelCreating(modelBuilder);
    }

    public async ValueTask<T> InsertAsync<T> (T @object)
    {
        var broker = new StorageBroker(_configuration);
        broker.Entry(@object!).State = EntityState.Added;
        await broker.SaveChangesAsync();
        
        return @object;
    }

    public async ValueTask<T?> SelectAsync<T>(params object[] objectIds) where T : class
    {
        var broker = new StorageBroker(_configuration);
        
        return await broker.FindAsync<T>(objectIds);
    }
    
    public IQueryable<T> SelectAll<T>() where T : class
    {
        var broker = new StorageBroker(_configuration);

        return broker.Set<T>();
    }

    public async ValueTask<T> UpdateAsync<T>(T @object)
    {
       var broker = new StorageBroker(_configuration);
       broker.Entry(@object!).State = EntityState.Modified;
       await broker.SaveChangesAsync();
       
       return @object;
    }

    public async ValueTask<T> DeleteAsync<T>(T @object)
    {
        var broker = new StorageBroker(_configuration);
        broker.Entry(@object!).State = EntityState.Deleted;
        await broker.SaveChangesAsync();
        
        return @object;
    }
}