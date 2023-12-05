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
    
    public IQueryable<Essay> GetAllAEssays() => _storageBroker.SelectAllEssays();

    public async ValueTask<Essay?> GetByIdAsync(Guid id) =>  await _storageBroker.SelectEssayByIdAsync(id);

    public async ValueTask<Essay> DeleteEssayAsync(Essay essay) => await _storageBroker.DeleteEssayAsync(essay);
}