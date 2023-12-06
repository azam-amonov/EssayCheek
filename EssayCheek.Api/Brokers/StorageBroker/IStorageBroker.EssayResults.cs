using EssayCheek.Api.Model.Foundation.EssayResults;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial interface IStorageBroker
{
    ValueTask<EssayResult> InsertEssayResultAsync(EssayResult result);
    IQueryable<EssayResult> SelectAllEssayResults();
    ValueTask<EssayResult> DeleteEssayResultAsync(EssayResult result);
}