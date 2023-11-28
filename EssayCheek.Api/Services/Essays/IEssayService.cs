using EssayCheek.Api.Model.Essays;

namespace EssayCheek.Api.Services.Essays;

public interface IEssayService
{
    IQueryable<Essay> GetAllAEssays();
    ValueTask<Essay?> GetByIdAsync(Guid id);
    ValueTask<Essay> AddEssayAsync(Essay essay);
    ValueTask<Essay> DeleteEssayAsync(Essay essay);
}