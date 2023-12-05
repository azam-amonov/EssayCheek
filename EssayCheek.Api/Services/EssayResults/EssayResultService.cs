using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Foundation.EssayResult;

namespace EssayCheek.Api.Services.EssayResults;

public partial class EssayResultService: IEssayResultService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public EssayResultService(IStorageBroker storageBroker, ILoggingBroker loggingBroker, IDateTimeBroker dateTimeBroker)
    {
        _storageBroker = storageBroker;
        _loggingBroker = loggingBroker;
        _dateTimeBroker = dateTimeBroker;
    }

    public IQueryable<EssayResult> GetAllEssayResults() => _storageBroker.SelectAllEssayResults();

    public ValueTask<EssayResult> AddEssayResultsAsync(EssayResult essayResult) =>
    TryCatch(async () =>
    {
        ValidateEssayResult(essayResult);
        return await _storageBroker.InsertEssayResultAsync(essayResult);
    });

    public async ValueTask<EssayResult> RemoveEssayResultsAsync(EssayResult essayResult) 
        => await _storageBroker.DeleteEssayResultAsync(essayResult);
}