using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.EssayResult;

namespace EssayCheek.Api.Services.EssayResults;

public interface IEssayResultService
{
    IQueryable<EssayResult> GetAllEssayResults();
    ValueTask<EssayResult> AddEssayResultsAsync(EssayResult essayResult);
    ValueTask<EssayResult> RemoveEssayResultsAsync(EssayResult essayResult);
}