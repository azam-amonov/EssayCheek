using EssayCheek.Api.Model.EssayResult;
using Microsoft.EntityFrameworkCore;

namespace EssayCheek.Api.Brokers.StorageBroker;

public sealed partial class StorageBroker : IStorageBroker
{
    private DbSet<EssayResult> EssayResults => Set<EssayResult>();
    
    public IQueryable<EssayResult> SelectAllEssayResults() => SelectAll<EssayResult>();
    
    public async ValueTask<EssayResult> InsertEssayResultAsync(EssayResult result) 
        => await InsertAsync<EssayResult>(result);

    public async ValueTask<EssayResult> DeleteEssayResultAsync(EssayResult result) 
        => await DeleteAsync<EssayResult>(result);
}