using EFxceptions;
using EssayCheek.Api.Model.Foundation.EssayResults;
using EssayCheek.Api.Model.Foundation.Essays;
using EssayCheek.Api.Model.Foundation.Users;
using Microsoft.EntityFrameworkCore;

namespace EssayCheek.Api.Brokers.StorageBroker;

public sealed partial class StorageBroker : EFxceptionsContext
{
    private readonly IConfiguration _configuration;

    public StorageBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUser(modelBuilder.Entity<User>());
        ConfigureEssay(modelBuilder.Entity<Essay>());
        ConfigEssayResult(modelBuilder.Entity<EssayResult>());
        
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