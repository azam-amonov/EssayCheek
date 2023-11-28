using EssayCheek.Api.Model.Essays;

namespace EssayCheek.Api.Brokers.StorageBroker;

public partial interface IStorageBroker
{
    ValueTask<Essay> InsertEssayAsync(Essay entity);
    IQueryable<Essay> SelectAllEssays();
    ValueTask<Essay?> SelectEssayByIdAsync (Guid id);
    ValueTask<Essay> DeleteEssayAsync (Essay essay);
}