using EssayCheek.Api.Model.EssayResult;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial interface IStorageBroker
{
    ValueTask<EssayResult> InsertEssayResultAsync(EssayResult result);
    IQueryable<EssayResult> SelectAllEssayResults();
    ValueTask<EssayResult> DeleteEssayResultAsync(EssayResult result);
}