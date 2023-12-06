using EssayCheek.Api.Model.Foundation.EssayResults;

namespace EssayCheek.Api.Services.EssayResults;

public interface IEssayResultService
{
    IQueryable<EssayResult> RetrieveAllEssayResults();
    ValueTask<EssayResult> AddEssayResultsAsync(EssayResult essayResult);
    ValueTask<EssayResult> RemoveEssayResultsAsync(EssayResult essayResult);
}