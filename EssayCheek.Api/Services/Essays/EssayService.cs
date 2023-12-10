using EssayCheek.Api.Brokers.DateTimes;
using EssayCheek.Api.Brokers.Logging;
using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Foundation.Essays;

namespace EssayCheek.Api.Services.Essays;

public partial class EssayService : IEssayService
{
    private readonly IStorageBroker _storageBroker;
    private readonly IDateTimeBroker _dateTimeBroker;
    private readonly ILoggingBroker _loggingBroker;

    public EssayService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
    {
        _storageBroker = storageBroker;
        _dateTimeBroker = dateTimeBroker;
        _loggingBroker = loggingBroker;
    }

    public ValueTask<Essay> AddEssayAsync(Essay essay) => 
    TryCatch(async () =>
    {
        ValidateEssay(essay);
        return await _storageBroker.InsertEssayAsync(essay);
    });
    
    public IQueryable<Essay> RetrieveAllEssays() => 
                    TryCatch( () => _storageBroker.SelectAllEssays());

    public ValueTask<Essay?> RetrieveEssayByIdAsync(Guid essayId) =>
    TryCatch(async () =>
    {
        ValidateEssayId(essayId);
        Essay? maybeEssay = await _storageBroker.SelectEssayByIdAsync(essayId);
        
        ValidateStorageEssay(maybeEssay!, essayId);
        return maybeEssay;

    });

    public ValueTask<Essay> RemoveEssayByIdAsync(Guid id) =>
    TryCatch(async () =>
    {
        Essay? retrievedEssay = await _storageBroker.SelectEssayByIdAsync(id);
        await _storageBroker.DeleteEssayAsync(retrievedEssay!);

        return retrievedEssay;
    });
}