using EssayCheek.Api.Model.Foundation.Essays;

namespace EssayCheek.Api.Services.Essays;

public interface IEssayService
{
    IQueryable<Essay> RetrieveAllEssays();
    ValueTask<Essay?> RetrieveGetByIdAsync(Guid id);
    ValueTask<Essay> AddEssayAsync(Essay essay);
    ValueTask<Essay> RemoveEssayAsync(Guid id);
}