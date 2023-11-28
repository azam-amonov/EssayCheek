using EssayCheek.Api.Brokers.StorageBroker;
using EssayCheek.Api.Model.Essays;

namespace EssayCheek.Api.Services.Essays;

public class EssayService : IEssayService
{
    private readonly IStorageBroker _storageBroker;

    public EssayService(IStorageBroker storageBroker)
    {
        _storageBroker = storageBroker;
    }

    public IQueryable<Essay> GetAllAEssays() => _storageBroker.SelectAllEssays();

    public async ValueTask<Essay?> GetByIdAsync(Guid id) =>  await _storageBroker.SelectEssayByIdAsync(id);

    public async ValueTask<Essay> AddEssayAsync(Essay essay) => await _storageBroker.InsertEssayAsync(essay);

    public async ValueTask<Essay> DeleteEssayAsync(Essay essay) => await _storageBroker.DeleteEssayAsync(essay);
}