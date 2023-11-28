using EssayCheek.Api.Model.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayCheek.Api.Brokers.StorageBroker;

public sealed partial class StorageBroker : IStorageBroker
{
    private DbSet<Essay> Essays => Set<Essay>();
    
    public async ValueTask<Essay> InsertEssayAsync(Essay essay) => await InsertAsync<Essay>(essay);

    public IQueryable<Essay> SelectAllEssays() => SelectAll<Essay>();

    public async ValueTask<Essay?> SelectEssayByIdAsync(Guid id) => await SelectAsync<Essay>(id);

    public async ValueTask<Essay> DeleteEssayAsync(Essay essay) => await DeleteAsync(essay);

}