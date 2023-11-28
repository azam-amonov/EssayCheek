using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.EssayResult;

namespace EssayCheek.Api.Services.EssayResults;

public class EssayResultService: IEssayResultService
{
    private readonly IStorageBroker _storageBroker;

    public EssayResultService(IStorageBroker storageBroker)
    {
        _storageBroker = storageBroker;
    }

    public IQueryable<EssayResult> GetAllEssayResults() => _storageBroker.SelectAllEssayResults();

    public async ValueTask<EssayResult> AddEssayResultsAsync(EssayResult essayResult) =>
                   await _storageBroker.InsertEssayResultAsync(essayResult);

    public async ValueTask<EssayResult> RemoveEssayResultsAsync(EssayResult essayResult) 
        => await _storageBroker.DeleteEssayResultAsync(essayResult);
}